using UnityEngine;
using System.Collections;
using System;

public class PhotonNetworkManager : MonoBehaviour {

    private string _roomName = "Blank Room Name";
    private RoomInfo[] _roomsList;

    private static bool _displayBadNamePopup = false;
    private float _playerClassSlider = 1;
    private float _playerSelectSlider = 0;
    private string _playerName = "Player Name";
    private PlayerStats.CharacterClass _playerClass = PlayerStats.CharacterClass.Chipmunk;

    public static bool IsHost = false;
	public static string selectedPlayerName = "";
    private Vector2 scrollPosition;

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
                GUILayout.BeginHorizontal();
                    var style = new GUIStyle(GUI.skin.box);
                    style.normal.textColor = Color.red;
                    style.fontSize = (int)(Screen.width / 50);
                    GUILayout.Label("Escape from the Lab", style);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(GUILayout.MinHeight(Screen.height/3));
                GUILayout.FlexibleSpace();

                    GUILayout.BeginVertical(GUILayout.MinWidth(Screen.width / 4));//Create Room
                        if (GUILayout.Button("Create Room")) {
                            
                            RoomOptions roomOptions = new RoomOptions() { isVisible = true, maxPlayers = 4, isOpen = true };
                            PhotonNetwork.JoinOrCreateRoom(_roomName, roomOptions, TypedLobby.Default);
                            IsHost = true;
                            Application.LoadLevel(GameControl.PLAY_SCREEN);//load play screen
                        }
                        _roomName = GUILayout.TextField(_roomName, 20);
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(GUILayout.MinWidth(Screen.width / 4));//Join Room
                        if (_roomsList != null) {
                            for (int i = 0; i < _roomsList.Length; i++) {
                                if (GUILayout.Button("Join " + _roomsList[i].name)) {
                                    Application.LoadLevel(GameControl.PLAY_SCREEN);//load play screen
                                    StartCoroutine(WaitForSceneToLoad(1f));
                                    PhotonNetwork.JoinRoom(_roomsList[i].name);//Connect to Room
                                }
                            }
                        }
                    GUILayout.EndVertical();
           
                GUILayout.BeginVertical(GUILayout.MinWidth(Screen.width / 4));//Select Player
                    GUILayout.Label("Selected Player");
                    string[] allplayerNames = PlayerPrefs.GetString(GameControl.PLAYER_NAMES_KEY).Split(',');
                    _playerSelectSlider = GUILayout.HorizontalSlider(_playerSelectSlider, 0f, (float)allplayerNames.Length-1);
                    int playerIndex = Mathf.RoundToInt(_playerSelectSlider);
                    GUILayout.Box("Name: " +allplayerNames[playerIndex] + "\n"
                        + "Level " + (PlayerPrefs.GetInt(GameControl.PLAYER_LEVEL_KEY + allplayerNames[playerIndex]))
                        + " " + (PlayerStats.CharacterClass)PlayerPrefs.GetInt(GameControl.PLAYER_CLASS_KEY + allplayerNames[playerIndex]) + "\n"
                        + "Max Health: " + PlayerPrefs.GetInt(GameControl.PLAYER_MAX_HEALTH_KEY + allplayerNames[playerIndex]) + "\n" 
                        + "Max Mana: " + PlayerPrefs.GetInt(GameControl.PLAYER_MAX_MANA_KEY + allplayerNames[playerIndex]) + "\n"
                        + "Attack: " + PlayerPrefs.GetInt(GameControl.PLAYER_ATTACK_KEY + allplayerNames[playerIndex]) + "\n"
		            	+ "Defense: " + PlayerPrefs.GetInt(GameControl.PLAYER_DEFENSE_KEY + allplayerNames[playerIndex]) + "\n"
		             	+ "Movement Speed: " + PlayerPrefs.GetFloat(GameControl.PLAYER_MOVEMENT_KEY + allplayerNames[playerIndex]));
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
                        _playerClassSlider = GUILayout.HorizontalSlider(_playerClassSlider, 1f, 4f);
                        _playerClass = (PlayerStats.CharacterClass)Mathf.RoundToInt(_playerClassSlider);
                    GUILayout.EndVertical();

