using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour {

    public Texture XPBarEmpty;
    public Texture XPBarFull;
    public float XPBarVerticalSize = 1 / 10;


    private Vector2 _scoreBarStart = new Vector2(Screen.width/10, 0);
    private Vector2 _scoreBarDims;

    void Start() {
        _scoreBarDims = new Vector2(Screen.width, Screen.height * XPBarVerticalSize);
    }

    void OnGUI() {
        //Draw rectangle on top for the score, all the way across, making it percent based on how much xp is needed for that level
        DrawScore();
    }

    void DrawScore() {
        //Draw bar
        float progress = (float)PlayerStats.PlayerScore / PlayerStats.CalculateLevelUpXP();
        GUI.DrawTexture(new Rect(_scoreBarStart.x, _scoreBarStart.y, _scoreBarDims.x, _scoreBarDims.y), XPBarEmpty);
        GUI.BeginGroup(new Rect(_scoreBarStart.x, _scoreBarStart.y, _scoreBarDims.x * Mathf.Clamp(progress,0,1), _scoreBarDims.y));
        GUI.DrawTexture(new Rect(0, 0, _scoreBarDims.x, _scoreBarDims.y), XPBarFull);
        GUI.EndGroup();

        //Draw Level
        GUI.BeginGroup(new Rect(0, 0, _scoreBarStart.x, _scoreBarDims.y*2));
        GUIStyle style = new GUIStyle();
        style.richText = true;
        style.alignment = TextAnchor.UpperCenter;
        style.fontSize = (int)(_scoreBarDims.y );
        GUI.Box(new Rect(0, 0, _scoreBarStart.x, _scoreBarDims.y), "Level: " + PlayerStats.PlayerLevel, style);
        GUI.EndGroup();
    }

    void drawHealth() {
        string scoreNum = PlayerStats.PlayerHealth + "";
        GUIStyle style = new GUIStyle();
        style.richText = true;
        style.alignment = TextAnchor.UpperCenter;
        style.fontSize = 40;
        GUI.Label(new Rect(Screen.width * 0.4f, Screen.height * 0.9f, Screen.width * 0.2f, Screen.height * 0.05f), scoreNum, style);
        GUI.color = Color.black;
    }

}