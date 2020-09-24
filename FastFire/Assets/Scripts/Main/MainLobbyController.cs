using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainLobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject lobbyPanel;

    public Text userNameField;

    private string roomName = "Sala 1";
    private int roomSize = 4;

    private List<RoomInfo> roomListings;
    [SerializeField]
    private Transform roomsContainer;
    [SerializeField]
    private GameObject RoomListingPrefab;

    public override void OnJoinedLobby()
    {
        lobbyPanel.SetActive(true);
        roomListings = new List<RoomInfo>();
        userNameField.text = PhotonNetwork.NickName;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(roomList == null)
        {
            roomListings = new List<RoomInfo>();
        }

        int tempIndex;
        foreach(RoomInfo room in roomList)
        {
            if (roomListings != null)
            {
                tempIndex = roomListings.FindIndex(ByName(room.Name));
            }
            else
            {
                tempIndex = -1;
            }
            if(tempIndex != -1)
            {
                roomListings.RemoveAt(tempIndex);
                Destroy(roomsContainer.GetChild(tempIndex).gameObject);
            }
            if(room.PlayerCount > 0)
            {
                roomListings.Add(room);
                ListRoom(room);
            }
        }
    }

    static System.Predicate<RoomInfo> ByName(string name)
    {
        return delegate (RoomInfo room)
        {
            return room.Name == name;
        };
    }

    void ListRoom(RoomInfo room)
    {

        if(room.IsOpen && room.IsVisible)
        {
            GameObject tempListing = Instantiate(RoomListingPrefab, roomsContainer);
            RoomButton tempButton = tempListing.GetComponent<RoomButton>();
            tempButton.SetRoom(room.Name, room.MaxPlayers, room.PlayerCount);
        }
    }

    public void OnRoomNameChanged(string nameIn)
    {
        roomName = nameIn;
    }

    public void OnRoomSizeChanged(string sizeIn)
    {
        roomSize = int.Parse(sizeIn);
    }

    public void CreateRoom()
    {
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte) roomSize};
        PhotonNetwork.CreateRoom(roomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
    }

    public void LogOut()
    {
        SceneManager.LoadScene(0);
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
    }
}
