using UnityEngine;
using System.Collections;
using System;

public class GameControl : MonoBehaviour {

    public static int Difficulty = 5; //from 1-10;

    public KeyCode ExitKey = KeyCode.Escape;

    public const string TITLE_SCREEN = "TitleScreen";
    public const string PLAY_SCREEN = "PlayScene";
    public const string PLAYER_NAMES_KEY = "PLAYERNAMESKEY";
    public const string PLAYER_CLASS_KEY = "PLAYERCLASSKEY";
    public const string PLAYER_MAX_HEALTH_KEY = "PLAYERMAXHEALTHKEY";
    public const string PLAYER_MAX_MANA_KEY = "PLAYERMAXMANAKEY";
    public const string PLAYER_DEFENSE_KEY = "PLAYERDEFENSEKEY";
    public const string PLAYER_ATTACK_KEY = "PLAYERATTACKKEY";
    public const string PLAYER_RANGED_ATTACK_KEY = "PLAYERRANGEDATTACKKEY";
	public const string PLAYER_MOVEMENT_KEY = "PLAYERMOVEMENTKEY";
    public const string PLAYER_LEVEL_KEY = "PLAYERLEVELKEY";
    public const string PLAYER_SCORE_KEY = "PLAYERSCOREKEY";    

	void Start () {
        DontDestroyOnLoad(this);
        DelegateHolder.OnPlayerHasConnected += PlayerConnected;
        DelegateHolder.OnPlayerHasDisconnected += PlayerDisconnected;
    }

    public void PlayerConnected() {
    }

    public void PlayerDisconnected() {
    }
	
	void Update () {
        if (Input.GetKey(ExitKey)) {
            if (Application.loadedLevelName.Equals(TITLE_SCREEN)) {//escape key for when on Title Screen
                Application.Quit();
            }
            if (Application.loadedLevelName.Equals(PLAY_SCREEN)) {//escape key for when in play screen
                PhotonNetwork.LeaveRoom();
                Application.LoadLevel(TITLE_SCREEN);
            }
        }
        if (Input.GetKey(KeyCode.T) || Input.GetKey(KeyCode.Slash)) {
            ChatDisplay.ChatState = ChatDisplay.ChattingState.ChatOpenAndTyping;

        }
	}

    public static void ClearMap() {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
            Destroy(enemy);
        }
    }



    
}
