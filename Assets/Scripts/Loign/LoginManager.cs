using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{

    [SerializeField]
    GameObject warrning;
    public void Start()
    {
        warrning.SetActive(false);  
    }

    public void Login()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Register()
    {
        SceneManager.LoadScene("Register");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
