using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DelayStartLobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject delayStartButton; // Button used for creating and joining a game.
    [SerializeField]
    private GameObject delayCancelButton; // Button used for stop searing for a game to join
    [SerializeField]
    private int RoomSize; // Manual set the number of players in the room at one time
    public override void OnConnectedToMaster() // Callback function for when ther first connection is established
    {
        PhotonNetwork.AutomaticallySyncScene = true; // Makes it so whatever scene the master client
        delayStartButton.SetActive(true);
    }

    public void DelayStart() // Paired to the delay start button
    {
        delayStartButton.SetActive(false);
        delayCancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom(); // First tries to join an existing room
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom();
    }

    void CreateRoom()
    {
        int randomRoomNumber = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)RoomSize };
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CreateRoom();
    }

    public void DelayCancel()
    {
        delayCancelButton.SetActive(false);
        delayStartButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
