using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour {

    public Texture XPBarEmpty;
    public Texture XPBarFull;
    public float XPBarVerticalSize = 1 / 10;
    private GUIStyle LevelBoxStyle = null;
    private GUIStyle HealthStyle = null;

    public Texture HealthBarEmpty;
    public Texture HealthBarFull;

    private Vector2 _scoreBarStart = new Vector2(Screen.width/10, 0);
    private Vector2 _scoreBarDims;
    private Vector2 _healthBarStart;
    private Vector2 _healthBarDims;

    private Vector2 _manaBarStart = new Vector2(Screen.width / 60, Screen.height * 2 / 10);
    private Vector2 _manaBarDims = new Vector2(Screen.width / 40, Screen.height * 4 / 10);

    void Start() {
        _scoreBarDims = new Vector2(Screen.width/8, Screen.height * XPBarVerticalSize);

        _healthBarStart = new Vector2(Screen.width / 60, Screen.height * 2 / 10);
        _healthBarDims = new Vector2(Screen.width / 40, Screen.height * 4 / 10);

        _manaBarStart = new Vector2(_healthBarStart.x + _healthBarDims.x, _healthBarStart.y);
        _manaBarDims = new Vector2(_healthBarDims.x, _healthBarDims.y);
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
        //Draw bar
        float healthProgress = (float)(PlayerStats.MaxHealth-PlayerStats.PlayerHealth) / PlayerStats.MaxHealth;
        //float manaProgress = (float)(PlayerStats.MaxMana - PlayerStats.)

        GUI.DrawTexture(new Rect(_healthBarStart.x, _healthBarStart.y, _healthBarDims.x, _healthBarDims.y), HealthBarFull);
        GUI.BeginGroup(new Rect(_healthBarStart.x, _healthBarStart.y, _healthBarDims.x + _manaBarDims.x, _healthBarDims.y * Mathf.Clamp(healthProgress, 0, 1)));
        GUI.DrawTexture(new Rect(0, 0, _healthBarDims.x, _healthBarDims.y), HealthBarEmpty);
        //GUI.DrawTexture(new Rect(_manaBarStart.x, _manaBarStart.y, _manaBarDims.x, _manaBarDims.y), HealthBarEmpty);
        GUI.Label(new Rect(0, 0, _healthBarDims.x, _healthBarDims.y), "" + PlayerStats.PlayerHealth, HealthStyle);
        GUI.EndGroup();
    }

    

    private void InitStyles() {
        if (LevelBoxStyle == null) {
            LevelBoxStyle = new GUIStyle(GUI.skin.box);
            LevelBoxStyle.normal.background = MakeTex(2, 2, Color.cyan);
            LevelBoxStyle.normal.textColor = Color.black;
            LevelBoxStyle.richText = true;
            LevelBoxStyle.alignment = TextAnchor.UpperCenter;
            LevelBoxStyle.fontSize = (int)(_scoreBarDims.y * 0.6f);

        }
        if (HealthStyle == null) {
            HealthStyle = new GUIStyle();
            HealthStyle.richText = true;
            HealthStyle.alignment = TextAnchor.UpperCenter;
            HealthStyle.fontSize = (int)(_scoreBarDims.y);
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