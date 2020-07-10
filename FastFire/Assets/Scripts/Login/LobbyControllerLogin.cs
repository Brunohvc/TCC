using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Net;
using System.IO;
using Assets.Scripts.Login;
using System;

public class LobbyControllerLogin : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject connectText;
    [SerializeField]
    private GameObject loginData;
    [SerializeField]
    private GameObject loginPanel;
    [SerializeField]
    private GameObject registerPanel;
    [SerializeField]
    public GameObject erroLogin;
    [SerializeField]
    public GameObject erroRegister;
    [SerializeField]
    public InputField playerLoginInput;
    [SerializeField]
    private InputField playerPasswordInput;
    [SerializeField]
    public InputField registerLoginInput;
    [SerializeField]
    public InputField registerEmailInput;
    [SerializeField]
    private InputField registerPasswordInput;
    [SerializeField]
    private InputField registerPasswordConfirmationInput;
    [SerializeField]
    private int LobbySceneIndex;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        loginData.SetActive(true);
        connectText.SetActive(false);
        registerPanel.SetActive(false);

        if (PlayerPrefs.HasKey("Login"))
        {
            if (PlayerPrefs.GetString("Login") != "")
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

    public void SwitchToRegisterPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
        erroRegister.SetActive(false);
    }

    public void SwitchToLoginPanel()
    {
        registerPanel.SetActive(false);
        loginPanel.SetActive(true);
        erroLogin.SetActive(false);
        connectText.SetActive(false);
    }

    public void LoginOnClick()
    {
        if (playerLoginInput.text != "" && playerPasswordInput.text != "")
        {
            erroLogin.SetActive(false);
            // make login
            SendPostRequestServer("login", playerLoginInput.text, playerPasswordInput.text, "");
            PlayerPrefs.SetString("Login", playerLoginInput.text);
            PlayerPrefs.SetString("Password", playerPasswordInput.text);
            SceneManager.LoadScene(LobbySceneIndex);
            PhotonNetwork.JoinLobby();
        }
    }

    public void RegisterOnClick()
    {
        // muda o panel
        if (registerEmailInput.text != "" && registerLoginInput.text != "" && registerPasswordInput.text != "" && registerPasswordConfirmationInput.text != "")
        {
            if (!registerPasswordInput.text.Equals(registerPasswordConfirmationInput.text))
            {
                erroRegister.GetComponent<Text>().text = "Password is different than password confirmation";
                erroRegister.SetActive(true);
            }
            else
            {
                VerifyLogin();
                SendPostRequestServer("store", registerLoginInput.text, registerPasswordInput.text, registerEmailInput.text);
                SwitchToLoginPanel();
            }
        }
        else
        {
            erroRegister.GetComponent<Text>().text = "Fill in all fields";
            erroRegister.SetActive(true);
        }
    }

    private void VerifyLogin()
    {
        SendGetRequestServer("isLoginUsed/" + registerLoginInput.text);
    }

    private void SendGetRequestServer(string action)
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://fast-fire.herokuapp.com/api/v1/users/" + action);
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "GET";
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            Login response = JsonUtility.FromJson<Login>(result);
            if (response.isLoginUsed)
            {
                erroRegister.GetComponent<Text>().text = "Login already in use";
                erroRegister.SetActive(true);
                throw new System.Exception("Server fail: Login already in use");
            }
        }
    }

    public void SendPostRequestServer(string action, string loginInput, string passwordInput, string emailInput)
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://fast-fire.herokuapp.com/api/v1/users/" + action);
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            Login login = new Login();
            login.email = emailInput;
            login.password = passwordInput;
            if (action.Equals("store")){
                login.login = loginInput;
            } else if (action.Equals("login"))
            {
                login.email = loginInput;
            }

            string json = JsonUtility.ToJson(login);

            streamWriter.Write(json);
            streamWriter.Flush();
            streamWriter.Close();
        }

        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            Login response = JsonUtility.FromJson<Login>(result);
            if (!response.message.Contains("Success"))
            {
                erroLogin.GetComponent<Text>().text = response.message;
                erroRegister.GetComponent<Text>().text = response.message;
                erroLogin.SetActive(true);
                erroRegister.SetActive(true);
                throw new System.Exception("Server fail: " + response.message);
            } else
            {
                if (response.user.login != null && response.user.login != null)
                {
                    PhotonNetwork.NickName = response.user.login;
                }
            }
        }
    }

}
