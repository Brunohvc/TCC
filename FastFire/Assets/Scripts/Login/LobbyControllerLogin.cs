using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System;

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
        openLoginScreen();
    }

    void openLoginScreen()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        loginData.SetActive(true);
        connectText.SetActive(false);

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

    public void LoginOnClick()
    {
        if (playerLoginInput.text != "")
        {
            PlayerPrefs.SetString("Login", playerLoginInput.text);
            PlayerPrefs.SetString("Password", playerPasswordInput.text);
            PhotonNetwork.NickName = playerLoginInput.text;
            SceneManager.LoadScene(LobbySceneIndex);
            PhotonNetwork.JoinLobby();
        }
    }
}
