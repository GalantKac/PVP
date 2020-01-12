using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RegisterManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI inputName;
    [SerializeField]
    TextMeshProUGUI inputPassword;
    [SerializeField]
    GameObject warriningMessage;

    private void Start()
    {
        warriningMessage.SetActive(false);
    }

    public void CreateUser()
    {
        //TODO
    }

    public void BackToLogin()
    {
        SceneManager.LoadScene("Login");
    }
}
