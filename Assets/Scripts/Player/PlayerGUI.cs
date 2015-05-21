using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour {

    public float PlayerScore = 0;
    
    public float PlayerHealth = 0;

    void Start() {
        DelegateHolder.StatChange += UpdateGUIStats;//add the method to the event, and the event is made from the delegate
    }

    void OnGUI() {
        drawScore();
    }

    void drawScore() {
        string scoreNum = (int)(PlayerScore) + "";
        GUIStyle style = new GUIStyle();
        style.richText = true;
        style.alignment = TextAnchor.UpperCenter;
        style.fontSize = 40;
        GUI.Label(new Rect(Screen.width * 0.4f, Screen.height * 0.9f, Screen.width * 0.2f, Screen.height * 0.05f), scoreNum , style);
        GUI.color = Color.black;
    }

    void drawHealth() {
        string scoreNum = (int)(PlayerHealth) + "";
        GUIStyle style = new GUIStyle();
        style.richText = true;
        style.alignment = TextAnchor.UpperCenter;
        style.fontSize = 40;
        GUI.Label(new Rect(Screen.width * 0.4f, Screen.height * 0.9f, Screen.width * 0.2f, Screen.height * 0.05f), scoreNum, style);
        GUI.color = Color.black;
    }

    float UpdateGUIStats(StatType statType, float change) {
        if (statType == StatType.Score) {
            PlayerScore += change;
            return PlayerScore;
        }
        else if (statType == StatType.Health) {
            PlayerHealth += change;
            return PlayerHealth;
        }
        else
            return -1;
    }

}