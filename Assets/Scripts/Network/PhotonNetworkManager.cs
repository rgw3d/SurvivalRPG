using UnityEngine;
using System.Collections;
using System;

public class PhotonNetworkManager : MonoBehaviour {

    private string _roomName = "Example Room Name";
    private RoomInfo[] _roomsList;
    public static bool IsHost = false;

    private bool _displayBadNamePopup = false;
    private float _playerClassSlider = 1;
    private float _playerSelectSlider = 0;
    private string _playerName = "Player Name";
    private PlayerStats.CharacterClass _playerClass = PlayerStats.CharacterClass.Fighter;

    void Start() {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    void OnGUI() {
        if(_displayBadNamePopup)
            DisplayBadNamePopup();
        
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
                        PhotonNetwork.CreateRoom(_roomName, roomOptions, TypedLobby.Default);
                    }
                    _roomName = GUILayout.TextField(_roomName, 20);
                GUILayout.EndVertical();

                GUILayout.BeginVertical(GUILayout.MinWidth(Screen.width / 4));//Join Room
                    if (_roomsList != null) {
                        for (int i = 0; i < _roomsList.Length; i++) {
                            if (GUILayout.Button("Join " + _roomsList[i].name))
                                PhotonNetwork.JoinRoom(_roomsList[i].name);
                        }
                    }
                GUILayout.EndVertical();
           
                GUILayout.BeginVertical(GUILayout.MinWidth(Screen.width / 4));//Select Player
                    GUILayout.Label("Selected Player");
                    string[] allplayerNames = PlayerPrefs.GetString(GameControl.PLAYERNAMESKEY).Split(',');
                    _playerSelectSlider = GUILayout.HorizontalSlider(_playerSelectSlider, 0f, (float)allplayerNames.Length-1);
                    int playerIndex = (int)Mathf.Round(_playerSelectSlider);
                    GUILayout.Box("Name: " +allplayerNames[playerIndex] + "\n" 
                        + "Class: "+ PlayerPrefs.GetInt(GameControl.PLAYERCLASSKEY + allplayerNames[playerIndex]) +"\n"
                        + "Max Health: " + PlayerPrefs.GetInt(GameControl.PLAYERMAXHEALTH + allplayerNames[playerIndex]));
                    
                GUILayout.EndVertical();

                GUILayout.BeginVertical(GUILayout.MinWidth(Screen.width / 4));//Create Player
                    if (GUILayout.Button("Create Character")) {
                        CreateCharacter();
                    }
                    _playerName = GUILayout.TextField(_playerName, 12);
                
                    GUILayout.Box("Class: " + _playerClass);
                    _playerClassSlider = GUILayout.HorizontalSlider(_playerClassSlider, 0f, 2f);
                    switch ((int)Mathf.Round(_playerClassSlider)) {
                        case 0:
                            _playerClass = PlayerStats.CharacterClass.Fighter;
                            break;
                        case 1:
                            _playerClass = PlayerStats.CharacterClass.Mage;
                            break;
                        case 2:
                            _playerClass = PlayerStats.CharacterClass.Healer;
                            break;
                    }
                GUILayout.EndVertical();

                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        GUILayout.EndArea();
        
    }

    private void CreateCharacter() {
        if (_playerName.Contains(",") || _playerName.Contains(" ")) {//If the name contains bad characters then it is rejected
            StartCoroutine(TextPopup(.5f));
            return;
        }
        string[] allplayerNames = PlayerPrefs.GetString(GameControl.PLAYERNAMESKEY).Split(',');
        foreach (string s in allplayerNames) {
            if (s.Equals(_playerName)) {//if any of the same name exist then the name is rejected
                StartCoroutine(TextPopup(.5f));
                return;
            }
        }
        

        if (PlayerPrefs.GetString(GameControl.PLAYERNAMESKEY).Equals("")) {
            PlayerPrefs.SetString(GameControl.PLAYERNAMESKEY, _playerName);
        }
        else {
            PlayerPrefs.SetString(GameControl.PLAYERNAMESKEY, PlayerPrefs.GetString(GameControl.PLAYERNAMESKEY) +","+ _playerName);
        }
        
        PlayerPrefs.SetInt(GameControl.PLAYERCLASSKEY + _playerName, (int)_playerClass);

        if (_playerClass == PlayerStats.CharacterClass.Fighter) {
            PlayerPrefs.SetInt(GameControl.PLAYERMAXHEALTH + _playerName, 1000);
            //do something 
            //set base stats
        }
        else if (_playerClass == PlayerStats.CharacterClass.Healer) {
            //do something 
        }
        else if (_playerClass == PlayerStats.CharacterClass.Mage) {
            //do something 
        }
    }

    IEnumerator TextPopup(float waitTime) {
        _displayBadNamePopup = true;
        yield return new WaitForSeconds(waitTime);
        _displayBadNamePopup = false;

    }
    void DisplayBadNamePopup() {
        GUI.Box(new Rect(2 * Screen.width / 5, Screen.height / 2, Screen.width / 5, Screen.height / 10), "Bad Name");
    }

    void OnReceivedRoomListUpdate() {
        _roomsList = PhotonNetwork.GetRoomList();
    }

    void OnCreatedRoom() {//if this user created the room, then this is called
        Debug.Log("Created Room. Connected to room");
        IsHost = true;
        DelegateHolder.TriggerGenerateAndRenderMap();
    }

    void OnPhotonPlayerConnected() {
        Debug.Log("Player has connected");
    }
    public void OnJoinedRoom() {
        Debug.Log("Joined Room");
    }

    


    

}
