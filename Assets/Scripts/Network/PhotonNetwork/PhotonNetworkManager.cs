using UnityEngine;
using System.Collections;
using System;

public class PhotonNetworkManager : MonoBehaviour {

    private string roomName = "Example Room Name";
    private RoomInfo[] roomsList;



    public StevensMapGeneration mapGeneration;

    public static bool isHost = false;

    void Start() {
        //PhotonNetwork.ConnectToBestCloudServer("0.1");
        mapGeneration = GameObject.FindGameObjectWithTag("Map Controller").GetComponent<StevensMapGeneration>();
        PhotonNetwork.ConnectUsingSettings("0.1");
        //PhotonNetwork.MAX_VIEW_IDS = 2000;
    }


    void OnGUI()
    {
        if (!PhotonNetwork.connected)
        {
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        }
        else if (PhotonNetwork.room == null)
        {
            // Create Room
            if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server")){
                RoomOptions roomOptions = new RoomOptions(){ isVisible = true, maxPlayers = 4, isOpen = true};
                PhotonNetwork.CreateRoom(roomName +"  "+  Guid.NewGuid().ToString("N"), roomOptions,TypedLobby.Default);
            }
            roomName = GUI.TextField(new Rect(100, 250, 250, 25), roomName, 20);
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
        Debug.Log("Connected to room");
        isHost = true;
        mapGeneration.GenerateAndDisplayMap();
        //start functions generating map
    }

    void OnPhotonPlayerConnected() {
        Debug.Log("Player has connected");
    }
    public void OnJoinedRoom() {
        Debug.Log("Connected to Room");
    }

    

}
