using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {

    public static int Difficulty = 5; //Out of 0-10
    public static string FilePath = "C/path/to/file";
    //Music when we have it
    //Any other settings
    public KeyCode ExitKey = KeyCode.Escape;
    public readonly static string TITLESCREEN = "TitleScreen";
    public readonly static string PLAYSCREEN = "PlayScreen";
    


	void Start () {
        DontDestroyOnLoad(this);
	}
	
	void Update () {
	    //use this to control what music is playing
        if (Input.GetKey(ExitKey)) {
            if (Application.loadedLevelName.Equals(TITLESCREEN)) {//escape key for when on Title Screen
                Application.Quit();
            }
            if (Application.loadedLevelName.Equals(PLAYSCREEN)) {//escape key for when in play screen
                PhotonNetwork.LeaveRoom();
                Application.LoadLevel(TITLESCREEN);
            }
            
        }
        
	}
}
