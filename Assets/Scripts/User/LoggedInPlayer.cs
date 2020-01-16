using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoggedInPlayer : MonoBehaviour
{
    static public LoggedInPlayer instance = null;
    public NetworkManager networkManager = null;

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
}
