using Project.Utility;
using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : SocketIOComponent
{
    public Dictionary<string, GameObject> serverObjects;

    [Header("Player Container")]
    public Transform parent;

    [Header("Characters")]
    [SerializeField] GameObject[] characters;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;
    public override void Start()
    {
        base.Start();
        initialize();
        startEvents();
    }
    public override void Update()
    {
        base.Update();
    }
    private void initialize()
    {
        serverObjects = new Dictionary<string, GameObject>();
    }

    private void startEvents()
    {
        On("spawn", (e) =>
        {
            string id = e.data["id"].ToString().RemoveQuotes();

            GameObject firstPlayer = Instantiate(characters[Random.Range(0, 2)], spawnPoints[Random.Range(0, 2)].position, Quaternion.identity);
            firstPlayer.name = "Server ID: " + id;
            firstPlayer.transform.SetParent(parent);
            serverObjects.Add(id, firstPlayer);

            LoggedInPlayer.instance.yourPlayer = firstPlayer;

            Debug.Log("Create Player");
        });

        On("update", (e) =>
        {
            string x = e.data["x"].ToString().RemoveQuotes();
            string y = e.data["y"].ToString().RemoveQuotes();
            Debug.Log("Position x: " + x);
            Debug.Log("Position y: " + y);
        });

        On("disconnected", (e) =>
        {
            string id = e.data["id"].ToString().RemoveQuotes();

            GameObject playerToRemove = serverObjects[id];
            Destroy(playerToRemove);
            serverObjects.Remove(id);
            Debug.Log("Usunalem");
        });
    }

    public void LogginPlayer(User dataUser)
    {
        string jsonData = JsonUtility.ToJson(dataUser);

        Emit("loginCompleted", new JSONObject(jsonData), (e) =>
        {
            Debug.Log("Zalogowano");
        });
    }

    public void JoinToGame()
    {
        Emit("join", (e) =>
        {
            Debug.Log("Dolaczyl do gry");
        });
    }

    public void UpdatePosition(User user)
    {
        //  Debug.Log("After send x: " + LoggedInPlayer.instance.user.position.x);
        //   Debug.Log("After send y: " + LoggedInPlayer.instance.user.position.y);
        string jsonData = JsonUtility.ToJson(user);
        Emit("updatePosition", new JSONObject(jsonData), (e) =>
        {
            Debug.Log("Wysłano pozycje");
        });
    }
}
