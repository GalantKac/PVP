using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class NetworkTransform : MonoBehaviour
    {
        [SerializeField]
        private Vector3 oldPosition;

        public User user;

        private float timeToRefresh = 0;

        public void Start()
        {
            oldPosition = transform.position;
            user = new User();
            user.x = "0.0";
            user.y = "0.0";
        }

        private void Update()
        {
            if(oldPosition != transform.position)
            {
                oldPosition = transform.position;
                timeToRefresh = 0;
                SendData();
            }else
            {
                timeToRefresh += Time.deltaTime;

                if(timeToRefresh >= 1)
                {
                    timeToRefresh = 0;
                    SendData();
                }
            }
        }

        private void SendData()
        {
            user.x = transform.position.x.ToString();
            user.y = transform.position.y.ToString();
            LoggedInPlayer.instance.networkManager.UpdatePosition(user);
        }
    }
