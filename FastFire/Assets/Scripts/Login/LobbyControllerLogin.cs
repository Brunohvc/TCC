using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

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

        Debug.Log("Login Scene On!");

        loginData.SetActive(true);
        connectText.SetActive(false);

        if (PlayerPrefs.HasKey("Login"))
        {
            if(PlayerPrefs.GetString("Login") != "")
            {
                Debug.Log("Login: " + PlayerPrefs.GetString("Login"));
                playerLoginInput.text = PlayerPrefs.GetString("Login");
            }

            if (PlayerPrefs.HasKey("Password"))
            {
                if (PlayerPrefs.GetString("Password") != "")
                {
                    Debug.Log("Password: " + PlayerPrefs.GetString("Password"));
                    playerLoginInput.text = PlayerPrefs.GetString("Password");
                }
            }
        }
    }

    public void PlayerLoginUpdate(string loginInput)
    {
        Debug.Log("Login: " + loginInput);
        playerLoginInput.text = loginInput;
    }

    public void PlayerPasswordUpdate(string passwordInput)
    {
        Debug.Log("Password: " + passwordInput);
        playerPasswordInput.text = passwordInput;
    }

    public void LoginOnClick()
    {
        Debug.Log("Login Button Click!");

        if (playerLoginInput.text != "")
        {
            PlayerPrefs.SetString("Login", playerLoginInput.text);
            PlayerPrefs.SetString("Password", playerPasswordInput.text);
            Debug.Log("Login: " + PlayerPrefs.GetString("Login"));
            Debug.Log("Password: " + PlayerPrefs.GetString("Password"));
            PhotonNetwork.NickName = playerPasswordInput.text;
            // PhotonNetwork.JoinLobby();
            SceneManager.LoadScene(LobbySceneIndex);
        }
    }
}
