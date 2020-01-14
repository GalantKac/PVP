using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    string loginUrl = "http://127.0.0.1:3000/login";

    [SerializeField]
    TextMeshProUGUI inputEmail;
    [SerializeField]
    TextMeshProUGUI inputPassword;
    [SerializeField]
    GameObject warrning;
    public void Start()
    {
        warrning.SetActive(false);
    }

    public void Login()
    {
        User loginUser = new User(inputEmail.text, inputPassword.text);
        StartCoroutine(PostLogin(loginUrl, loginUser));
        // SceneManager.LoadScene("Menu");
    }

    public void Register()
    {
        SceneManager.LoadScene("Register");
    }

    public void Exit()
    {
        Application.Quit();
    }

    private IEnumerator PostLogin(string url, User user)
    {
        var jsonData = JsonUtility.ToJson(user);
        Debug.Log(jsonData);

        UnityWebRequest www = UnityWebRequest.Post(url, jsonData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }
}
