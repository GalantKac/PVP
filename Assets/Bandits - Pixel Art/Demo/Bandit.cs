﻿using UnityEngine;
using System.Collections;
using Project.Networiking;

public class Bandit : MonoBehaviour
{

    [SerializeField] float m_speed = 1.0f;
    [SerializeField] float m_jumpForce = 2.0f;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Bandit m_groundSensor;
    [HideInInspector]
    public bool m_grounded = false;
    private bool m_isDead = false;

    [Header("Referenc this Class")]
    [SerializeField]
    private PlayerIdentity playerIdentity;

    [SerializeField]
    private NetworkTransform networkTransform;

    [Header("Player Attack")]
    public Transform attackPosition;
    public LayerMask whoIsEnemy;
    public float attackRange;
    public int damage;

    // Use this for initialization
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        networkTransform = GetComponent<NetworkTransform>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIdentity.IsControlling())
        {
            //Check if character just landed on the ground
            if (!m_grounded && m_groundSensor.State())
            {
                m_grounded = true;
                networkTransform.user.grounded = true;
                m_animator.SetBool("Grounded", m_grounded);
            }

            //Check if character just started falling
            if (m_grounded && !m_groundSensor.State())
            {
                m_grounded = false;
                networkTransform.user.grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
            }

            // -- Handle input and movement --
            float inputX = Input.GetAxis("Horizontal");

            // Swap direction of sprite depending on walk direction
            if (inputX > 0 && !m_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                transform.localScale = new Vector3(-4.0f, 4.0f, 1.0f);
                networkTransform.SendRotation(-4f);
            }
            else if (inputX < 0 && !m_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
                networkTransform.SendRotation(4f);
            }
            // Move
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

            //Set AirSpeed in animator
            m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

            // -- Handle Animations --
            //Death
            if (Input.GetKeyDown("e"))
            {
                if (!m_isDead)
                {
                    m_animator.SetTrigger("Death");
                    networkTransform.user.animState = AnimState.Death.ToString();
                    networkTransform.SendAnimationState(networkTransform.user.animState, true);
                }
                else
                {
                    m_animator.SetTrigger("Recover");
                }
                m_isDead = !m_isDead;
            }
            //Hurt
            else if (Input.GetKeyDown("q"))
            {
                m_animator.SetTrigger("Hurt");
                networkTransform.user.animState = AnimState.Hurt.ToString();
                networkTransform.SendAnimationState(networkTransform.user.animState, true);
            }
            //Attack
            else if (Input.GetMouseButtonDown(0) && !m_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && !m_animator.GetCurrentAnimatorStateInfo(0).IsTag("Run"))
            {
                if (m_grounded)// jesli jest na ziemi
                {
                    m_animator.SetTrigger("Attack");
                    networkTransform.user.animState = AnimState.Attack.ToString();
                    networkTransform.SendAnimationState(networkTransform.user.animState, true);
                    //zadaj obrazenia wszystkim w tym obszarze i wyslij info do servera (zmien hp gracza, animacja jesli dostal, UI hp zmien)
                    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, whoIsEnemy);
                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        //if(user.isDeath) // jesli zginie wyslij inna informacje do servera
                        Debug.Log(enemiesToDamage[i].gameObject.name);
                        User hitUser = enemiesToDamage[i].GetComponent<NetworkTransform>().user;
                        hitUser.id = int.Parse(enemiesToDamage[i].gameObject.name);
                        LoggedInPlayer.instance.networkManager.UpdateHp(enemiesToDamage[i].GetComponent<NetworkTransform>().user);
                    }
                }
            }
            //Jump
            else if (Input.GetKeyDown("space") && m_grounded)
            {
                m_animator.SetTrigger("Jump");
                networkTransform.user.animState = AnimState.Jump.ToString();
                networkTransform.SendAnimationState(networkTransform.user.animState, false);
                m_grounded = false;
                networkTransform.user.grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                m_groundSensor.Disable(0.2f);
            }
            //Run
            else if (Mathf.Abs(inputX) > Mathf.Epsilon && m_grounded)
            {
                m_animator.SetInteger("AnimState", 2);
                networkTransform.user.animState = AnimState.Run.ToString();
                networkTransform.SendAnimationState(networkTransform.user.animState, true);
            }
            //Idle
            else
            {
                if (m_grounded)
                {
                    m_animator.SetInteger("AnimState", 0);
                    networkTransform.user.animState = AnimState.Idle.ToString();
                    networkTransform.SendAnimationState(networkTransform.user.animState, true);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }
}
