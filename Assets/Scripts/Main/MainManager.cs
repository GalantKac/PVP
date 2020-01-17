using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public Transform playerContainer;

    void Start()
    {
        LoggedInPlayer.instance.networkManager.playersContainer = playerContainer;
        LoggedInPlayer.instance.networkManager.JoinToGame();
    }
}
