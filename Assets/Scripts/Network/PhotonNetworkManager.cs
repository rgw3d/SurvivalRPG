using UnityEngine;
using System.Collections;
using System;

public class PhotonNetworkManager : MonoBehaviour {

    private string _roomName = "Example Room Name";
    private RoomInfo[] _roomsList;
    public static bool IsHost = false;

    private static bool _displayBadNamePopup = false;
    private float _playerClassSlider = 1;
    private float _playerSelectSlider = 0;
    private string _playerName = "Player Name";
    private PlayerStats.CharacterClass _playerClass = PlayerStats.CharacterClass.Fighter;

	public  static string selectedPlayerName = "";

    private string _chatUsername = "UserName";
    private ChatBuffer _chatClient;
    private Vector2 scrollPosition;

    void Start() {
        PhotonNetwork.ConnectUsingSettings("0.1");
        _chatClient = FindObjectOfType<ChatBuffer>();
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
                GUILayout.BeginHorizontal(GUILayout.MinHeight(Screen.height/3));
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
                        + "Attack: " + PlayerPrefs.GetInt(GameControl.PLAYERATTACKKEY + allplayerNames[playerIndex]) + "\n"
		            	+ "Defense: " + PlayerPrefs.GetInt(GameControl.PLAYERDEFENSEKEY + allplayerNames[playerIndex]) + "\n"
		             	+ "Movement Speed: " + PlayerPrefs.GetFloat(GameControl.PLAYERMOVEMENTKEY + allplayerNames[playerIndex]));
					selectedPlayerName = allplayerNames[playerIndex];
					if (GUILayout.Button("Delete Character")) {
						DeleteCharacter(allplayerNames[playerIndex]);
                        _playerSelectSlider = 0;
					}
                GUILayout.EndVertical();

                    GUILayout.BeginVertical(GUILayout.MinWidth(Screen.width / 4));//Create Player
                        if (GUILayout.Button("Create Character")) {
                            CreateCharacter();
                        }
                        _playerName = GUILayout.TextField(_playerName, 12);
                
                        GUILayout.Box("Class: " + _playerClass);
                        _playerClassSlider = GUILayout.HorizontalSlider(_playerClassSlider, 0f, 3f);
                        _playerClass = PlayerStats.IntToCharacterClass(Mathf.RoundToInt(_playerClassSlider));
                    GUILayout.EndVertical();

                    GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                    //LobbyChatClient();
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
				PlayerPrefs.SetInt(GameControl.PLAYERATTACKKEY + _playerName, 100);
                PlayerPrefs.SetInt(GameControl.PLAYERDEFENSEKEY + _playerName, 100);
				PlayerPrefs.SetFloat(GameControl.PLAYERMOVEMENTKEY + _playerName, 80f);
                break;
            case PlayerStats.CharacterClass.Mage:
                PlayerPrefs.SetInt(GameControl.PLAYERMAXHEALTHKEY + _playerName, 1);
                PlayerPrefs.SetInt(GameControl.PLAYERMAXMANAKEY + _playerName, 1000);
                PlayerPrefs.SetInt(GameControl.PLAYERATTACKKEY + _playerName, 100);
				PlayerPrefs.SetInt(GameControl.PLAYERDEFENSEKEY + _playerName, 1);
				PlayerPrefs.SetFloat(GameControl.PLAYERMOVEMENTKEY + _playerName, 800f);
                break;
            case PlayerStats.CharacterClass.Healer:
                PlayerPrefs.SetInt(GameControl.PLAYERMAXHEALTHKEY + _playerName, 50);
                PlayerPrefs.SetInt(GameControl.PLAYERMAXMANAKEY + _playerName, 500);
                PlayerPrefs.SetInt(GameControl.PLAYERATTACKKEY + _playerName, 10);
				PlayerPrefs.SetInt(GameControl.PLAYERDEFENSEKEY + _playerName, 20);
				PlayerPrefs.SetFloat(GameControl.PLAYERMOVEMENTKEY + _playerName, 80f);
                break;
			case PlayerStats.CharacterClass.Shrek:
				PlayerPrefs.SetInt(GameControl.PLAYERMAXHEALTHKEY + _playerName, 9000);
				PlayerPrefs.SetInt(GameControl.PLAYERMAXMANAKEY + _playerName, 0);
				PlayerPrefs.SetInt(GameControl.PLAYERATTACKKEY + _playerName, 411);
				PlayerPrefs.SetInt(GameControl.PLAYERDEFENSEKEY + _playerName, 350);
				PlayerPrefs.SetFloat(GameControl.PLAYERMOVEMENTKEY + _playerName, 20f);
				break;
        }
    }

	void DeleteCharacter(string deletedPlayerName){
		string[] allPlayerNames = PlayerPrefs.GetString(GameControl.PLAYERNAMESKEY).Split(',');
		string newAllPlayerNames = "";
		foreach(string name in allPlayerNames){
			if(!name.Equals(deletedPlayerName)){
				if(newAllPlayerNames.Equals(""))
					newAllPlayerNames += name;
				else
					newAllPlayerNames += "," + name;
			}
		}
		PlayerPrefs.SetString(GameControl.PLAYERNAMESKEY,newAllPlayerNames);
		PlayerPrefs.DeleteKey(GameControl.PLAYERCLASSKEY + deletedPlayerName);
		PlayerPrefs.DeleteKey(GameControl.PLAYERMAXHEALTHKEY + deletedPlayerName);
		PlayerPrefs.DeleteKey(GameControl.PLAYERMAXMANAKEY + deletedPlayerName);
		PlayerPrefs.DeleteKey(GameControl.PLAYERDEFENSEKEY + deletedPlayerName);
		PlayerPrefs.DeleteKey(GameControl.PLAYERATTACKKEY + deletedPlayerName);
	}

    public static IEnumerator TextPopup(float waitTime) {
        _displayBadNamePopup = true;
        yield return new WaitForSeconds(waitTime);
        _displayBadNamePopup = false;

    }
    void DisplayBadNamePopup() {
        GUI.Box(new Rect(2 * Screen.width / 5, Screen.height / 2, Screen.width / 5, Screen.height / 10), "Bad Name");
    }

    void LobbyChatClient() {
        if (_chatClient.Host.Equals("")) {
            GUILayout.Label("Chat Username: ");
            _chatUsername = GUILayout.TextField(_chatUsername);
            if (GUILayout.Button("Join ChatRoom")) {
                if (!_chatUsername.Equals("UserName") && !_chatUsername.Equals("")) {
                    _chatClient.Host = _chatUsername;
                }
                else {
                    StartCoroutine(TextPopup(.5f));
                }
            }
        }
        else {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.MinWidth(Screen.width/3));
            GUILayout.Label(_chatClient.TextOutput());
            GUILayout.EndScrollView();
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
