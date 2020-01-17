using Project.Networiking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoggedInPlayer : MonoBehaviour
{
    static public LoggedInPlayer instance = null;
    [Header("Network Manager")]
    public NetworkManager networkManager = null;

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