                    GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        GUILayout.EndArea();
        
    }

    private void CreateCharacter() {
        if (_playerName.Contains(",") || _playerName.Contains(" ")) {//If the name contains bad characters then it is rejected
            StartCoroutine(TextPopup(.5f));
            return;
        }
        string[] allplayerNames = PlayerPrefs.GetString(GameControl.PLAYER_NAMES_KEY).Split(',');
        foreach (string s in allplayerNames) {
            if (s.Equals(_playerName)) {//if any of the same name exist then the name is rejected
                StartCoroutine(TextPopup(.5f));
                return;
            }
        }
        

        if (PlayerPrefs.GetString(GameControl.PLAYER_NAMES_KEY).Equals("")) //first name that is being added
            PlayerPrefs.SetString(GameControl.PLAYER_NAMES_KEY, _playerName);
        else //every additional name
            PlayerPrefs.SetString(GameControl.PLAYER_NAMES_KEY, PlayerPrefs.GetString(GameControl.PLAYER_NAMES_KEY) +","+ _playerName);
        
        PlayerPrefs.SetInt(GameControl.PLAYER_CLASS_KEY + _playerName, (int)_playerClass);

        switch (_playerClass) {
            case PlayerStats.CharacterClass.Chipmunk://fighter
                PlayerPrefs.SetInt(GameControl.PLAYER_LEVEL_KEY + _playerName, 1);
                PlayerPrefs.SetInt(GameControl.PLAYER_MAX_HEALTH_KEY + _playerName, 100);
                PlayerPrefs.SetInt(GameControl.PLAYER_MAX_MANA_KEY + _playerName, 20);
                PlayerPrefs.SetInt(GameControl.PLAYER_ATTACK_KEY + _playerName, 30);
                PlayerPrefs.SetInt(GameControl.PLAYER_RANGED_ATTACK_KEY + _playerName, 10);
                PlayerPrefs.SetInt(GameControl.PLAYER_DEFENSE_KEY + _playerName, 5);
				PlayerPrefs.SetFloat(GameControl.PLAYER_MOVEMENT_KEY + _playerName, 80f);
                PlayerPrefs.SetInt(GameControl.PLAYER_ATTACK_COOLDOWN_KEY + _playerName, 15);
                PlayerPrefs.SetInt(GameControl.PLAYER_POWER_ATTACK_MAX_VALUE_KEY + _playerName, 120);
                PlayerPrefs.SetInt(GameControl.PLAYER_ABILITY_1_COOLDOWN_KEY + _playerName, 40);
                PlayerPrefs.SetInt(GameControl.PLAYER_ABILITY_2_COOLDOWN_KEY + _playerName, 120);
                break;
            case PlayerStats.CharacterClass.Toad://mage
                PlayerPrefs.SetInt(GameControl.PLAYER_LEVEL_KEY + _playerName, 1);
                PlayerPrefs.SetInt(GameControl.PLAYER_MAX_HEALTH_KEY + _playerName, 50);
                PlayerPrefs.SetInt(GameControl.PLAYER_MAX_MANA_KEY + _playerName, 200);
                PlayerPrefs.SetInt(GameControl.PLAYER_RANGED_ATTACK_KEY + _playerName, 30);
                PlayerPrefs.SetInt(GameControl.PLAYER_ATTACK_KEY + _playerName, 10);
				PlayerPrefs.SetInt(GameControl.PLAYER_DEFENSE_KEY + _playerName, 2);
				PlayerPrefs.SetFloat(GameControl.PLAYER_MOVEMENT_KEY + _playerName, 100f);
                PlayerPrefs.SetInt(GameControl.PLAYER_ATTACK_COOLDOWN_KEY + _playerName, 10);
                PlayerPrefs.SetInt(GameControl.PLAYER_POWER_ATTACK_MAX_VALUE_KEY + _playerName, 120);
                PlayerPrefs.SetInt(GameControl.PLAYER_ABILITY_1_COOLDOWN_KEY + _playerName, 40);
                PlayerPrefs.SetInt(GameControl.PLAYER_ABILITY_2_COOLDOWN_KEY + _playerName, 120);
                break;
            case PlayerStats.CharacterClass.Dove://healer
                PlayerPrefs.SetInt(GameControl.PLAYER_LEVEL_KEY + _playerName, 1);
                PlayerPrefs.SetInt(GameControl.PLAYER_MAX_HEALTH_KEY + _playerName, 50);
                PlayerPrefs.SetInt(GameControl.PLAYER_MAX_MANA_KEY + _playerName, 500);
                PlayerPrefs.SetInt(GameControl.PLAYER_RANGED_ATTACK_KEY + _playerName, 30);
                PlayerPrefs.SetInt(GameControl.PLAYER_ATTACK_KEY + _playerName, 10);
				PlayerPrefs.SetInt(GameControl.PLAYER_DEFENSE_KEY + _playerName, 20);
				PlayerPrefs.SetFloat(GameControl.PLAYER_MOVEMENT_KEY + _playerName, 80f);
                PlayerPrefs.SetInt(GameControl.PLAYER_ATTACK_COOLDOWN_KEY + _playerName, 10);
                PlayerPrefs.SetInt(GameControl.PLAYER_POWER_ATTACK_MAX_VALUE_KEY + _playerName, 120);
                PlayerPrefs.SetInt(GameControl.PLAYER_ABILITY_1_COOLDOWN_KEY + _playerName, 40);
                PlayerPrefs.SetInt(GameControl.PLAYER_ABILITY_2_COOLDOWN_KEY + _playerName, 120);
                break;
			case PlayerStats.CharacterClass.Turtle://tank
                PlayerPrefs.SetInt(GameControl.PLAYER_LEVEL_KEY + _playerName, 1);
				PlayerPrefs.SetInt(GameControl.PLAYER_MAX_HEALTH_KEY + _playerName, 9000);
				PlayerPrefs.SetInt(GameControl.PLAYER_MAX_MANA_KEY + _playerName, 0);
                PlayerPrefs.SetInt(GameControl.PLAYER_RANGED_ATTACK_KEY + _playerName, 30);
				PlayerPrefs.SetInt(GameControl.PLAYER_ATTACK_KEY + _playerName, 411);
				PlayerPrefs.SetInt(GameControl.PLAYER_DEFENSE_KEY + _playerName, 350);
				PlayerPrefs.SetFloat(GameControl.PLAYER_MOVEMENT_KEY + _playerName, 30f);
                PlayerPrefs.SetInt(GameControl.PLAYER_ATTACK_COOLDOWN_KEY + _playerName, 10);
                PlayerPrefs.SetInt(GameControl.PLAYER_POWER_ATTACK_MAX_VALUE_KEY + _playerName, 120);
                PlayerPrefs.SetInt(GameControl.PLAYER_ABILITY_1_COOLDOWN_KEY + _playerName, 40);
                PlayerPrefs.SetInt(GameControl.PLAYER_ABILITY_2_COOLDOWN_KEY + _playerName, 120);
				break;
        }
    }

	void DeleteCharacter(string deletedPlayerName){
		string[] allPlayerNames = PlayerPrefs.GetString(GameControl.PLAYER_NAMES_KEY).Split(',');
		string newAllPlayerNames = "";
		foreach(string name in allPlayerNames){
			if(!name.Equals(deletedPlayerName)){
				if(newAllPlayerNames.Equals(""))
					newAllPlayerNames += name;
				else
					newAllPlayerNames += "," + name;
			}
		}
		PlayerPrefs.SetString(GameControl.PLAYER_NAMES_KEY,newAllPlayerNames);
        PlayerPrefs.DeleteKey(GameControl.PLAYER_LEVEL_KEY + deletedPlayerName);
		PlayerPrefs.DeleteKey(GameControl.PLAYER_CLASS_KEY + deletedPlayerName);
		PlayerPrefs.DeleteKey(GameControl.PLAYER_MAX_HEALTH_KEY + deletedPlayerName);
		PlayerPrefs.DeleteKey(GameControl.PLAYER_MAX_MANA_KEY + deletedPlayerName);
		PlayerPrefs.DeleteKey(GameControl.PLAYER_DEFENSE_KEY + deletedPlayerName);
		PlayerPrefs.DeleteKey(GameControl.PLAYER_ATTACK_KEY + deletedPlayerName);
        PlayerPrefs.DeleteKey(GameControl.PLAYER_RANGED_ATTACK_KEY + deletedPlayerName);
        PlayerPrefs.DeleteKey(GameControl.PLAYER_MOVEMENT_KEY + deletedPlayerName);
        PlayerPrefs.DeleteKey(GameControl.PLAYER_ATTACK_COOLDOWN_KEY + deletedPlayerName);
        PlayerPrefs.DeleteKey(GameControl.PLAYER_POWER_ATTACK_MAX_VALUE_KEY + deletedPlayerName);
        PlayerPrefs.DeleteKey(GameControl.PLAYER_ABILITY_1_COOLDOWN_KEY + deletedPlayerName);
        PlayerPrefs.DeleteKey(GameControl.PLAYER_ABILITY_2_COOLDOWN_KEY + deletedPlayerName);
        PlayerPrefs.DeleteKey(GameControl.PLAYER_SCORE_KEY + deletedPlayerName);
	}

    public static IEnumerator TextPopup(float waitTime) {
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
        Debug.Log("Host created Room. Connected to room");
    }

    void OnPhotonPlayerConnected() {
        DelegateHolder.TriggerPlayerHasConnected();
        Debug.Log("Player has connected");
    }

    void OnPhotonPlayerDisconnected() {
        DelegateHolder.TriggerPlayerHasDisconnected();
        Debug.Log("Player has disconnected");
    }

    public void OnJoinedRoom() {
        if (IsHost) {
            DelegateHolder.TriggerGenerateAndRenderMap(); //Generate map
            DelegateHolder.TriggerPlayerHasConnected(); // Boadcast that the player has connected
        }
        Debug.Log("Joined Room");
    }

    IEnumerator WaitForSceneToLoad(float timeDelay) {
        yield return new WaitForSeconds(timeDelay);
    }

}
