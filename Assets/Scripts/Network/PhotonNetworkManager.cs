﻿using UnityEngine;
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

    private string _chatUsername = "UserName";
    private ChatReciever _chatClient = null;

    void Start() {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    void OnGUI() {
        if(_displayBadNamePopup)
            DisplayBadNamePopup();
        
        if (!PhotonNetwork.connected)
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        else if (PhotonNetwork.room == null)
            NotConnectedToRoom();
        
    }

    private void NotConnectedToRoom() {

        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            GUILayout.BeginVertical();
                GUILayout.BeginHorizontal(GUILayout.MinHeight(2*Screen.height/3));
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
                        int playerIndex = Mathf.RoundToInt(_playerSelectSlider);
                        GUILayout.Box("Name: " +allplayerNames[playerIndex] + "\n" 
                            + "Class: "+ PlayerStats.IntToCharacterClass(PlayerPrefs.GetInt(GameControl.PLAYERCLASSKEY + allplayerNames[playerIndex])) +"\n"
                            + "Max Health: " + PlayerPrefs.GetInt(GameControl.PLAYERMAXHEALTHKEY + allplayerNames[playerIndex]) + "\n" 
                            + "Max Mana: " + PlayerPrefs.GetInt(GameControl.PLAYERMAXMANAKEY + allplayerNames[playerIndex]) + "\n"
                            + "Defense: " + PlayerPrefs.GetInt(GameControl.PLAYERDEFENSEKEY + allplayerNames[playerIndex]) + "\n"
                            + "Attack: " + PlayerPrefs.GetInt(GameControl.PLAYERATTACKKEY + allplayerNames[playerIndex]));
                    
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(GUILayout.MinWidth(Screen.width / 4));//Create Player
                        if (GUILayout.Button("Create Character")) {
                            CreateCharacter();
                        }
                        _playerName = GUILayout.TextField(_playerName, 12);
                
                        GUILayout.Box("Class: " + _playerClass);
                        _playerClassSlider = GUILayout.HorizontalSlider(_playerClassSlider, 0f, 2f);
                        _playerClass = PlayerStats.IntToCharacterClass(Mathf.RoundToInt(_playerClassSlider));
                    GUILayout.EndVertical();

                    GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                    LobbyChatClient();
                GUILayout.EndHorizontal();
            GUILayout.EndVertical();
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
        

        if (PlayerPrefs.GetString(GameControl.PLAYERNAMESKEY).Equals("")) //first name that is being added
            PlayerPrefs.SetString(GameControl.PLAYERNAMESKEY, _playerName);
        else //every additional name
            PlayerPrefs.SetString(GameControl.PLAYERNAMESKEY, PlayerPrefs.GetString(GameControl.PLAYERNAMESKEY) +","+ _playerName);
        
        PlayerPrefs.SetInt(GameControl.PLAYERCLASSKEY + _playerName, (int)_playerClass);

        switch (_playerClass) {
            case PlayerStats.CharacterClass.Fighter:
                PlayerPrefs.SetInt(GameControl.PLAYERMAXHEALTHKEY + _playerName, 100);
                PlayerPrefs.SetInt(GameControl.PLAYERMAXMANAKEY + _playerName, 20);
                PlayerPrefs.SetInt(GameControl.PLAYERDEFENSEKEY + _playerName, 100);
                PlayerPrefs.SetInt(GameControl.PLAYERATTACKKEY + _playerName, 100);
                break;
            case PlayerStats.CharacterClass.Mage:
                PlayerPrefs.SetInt(GameControl.PLAYERMAXHEALTHKEY + _playerName, 1);
                PlayerPrefs.SetInt(GameControl.PLAYERMAXMANAKEY + _playerName, 1000);
                PlayerPrefs.SetInt(GameControl.PLAYERDEFENSEKEY + _playerName, 1);
                PlayerPrefs.SetInt(GameControl.PLAYERATTACKKEY + _playerName, 100);
                break;
            case PlayerStats.CharacterClass.Healer:
                PlayerPrefs.SetInt(GameControl.PLAYERMAXHEALTHKEY + _playerName, 50);
                PlayerPrefs.SetInt(GameControl.PLAYERMAXMANAKEY + _playerName, 500);
                PlayerPrefs.SetInt(GameControl.PLAYERDEFENSEKEY + _playerName, 20);
                PlayerPrefs.SetInt(GameControl.PLAYERATTACKKEY + _playerName, 10);
                break;
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

    void LobbyChatClient() {
        if (_chatClient != null) {
            GUILayout.Label("Chat Username: ");
            _chatUsername = GUILayout.TextField(_chatUsername);
            if (GUILayout.Button("Join ChatRoom")) {
                if (!_chatUsername.Equals("UserName") && !_chatUsername.Equals("")) {
                    _chatClient = new ChatReciever(_chatUsername);
                }
                else {
                    StartCoroutine(TextPopup(.5f));
                }
            }
            
        }
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
