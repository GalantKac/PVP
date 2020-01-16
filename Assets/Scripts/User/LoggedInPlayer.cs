using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoggedInPlayer : MonoBehaviour
{
    static public LoggedInPlayer instance = null;
    [Header("Network Manager")]
    public NetworkManager networkManager = null;
    [Header("Player")]
    public GameObject yourPlayer = null;
    [Header("Player position")]
    public Vector3 oldPosition = Vector3.zero;
    private float refreshTimer = 0;

    [Header("Active Player")]
    public User user;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        networkManager = GetComponent<NetworkManager>();
    }

    private void Update()
    {
        if (yourPlayer != null)
        {
            if (oldPosition != yourPlayer.transform.position)
            {
                oldPosition = yourPlayer.transform.position;
                refreshTimer = 0;
                user.x = yourPlayer.transform.position.x.ToString();
                user.y = yourPlayer.transform.position.y.ToString();
                networkManager.UpdatePosition(user);
            }
            else
            {
                refreshTimer += Time.deltaTime;

                if (refreshTimer >= 1)
                {
                    refreshTimer = 0;
                    user.x = yourPlayer.transform.position.x.ToString();
                    user.y = yourPlayer.transform.position.y.ToString();
                    networkManager.UpdatePosition(user);
                }
            }
        }
    }
}
