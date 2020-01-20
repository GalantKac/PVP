using Project.Utility;
using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Networiking
{
    public class NetworkManager : SocketIOComponent
    {
        [Header("Player Container")]
        public Transform playersContainer;

        [Header("Characters")]
        [SerializeField] GameObject playerPrefab;

        public static string ClientID { get; private set; }

        private Dictionary<string, PlayerIdentity> serverObjects;
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
            serverObjects = new Dictionary<string, PlayerIdentity>();
        }

        private void startEvents()
        {
            On("loginCompleted", (e) =>
            {
                string id = e.data["id"].ToString().RemoveQuotes();
                ClientID = id;
                string xs = e.data["x"].ToString().RemoveQuotes();
                string ys = e.data["y"].ToString().RemoveQuotes();
                float x = 1;
                float.TryParse(xs, out x);
                float y = 1;
                float.TryParse(ys, out y);
                Debug.Log("E: " + e);
                Debug.Log("e.data: " + e.data);
                Debug.Log("ID: " + id + " xf: " + x + " yf: " + y);
                Debug.Log("ID: " + id + " xs: " + xs + " ys: " + xs);
            });

            On("spawn", (e) =>
            {
                string id = e.data["id"].ToString().RemoveQuotes();
                GameObject playerInGame = Instantiate(playerPrefab, playersContainer);
                playerInGame.name = "Server ID: " + id;
                PlayerIdentity playerIdentity = playerInGame.GetComponent<PlayerIdentity>();
                playerIdentity.SetControllerID(id);
                playerIdentity.SetSocketRef(this);
                playerInGame.transform.SetParent(playersContainer);
                serverObjects.Add(id, playerIdentity);

                //playerInGame.GetComponent<NetworkTransform>().user.id = int.Parse(id);
                //playerInGame.GetComponent<NetworkTransform>().user.email = e.data["email"].ToString().RemoveQuotes();
                //playerInGame.GetComponent<NetworkTransform>().user.nick = e.data["nick"].ToString().RemoveQuotes();
                //playerInGame.GetComponent<NetworkTransform>().user.password = e.data["password"].ToString().RemoveQuotes();
                //playerInGame.GetComponent<NetworkTransform>().user.wins = int.Parse(e.data["wins"].ToString().RemoveQuotes());
                //playerInGame.GetComponent<NetworkTransform>().user.loses = int.Parse(e.data["loses"].ToString().RemoveQuotes());
                //playerInGame.GetComponent<NetworkTransform>().user.x = e.data["x"].ToString().RemoveQuotes();
                //playerInGame.GetComponent<NetworkTransform>().user.y = e.data["y"].ToString().RemoveQuotes();


                Debug.Log("Create Player");
            });

            On("updatePosition", (e) =>
            {
                string id = e.data["id"].ToString().RemoveQuotes();
                string xs = e.data["x"].ToString().RemoveQuotes();
                string ys = e.data["y"].ToString().RemoveQuotes();
                float x = 1;
                float.TryParse(xs, out x);
                float y = 1;
                float.TryParse(ys, out y);

                //Debug.Log("Position x: " + x);
                //Debug.Log("Position y: " + y);

                PlayerIdentity updatePlayerIdentity = serverObjects[id];
                updatePlayerIdentity.transform.position = new Vector3(x, y, 0);
            });

            On("updateRotation", (e) =>
            {
                string id = e.data["id"].ToString().RemoveQuotes();
                string xrotationX = e.data["rotationX"].ToString().RemoveQuotes();
                float x = 1;
                float.TryParse(xrotationX, out x);

                Debug.Log("Rotation:" + x);
                PlayerIdentity updatePlayerIdentity = serverObjects[id];
                updatePlayerIdentity.transform.localScale = new Vector3(x, 4f, 1f);
            });

            On("disconnected", (e) =>
            {
                string id = e.data["id"].ToString().RemoveQuotes();

                GameObject playerToRemove = serverObjects[id].gameObject;
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

        public void UpdateRotation(User user)
        {
            string jsonData = JsonUtility.ToJson(user);
            Emit("updateRotation", new JSONObject(jsonData), (e) =>
            {
                Debug.Log("Wysłano rotacje");
            });
        }
    }
}
