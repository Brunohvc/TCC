using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Net;
using System.IO;
using Assets.Scripts.Login;

public class LobbyControllerLogin : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject connectText;
    [SerializeField]
    private GameObject loginData;
    [SerializeField]
    public InputField playerLoginInput;
    [SerializeField]
    private InputField playerPasswordInput;

    [SerializeField]
    private int LobbySceneIndex;


    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        loginData.SetActive(true);
        connectText.SetActive(false);

        if (PlayerPrefs.HasKey("Login"))
        {
            if(PlayerPrefs.GetString("Login") != "")
            {
                playerLoginInput.text = PlayerPrefs.GetString("Login");
            }

            if (PlayerPrefs.HasKey("Password"))
            {
                if (PlayerPrefs.GetString("Password") != "")
                {
                    playerPasswordInput.text = PlayerPrefs.GetString("Password");
                }
            }
        }
    }

    public void PlayerLoginUpdate(string loginInput)
    {
        playerLoginInput.text = loginInput;
    }

    public void PlayerPasswordUpdate(string passwordInput)
    {
        playerPasswordInput.text = passwordInput;
    }

    public void LoginOnClick()
    {
        if (playerLoginInput.text != "" && playerPasswordInput.text != "")
        {
            // make login
            LogonServer();
            PlayerPrefs.SetString("Login", playerLoginInput.text);
            PlayerPrefs.SetString("Password", playerPasswordInput.text);
            PhotonNetwork.NickName = playerLoginInput.text;
            SceneManager.LoadScene(LobbySceneIndex);
            PhotonNetwork.JoinLobby();
        }
    }

    public void LogonServer()
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3333/api/v1/users/login");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            Login login = new Login();
            login.email = "zanghelinigab@gmail.com"; // playerLoginInput.text;
            login.password = "123456"; // playerPasswordInput.text

            string json = JsonUtility.ToJson(login);

            streamWriter.Write(json);
            streamWriter.Flush();
            streamWriter.Close();
        }

        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            Login loginResult = JsonUtility.FromJson<Login>(result);
            if (!loginResult.message.Contains("Success"))
            {
                throw new System.Exception("Login failed");
            }
        }
    }

}
