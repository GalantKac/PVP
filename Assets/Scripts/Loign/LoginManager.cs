using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    // string loginUrl = "https://pvp-server.herokuapp.com/users/login";
    string loginUrl = "http://localhost:3000/users/login";
    //127.0.0.1:3000

    [SerializeField]
    TextMeshProUGUI inputEmail;
    [SerializeField]
    TMP_InputField inputPassword;
    [SerializeField]
    GameObject warrningMessage;
    public void Start()
    {
        warrningMessage.SetActive(false);
    }

    public void Login()
    {
        if (inputPassword.text == "" || inputEmail.text == "")
        {
            warrningMessage.SetActive(true);
        }
        else
        {
            warrningMessage.SetActive(false);
            User loginUser = new User(inputEmail.text.TrimEnd('\u200b'), inputPassword.text.TrimEnd('\u200b'));
            StartCoroutine(PostLogin(loginUrl, loginUser));
        }
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
        //Debug.Log("JSONdata: " + jsonData);

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
                    // handle the result
                    string result = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    // Debug.Log(result);
                    User resultUser = JsonUtility.FromJson<User>(result);
                    resultUser.x = "5,312";
                    resultUser.y = "2,123";
                    LoggedInPlayer.instance.networkManager.LogginPlayer(resultUser);
                    SceneManager.LoadScene("Menu");
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
