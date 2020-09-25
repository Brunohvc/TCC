using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameController : MonoBehaviourPunCallbacks
{

    public Text kills;
    public Text name;
    public Text duration;

    // Start is called before the first frame update
    void Start()
    {
        EndGame endGame = new EndGame();

        Debug.Log("Kills: " + endGame.getKills());
        Debug.Log("Nome: " + endGame.getNickName());
        Debug.Log("Tempo: " + endGame.getTime());

        name.text = "Player: " + endGame.getNickName();
        kills.text = "Kills: " + endGame.getKills();
        duration.text = "Duration: " + endGame.getTime();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.LeaveRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LeaveRoom()
    {
        SceneManager.LoadScene(1);
        PhotonNetwork.JoinLobby();
    }
}
