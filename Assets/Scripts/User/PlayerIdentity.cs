using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Networiking
{

    public class PlayerIdentity : MonoBehaviour
    {
        [SerializeField]
        private string id;
        [SerializeField]
        private bool isControling;

        private SocketIOComponent socket;

        public void Awake()
        {
            isControling = false;
        }
        public void SetControllerID(string ID)
        {
            id = ID;
            isControling = (NetworkManager.ClientID == id) ? true : false;
        }

        public void SetSocketRef(SocketIOComponent socketIOComponent)
        {
            socket = socketIOComponent;
        }

        public string GetId()
        {
            return id;
        }

        public bool IsControlling()
        {
            return isControling;
        }

        public SocketIOComponent GetSocket()
        {
            return socket;
        }
    }
}
