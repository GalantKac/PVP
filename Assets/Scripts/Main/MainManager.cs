using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public Transform playerContainer;
    public Transform spawnPointLeft;
    public Transform spawnPointRight;
    void Start()
    {
        LoggedInPlayer.instance.networkManager.parent = playerContainer;
        LoggedInPlayer.instance.networkManager.spawnPoints[0] = spawnPointLeft;
        LoggedInPlayer.instance.networkManager.spawnPoints[1] = spawnPointRight;
        LoggedInPlayer.instance.networkManager.JoinToGame();
    }
}
