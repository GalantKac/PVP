using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.MenuManager
{
    public class MenuManager : MonoBehaviour
    {
        [Header("Server List")]
        [SerializeField]
        GameObject serverLsit;
        [SerializeField]
        Scrollbar serverListScrollbar;

        private void Start()
        {
            serverLsit.SetActive(false);
        }

        public void CreateServer()
        {
            SceneManager.LoadScene("Main");
        }

        public void ShowServerList()
        {
            serverLsit.SetActive(true);
            serverListScrollbar.value = 1;
        }

        public void CloseServerList()
        {
            serverLsit.SetActive(false);
        }

        public void JoinToServer()
        {
            //TODO
        }

        public void Options()
        {
            SceneManager.LoadScene("Options");
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
