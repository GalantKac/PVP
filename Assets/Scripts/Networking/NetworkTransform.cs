using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Networiking
{
    [RequireComponent(typeof(PlayerIdentity))]
    public class NetworkTransform : MonoBehaviour
    {
        [SerializeField]
        private Vector3 oldPosition;

        public User user;

        private float timeToRefresh = 0;

        private PlayerIdentity playerIdentity;

        public void Start()
        {
            playerIdentity = GetComponent<PlayerIdentity>();
            oldPosition = transform.position;
            user = new User();
            user.x = "0.0";
            user.y = "0.0";

            if (playerIdentity.IsControlling().Equals(false))
            {
                enabled = false;
            }
        }

        private void Update()
        {
            if (playerIdentity.IsControlling())
            {
                if (oldPosition != transform.position)
                {
                    oldPosition = transform.position;
                    timeToRefresh = 0;
                    SendPosition();
                }
                else
                {
                    timeToRefresh += Time.deltaTime;

                    if (timeToRefresh >= 1)
                    {
                        timeToRefresh = 0;
                        SendPosition();
                    }
                }
            }
        }

        private void SendPosition()
        {
            user.x = transform.position.x.ToString();
            user.y = transform.position.y.ToString();
            LoggedInPlayer.instance.networkManager.UpdatePosition(user);
        }

        public void SendRotation(float x)
        {
            user.rotationX = Mathf.RoundToInt(x);
            Debug.Log("Rotation:" + user.rotationX);
            LoggedInPlayer.instance.networkManager.UpdateRotation(user);
        }

        public void SendAnimationState()
        {

        }
    }
}
