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
    [SerializeField] GameObject playerPrefab;

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
            GameObject playerInGame = Instantiate(playerPrefab, parent);
            playerInGame.name = "Server ID: " + id;

            playerInGame.GetComponent<NetworkTransform>().user.id = int.Parse(id);
            playerInGame.GetComponent<NetworkTransform>().user.email = e.data["email"].ToString().RemoveQuotes();
            playerInGame.GetComponent<NetworkTransform>().user.nick = e.data["nick"].ToString().RemoveQuotes();
            playerInGame.GetComponent<NetworkTransform>().user.password = e.data["password"].ToString().RemoveQuotes();
            playerInGame.GetComponent<NetworkTransform>().user.wins = int.Parse(e.data["wins"].ToString().RemoveQuotes());
            playerInGame.GetComponent<NetworkTransform>().user.loses = int.Parse(e.data["loses"].ToString().RemoveQuotes());
            playerInGame.GetComponent<NetworkTransform>().user.x = e.data["x"].ToString().RemoveQuotes();
            playerInGame.GetComponent<NetworkTransform>().user.y = e.data["y"].ToString().RemoveQuotes();

            playerInGame.transform.SetParent(parent);
            serverObjects.Add(id, playerInGame);

            Debug.Log("Create Player");
        });

        On("updatePosition", (e) =>
        {
            string id = e.data["id"].ToString().RemoveQuotes();
            float x = e.data["x"].f;
            float y = e.data["y"].f;
            Debug.Log("Position x: " + x);
            Debug.Log("Position y: " + y);

            GameObject updatePositionObject = serverObjects[id];
            updatePositionObject.transform.position = new Vector3(x, y, 0);
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
