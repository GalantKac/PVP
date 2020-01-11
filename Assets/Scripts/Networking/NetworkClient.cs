using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using Project.Utility;

namespace Project.Networiking
{
    public class NetworkClient : SocketIOComponent
    {
        [Header("NetworkClient")]
        [SerializeField]
        private Transform playersContainer;

        private Dictionary<string, GameObject> serverObjects;
        public override void Start()
        {
            base.Start();
            initialize();
            setupEvents();
        }
        public override void Update()
        {
            base.Update();
        }

        private void initialize()
        {
            serverObjects = new Dictionary<string, GameObject>();
        }

        private void setupEvents()
        {
            On("open", (e) =>
            {
                Debug.Log("User connected with server");
            });

            On("register", (e) =>
            {
                string id = e.data["id"].ToString().RemoveQuotes();

                Debug.LogFormat("Our Clients's ID ({0})", id);
            });

            On("spawn", (e) =>
            {
                string id = e.data["id"].ToString().RemoveQuotes();

                GameObject newPlayer = new GameObject("Server ID: " + id);
                newPlayer.transform.SetParent(playersContainer);
                serverObjects.Add(id, newPlayer);
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
    }
}
