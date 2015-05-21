﻿using UnityEngine;
using System.Collections;
using System;

public class GameControl : MonoBehaviour {

    public static int Difficulty = 5; //from 1-10;
    public static float LevelUpScoreBase = 100;
    public static float LevelUpMultiplier = 2;// exp to get to level x = LevelUpScoreMultilier * LevelUpMultiplier * x

    public KeyCode ExitKey = KeyCode.Escape;
    public const string TITLESCREEN = "TitleScreen";
    public const string PLAYSCREEN = "PlayScene";

    public const string PLAYERNAMESKEY = "PLAYERNAMESKEY";
    public const string PLAYERCLASSKEY = "PLAYERCLASSKEY";
    public const string PLAYERMAXHEALTHKEY = "PLAYERMAXHEALTHKEY";
    public const string PLAYERMAXMANAKEY = "PLAYERMAXMANAKEY";
    public const string PLAYERDEFENSEKEY = "PLAYERDEFENSEKEY";
    public const string PLAYERATTACKKEY = "PLAYERATTACKKEY";
	public const string PLAYERMOVEMENTKEY = "PLAYERMOVEMENTKEY";
    public const string PLAYERLEVELKEY = "PLAYERLEVELKEY";

    

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
            if (Application.loadedLevelName.Equals(TITLESCREEN)) {//escape key for when on Title Screen
                Application.Quit();
            }
            if (Application.loadedLevelName.Equals(PLAYSCREEN)) {//escape key for when in play screen
                PhotonNetwork.LeaveRoom();
                Application.LoadLevel(TITLESCREEN);
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
