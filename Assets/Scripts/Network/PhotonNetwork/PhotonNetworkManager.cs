using UnityEngine;
using System.Collections;
using System;

public class PhotonNetworkManager : MonoBehaviour {

    private const string roomName = "RoomNameUniqueWendel";
    private RoomInfo[] roomsList;

    void Start() {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }


    void OnGUI()
    {
        if (!PhotonNetwork.connected)
        {
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        }
        else if (PhotonNetwork.room == null)
        {
            Debug.Log("drawing buttons");
            // Create Room
            if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server")){
                RoomOptions roomOptions = new RoomOptions(){ isVisible = true, maxPlayers = 4, isOpen = true};
                PhotonNetwork.CreateRoom(roomName + Guid.NewGuid().ToString("N"), roomOptions,TypedLobby.Default);
            }
            // Join Room
            if (roomsList != null)
            {
                for (int i = 0; i< roomsList.Length; i++)
                {
                    if (GUI.Button(new Rect(100, 250 + (110 * i), 250, 100), "Join " + roomsList[i].name))
                        PhotonNetwork.JoinRoom(roomsList[i].name);
                }
            }
        }
    }

    void OnReceivedRoomListUpdate() {
        roomsList = PhotonNetwork.GetRoomList();
    }

    void OnCreatedRoom() {//if started room, then this is called
        //start functions generating map
    }

    void OnPhotonPlayerConnected() {

    }
    public void OnJoinedRoom() {
        //called when created or joined a room

        //call 
        Debug.Log("Connected to Room");
    }

    [RPC]
    bool hasGeneratedMap() {

        return true;
    }
}
