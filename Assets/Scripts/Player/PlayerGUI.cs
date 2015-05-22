using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour {

    void OnGUI() {
        //Draw rectangle on top for the score, all the way across, making it percent based on how much xp is needed for that level
        drawScore();
    }

    void drawScore() {
        /*
        GUI.DrawTexture(new Rect(pos.x, pos.y, size.x, size.y), progressBarEmpty);
        GUI.BeginGroup(new Rect(pos.x, pos.y, size.x * Mathf.Clamp01(progress), size.y));
        GUI.DrawTexture(new Rect(0, 0, size.x, size.y), progressBarFull);
        GUI.EndGroup();
         */

        string scoreNum = PlayerStats.PlayerScore + "";
        GUIStyle style = new GUIStyle();
        style.richText = true;
        style.alignment = TextAnchor.UpperCenter;
        style.fontSize = 40;
        GUI.Label(new Rect(Screen.width * 0.4f, Screen.height * 0.9f, Screen.width * 0.2f, Screen.height * 0.05f), scoreNum , style);
        GUI.color = Color.black;
    }

    void drawHealth() {
        string scoreNum = PlayerStats.PlayerScore + "";
        GUIStyle style = new GUIStyle();
        style.richText = true;
        style.alignment = TextAnchor.UpperCenter;
        style.fontSize = 40;
        GUI.Label(new Rect(Screen.width * 0.4f, Screen.height * 0.9f, Screen.width * 0.2f, Screen.height * 0.05f), scoreNum, style);
        GUI.color = Color.black;
    }

}