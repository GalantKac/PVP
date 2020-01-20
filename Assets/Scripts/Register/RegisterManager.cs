using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class RegisterManager : MonoBehaviour
{
   // string registerURL = "https://pvp-server.herokuapp.com/users";
    string registerURL = "http://localhost:3000/users";

    [SerializeField]
    TextMeshProUGUI inputEmail;
    [SerializeField]
    TextMeshProUGUI inputName;
    [SerializeField]
    TMP_InputField inputPassword;
    [SerializeField]
    GameObject warrningMessage;

    private void Start()
    {
        warrningMessage.SetActive(false);
    }

    public void CreateUser()
    {
        if (inputName.text == "" || inputPassword.text == "" || inputEmail.text == "")
        {
            warrningMessage.SetActive(true);
        }
        else
        {
            warrningMessage.SetActive(false);
            User newUser = new User(inputEmail.text.TrimEnd('\u200b'), inputPassword.text.TrimEnd('\u200b'), inputName.text.TrimEnd('\u200b'));
            StartCoroutine(RegisterUser(registerURL, newUser));
        }
    }

    public void BackToLogin()
    {
        SceneManager.LoadScene("Login");
    }

    private IEnumerator RegisterUser(string url, User user)
    {
        var jsonData = JsonUtility.ToJson(user);
        Debug.Log("JSONdata: " + jsonData);

        using (UnityWebRequest www = UnityWebRequest.Post(url, jsonData))
        {
            www.SetRequestHeader("content-type", "application/json");
            www.uploadHandler.contentType = "application/json";
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    Debug.Log("Created User");
                    SceneManager.LoadScene("Login");
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
