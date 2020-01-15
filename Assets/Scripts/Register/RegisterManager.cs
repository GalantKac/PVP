using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class RegisterManager : MonoBehaviour
{
    string registerURL = "http://127.0.0.1:3000/users";
    string url = "http://127.0.0.1:3000/users/14";

    [Multiline]
    public string user;

    [SerializeField]
    TextMeshProUGUI inputEmail;
    [SerializeField]
    TextMeshProUGUI inputName;
    [SerializeField]
    TextMeshProUGUI inputPassword;
    [SerializeField]
    GameObject warriningMessage;

    private void Start()
    {
        warriningMessage.SetActive(false);
        StartCoroutine(Get(url));
    }

    public void CreateUser()
    {
        User newUser = new User(inputEmail.text, inputPassword.text);
        StartCoroutine(RegisterUser(registerURL, newUser));
    }

    public void BackToLogin()
    {
        SceneManager.LoadScene("Login");
    }

    private IEnumerator RegisterUser(string url, User user)
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
                    Debug.Log("Created");
                }
                else
                {
                    //handle the problem
                    Debug.Log("Error! data couldn't get.");
                }
            }
        }
    }


    public IEnumerator Get(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    // handle the result
                    var result = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    Debug.Log(result);

                    user = JsonUtility.FromJson<User>(result).ToString();
                    Debug.Log(user);
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
