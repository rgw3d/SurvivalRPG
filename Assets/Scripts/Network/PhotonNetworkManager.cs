using UnityEngine;
using System.Collections;
using System;

public class PhotonNetworkManager : MonoBehaviour {

    private string roomName = "Example Room Name";
    private string playerName = "Player Name";
    private PlayerStats.CharacterClass playerClass = PlayerStats.CharacterClass.Fighter;
    private float playerClassSlider = 1;
    private float playerSelectSlider = 0;
    private RoomInfo[] roomsList;
    public static bool isHost = false;
    private bool displayPopup = false;

    void Start() {
        PhotonNetwork.ConnectUsingSettings("0.1");
        DelegateHolder.TriggerGenerateAndRenderMap();
        //PlayerPrefs.DeleteAll();
    }

    void OnGUI()
    {
        //NotConnectedToRoom();
        if(displayPopup)
            DisplayPopup();
        
        if (!PhotonNetwork.connected)
        {
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        }
        else if (PhotonNetwork.room == null)
        {
            NotConnectedToRoom();
        }
        
    }

    private void NotConnectedToRoom() {

        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

            GUILayout.BeginVertical(GUILayout.MinWidth(Screen.width / 4));//Create Room
                if (GUILayout.Button("Create Room")) {
                    RoomOptions roomOptions = new RoomOptions() { isVisible = true, maxPlayers = 4, isOpen = true };
                    PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
                }
                roomName = GUILayout.TextField(roomName, 20);
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.MinWidth(Screen.width / 4));//Join Room
                if (roomsList != null) {
                    for (int i = 0; i < roomsList.Length; i++) {
                        if (GUILayout.Button("Join " + roomsList[i].name))
                            PhotonNetwork.JoinRoom(roomsList[i].name);
                    }
                }
            GUILayout.EndVertical();
           
            GUILayout.BeginVertical(GUILayout.MinWidth(Screen.width / 4));//Select Player
                GUILayout.Label("Selected Player");
                string[] allplayerNames = PlayerPrefs.GetString(GameControl.PLAYERNAMESKEY).Split(',');
                playerSelectSlider = GUILayout.HorizontalSlider(playerSelectSlider, 0f, (float)allplayerNames.Length-1);
                int playerIndex = (int)Mathf.Round(playerSelectSlider);
                GUILayout.Box("Name: " +allplayerNames[playerIndex] + "\n" 
                    + "Class: "+ PlayerPrefs.GetInt(GameControl.PLAYERCLASSKEY + allplayerNames[playerIndex]) +"\n"
                    + "Max Health: " + PlayerPrefs.GetInt(GameControl.PLAYERMAXHEALTH + allplayerNames[playerIndex]));
                    
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.MinWidth(Screen.width / 4));//Create Player
                if (GUILayout.Button("Create Character")) {
                    CreateCharacter();
                }
                playerName = GUILayout.TextField(playerName, 12);
                
                GUILayout.Box("Class:" + playerClass);
                playerClassSlider = GUILayout.HorizontalSlider(playerClassSlider, 0f, 2f);
                switch (Mathf.FloorToInt(playerClassSlider)) {
                    case 0:
                        playerClass = PlayerStats.CharacterClass.Fighter;
                        break;
                    case 1:
                        playerClass = PlayerStats.CharacterClass.Mage;
                        break;
                    case 2:
                        playerClass = PlayerStats.CharacterClass.Healer;
                        break;
                }
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
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

    private void CreateCharacter() {
        //what do
        string[] allplayerNames = PlayerPrefs.GetString(GameControl.PLAYERNAMESKEY).Split(',');
        foreach (string s in allplayerNames) {
            if (s.Equals(playerName)) {
                StartCoroutine(TextPopup(.5f));
                return;
            }
        }
        if (playerName.Contains(",") || playerName.Contains(" ")) {

            return;
        }

        if (PlayerPrefs.GetString(GameControl.PLAYERNAMESKEY).Equals("")) {
            PlayerPrefs.SetString(GameControl.PLAYERNAMESKEY, playerName);
        }
        else {
            PlayerPrefs.SetString(GameControl.PLAYERNAMESKEY, PlayerPrefs.GetString(GameControl.PLAYERNAMESKEY) +","+ playerName);
        }
        
        PlayerPrefs.SetInt(GameControl.PLAYERCLASSKEY + playerName, (int)playerClass);

        if (playerClass == PlayerStats.CharacterClass.Fighter) {
            PlayerPrefs.SetInt(GameControl.PLAYERMAXHEALTH + playerName, 1000);
            //do something 
            //set base stats
        }
        else if (playerClass == PlayerStats.CharacterClass.Healer) {
            //do something 
        }
        else if (playerClass == PlayerStats.CharacterClass.Mage) {
            //do something 
        }



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

    IEnumerator TextPopup(float waitTime) {
        displayPopup = true;
        yield return new WaitForSeconds(waitTime);
        displayPopup = false;

    }
    void DisplayPopup(){
        GUI.Box(new Rect(2 * Screen.width / 5, Screen.height / 2, Screen.width / 5, Screen.height / 10), "Bad Name");
    }


    

}
