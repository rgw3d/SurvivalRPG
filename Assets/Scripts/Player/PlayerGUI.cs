using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour {

    public Texture XPBarEmpty;
    public Texture XPBarFull;
    public float XPBarVerticalSize = 1 / 10;
    private GUIStyle LevelBoxStyle = null;

    public Texture HealthBarEmpty;
    public Texture HealthBarFull;

    private Vector2 _scoreBarStart = new Vector2(Screen.width/10, 0);
    private Vector2 _scoreBarDims;

    private Vector2 _healthBarStart = new Vector2(Screen.width * 19 / 20, Screen.height / 8);
    private Vector2 _healthBarDims = new Vector2(Screen.width / 20, Screen.height * 3 / 8);

    void Start() {
        _scoreBarDims = new Vector2(Screen.width, Screen.height * XPBarVerticalSize);
        
    }

    void OnGUI() {
        InitStyles();
        DrawScore();
        DrawHealth();
    }

    void DrawScore() {
        //Draw bar
        float progress = (float)PlayerStats.PlayerScore / PlayerStats.CalculateLevelUpXP();
        GUI.DrawTexture(new Rect(_scoreBarStart.x, _scoreBarStart.y, _scoreBarDims.x, _scoreBarDims.y), XPBarEmpty);
        GUI.BeginGroup(new Rect(_scoreBarStart.x, _scoreBarStart.y, _scoreBarDims.x * Mathf.Clamp(progress,0,1), _scoreBarDims.y));
        GUI.DrawTexture(new Rect(0, 0, _scoreBarDims.x, _scoreBarDims.y), XPBarFull);
        GUI.EndGroup();

        //Draw Level    
        GUI.Box(new Rect(0, 0, _scoreBarStart.x, _scoreBarDims.y), "Level: " + PlayerStats.PlayerLevel, LevelBoxStyle);
    }

    void DrawHealth() {
        //Right side vertical bar

        GUIStyle style = new GUIStyle();
        style.richText = true;
        style.alignment = TextAnchor.UpperCenter;
        style.fontSize = (int)(_scoreBarDims.y);

        //Draw bar
        float progress = (float)(PlayerStats.MaxHealth-PlayerStats.PlayerHealth) / PlayerStats.MaxHealth;
        GUI.DrawTexture(new Rect(_healthBarStart.x, _healthBarStart.y, _healthBarDims.x, _healthBarDims.y), HealthBarFull);
        GUI.BeginGroup(new Rect(_healthBarStart.x, _healthBarStart.y, _healthBarDims.x, _healthBarDims.y * Mathf.Clamp(progress, 0, 1)));
        GUI.DrawTexture(new Rect(0, 0, _healthBarDims.x, _healthBarDims.y), HealthBarEmpty);
        GUI.Label(new Rect(0, 0, _healthBarDims.x, _healthBarDims.y), "" + PlayerStats.PlayerHealth, style);
        GUI.EndGroup();
    }

    private void InitStyles() {
        if (LevelBoxStyle == null) {
            LevelBoxStyle = new GUIStyle(GUI.skin.box);
            LevelBoxStyle.normal.background = MakeTex(2, 2, Color.gray);
            LevelBoxStyle.normal.textColor = Color.black;
            LevelBoxStyle.richText = true;
            LevelBoxStyle.alignment = TextAnchor.UpperCenter;
            LevelBoxStyle.fontSize = (int)(_scoreBarDims.y * 0.6f);

        }
    }

    private Texture2D MakeTex(int width, int height, Color col) {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i) {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

}