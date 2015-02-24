using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour {


    public float playerScore = 0;
    public float playerHealth = 0;

    

    void Start() {
        PlayerControl playerController = FindObjectOfType<PlayerControl>();
        playerController.OnChangeScore += UpdateGUIStats;//add the method to the event, and the event is made from the delegate
    }

    void OnGUI() {
        drawScore();
    }

    void drawScore() {
        string scoreNum = (int)(playerScore) + "";
        GUIStyle style = new GUIStyle();
        style.richText = true;
        style.alignment = TextAnchor.UpperCenter;
        GUI.Label(new Rect(Screen.width * 0.4f, Screen.height * 0.9f, Screen.width * 0.2f, Screen.height * 0.05f), "<size=30>" + scoreNum + "</size>", style);
        GUI.color = Color.black;
    }

    float UpdateGUIStats(StatType statType, float change) {
        if (statType == StatType.Score) {
            playerScore += change;
            return playerScore;
        }
        else if (statType == StatType.Health) {
            playerHealth += change;
            return playerHealth;
        }
        else
            return -1;


    }

}