using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    string loginUrl = "http://127.0.0.1:3000/users/login";

    [SerializeField]
    TextMeshProUGUI inputEmail;
    [SerializeField]
    TMP_InputField inputPassword;
    [SerializeField]
    GameObject warrning;
    public void Start()
    {
        warrning.SetActive(false);
    }

    public void Login()
    {
        User loginUser = new User(inputEmail.text.TrimEnd('\u200b'), inputPassword.text.TrimEnd('\u200b'));
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

        using (UnityWebRequest www = UnityWebRequest.Post(url, jsonData))
        {
            www.SetRequestHeader("content-type", "application/json");
            www.uploadHandler.contentType = "application/json";
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    Debug.Log("Login");
                }
                else
                {
                    //handle the problem
                    Debug.Log("Error! data couldn't get.");
                }
            }
        }
    }
}
