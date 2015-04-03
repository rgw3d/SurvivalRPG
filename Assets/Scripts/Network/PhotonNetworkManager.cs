using UnityEngine;
using System.Collections;
using System;

public class PhotonNetworkManager : MonoBehaviour {

    private string roomName = "Example Room Name";
    private string playerName = "Player Name";
    private PlayerStats.CharacterClass playerClass = PlayerStats.CharacterClass.op_fighter;
    private float sliderValue = 1;
    private RoomInfo[] roomsList;
    public static bool isHost = false;

    void Start() {
        //PhotonNetwork.ConnectUsingSettings("0.1");
        DelegateHolder.TriggerGenerateAndRenderMap();
    }

    void OnGUI()
    {
        NotConnectedToRoom(); 
        /*
        if (!PhotonNetwork.connected)
        {
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        }
        else if (PhotonNetwork.room == null)
        {
            NotConnectedToRoom();
        }
         */
    }

    private void NotConnectedToRoom() {

        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        GUILayout.BeginHorizontal();
            GUILayout.Space(10);

            GUILayout.BeginVertical();
                GUILayout.FlexibleSpace();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Create Room")) {
                    RoomOptions roomOptions = new RoomOptions() { isVisible = true, maxPlayers = 4, isOpen = true };
                    PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
                }
                roomName = GUILayout.TextField(roomName, 20);
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical();
                GUILayout.FlexibleSpace();
                GUILayout.FlexibleSpace();
                if (roomsList != null) {
                    for (int i = 0; i < roomsList.Length; i++) {
                        if (GUILayout.Button("Join " + roomsList[i].name))
                            PhotonNetwork.JoinRoom(roomsList[i].name);
                    }
                }
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.FlexibleSpace();
            GUILayout.FlexibleSpace();
            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical();
                if (GUILayout.Button("Create Character")) {
                    //do something
                }
                playerName = GUILayout.TextField(playerName, 12);
                
                GUILayout.Box("Class:" + playerClass);
                sliderValue = GUILayout.HorizontalSlider(sliderValue, 0f, 2f);
                switch (Mathf.FloorToInt(sliderValue)) {
                    case 0:
                        playerClass = PlayerStats.CharacterClass.op_fighter;
                        break;
                    case 1:
                        playerClass = PlayerStats.CharacterClass.squishy_mage;
                        break;
                    case 2:
                        playerClass = PlayerStats.CharacterClass.usless_other_than_the_fact_that_they_can_heal_healer;
                        break;
                }
                
                

            
        GUILayout.EndHorizontal();
        GUILayout.EndArea();


        /*
        // Create Room
        if (GUI.Button(new Rect(Screen.width/6, Screen.height/6, Screen.width/6, Screen.height/6), "Create Room")) {
            RoomOptions roomOptions = new RoomOptions() { isVisible = true, maxPlayers = 4, isOpen = true };
            PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
        }
        roomName = GUI.TextField(new Rect(Screen.width / 6, 2 * Screen.height / 6, Screen.width / 6, Screen.height / 12), roomName, 20);

        // Join Room
        if (roomsList != null) {
            for (int i = 1; i <= roomsList.Length; i++) {
                if (GUI.Button(new Rect(Screen.width / 6,  12 * i* Screen.height / 24, Screen.width / 6, Screen.height / 12), "Join " + roomsList[i].name))
                    PhotonNetwork.JoinRoom(roomsList[i].name);
            }
        }


        //Create Character 
        if (GUI.Button(new Rect(4 *Screen.width / 6, Screen.height / 6, Screen.width / 6, Screen.height / 6), "Create Player")) {
            //create player
        }
        playerName = GUI.TextField(new Rect(4 * Screen.width / 6, 2 * Screen.height / 6, Screen.width / 6, Screen.height / 12), playerName, 12);
        GUI.
         * */
        

    }

    void OnReceivedRoomListUpdate() {
        roomsList = PhotonNetwork.GetRoomList();
    }

    void OnCreatedRoom() {//if this user created the room, then this is called
        Debug.Log("Created Room. Connected to room");
        isHost = true;
        DelegateHolder.TriggerGenerateAndRenderMap();
    }

    void OnPhotonPlayerConnected() {
        Debug.Log("Player has connected");
    }
    public void OnJoinedRoom() {
        Debug.Log("Joined Room");
    }

    

}
