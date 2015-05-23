using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour {

    public Texture XPBarEmpty;
    public Texture XPBarFull;
    public float XPBarVerticalSize = 1 / 10;
    private GUIStyle GUIBox = null;
    private GUIStyle LevelBoxStyle = null;
    private GUIStyle HealthStyle = null;

    public Texture HealthBarEmpty;
    public Texture HealthBarFull;

    public Vector2 Bounds;

    private Vector2 _scoreBarStart = new Vector2(Screen.width/10, 0);
    private Vector2 _scoreBarDims;
    private Vector2 _healthBarStart;
    private Vector2 _healthBarDims;

    private Vector2 _manaBarStart = new Vector2(Screen.width / 60, Screen.height * 2 / 10);
    private Vector2 _manaBarDims = new Vector2(Screen.width / 40, Screen.height * 4 / 10);

    void Start() {
        Bounds = new Vector2(Screen.width / 6, Screen.height/4 );
        _scoreBarDims = new Vector2(Screen.width/8, Screen.height * XPBarVerticalSize);

        _healthBarStart = new Vector2(Screen.width / 60, Screen.height * 2 / 10);
        _healthBarDims = new Vector2(Screen.width / 40, Screen.height * 4 / 10);

        _manaBarStart = new Vector2(_healthBarStart.x + _healthBarDims.x, _healthBarStart.y);
        _manaBarDims = new Vector2(_healthBarDims.x, _healthBarDims.y);
    }

    void OnGUI() {
        InitStyles();
        GUI.BeginGroup(new Rect(0, 0, Bounds.x, Bounds.y), GUIBox);//set the box
            //Top of box there is the Level # and XP -- takes up a 1/5th of the height
            DrawScore();
            //Then The health bar on the left under the level/xp
            DrawHealth();

        GUI.EndGroup();
    }

    void DrawScore() {
        float height = Bounds.y / 5;
        //Draw Level: # . This will take up a third of the score
        GUI.Box(new Rect(0, 0, Bounds.x/3, height), "Level: " + PlayerStats.PlayerLevel, LevelBoxStyle);

        //Draw xp bar
        float progress = (float)PlayerStats.PlayerScore / PlayerStats.CalculateLevelUpXP();
        GUI.DrawTexture(new Rect(Bounds.x / 3, 0, Bounds.x * 2 / 3, height), XPBarEmpty);
        GUI.BeginGroup(new Rect(Bounds.x / 3, 0, Bounds.x * 2 / 3 * Mathf.Clamp(progress, 0, 1), height));
        GUI.DrawTexture(new Rect(0, 0, Bounds.x * 2 / 3, height), XPBarFull);
        GUI.EndGroup();

    }

    void DrawHealth() {
        float startingHeight = Bounds.y / 5;
        float height = Bounds.y * 3 / 5;
        float width = Bounds.x / 4;

        //Draw bar
        float healthProgress = (float)(PlayerStats.MaxHealth-PlayerStats.PlayerHealth) / PlayerStats.MaxHealth;
        GUI.DrawTexture(new Rect(0, startingHeight, width,height), HealthBarFull);
        GUI.BeginGroup(new Rect(0,startingHeight, width, height * Mathf.Clamp(healthProgress, 0, 1)));
        GUI.DrawTexture(new Rect(0, 0, width, height), HealthBarEmpty);
        GUI.Label(new Rect(0, 0, width, height), "" + PlayerStats.PlayerHealth, HealthStyle);
        GUI.EndGroup();

        GUIStyle style = new GUIStyle();
        style.richText = true;
        style.alignment = TextAnchor.LowerCenter;
        style.normal.textColor = Color.black;
        style.fontSize = (int)(Bounds.y*.1);
        GUI.Label(new Rect(0, startingHeight, width, height), "Health", style);

    }

    

    private void InitStyles() {
        if (GUIBox == null) {
            GUIBox = new GUIStyle(GUI.skin.box);
            GUIBox.normal.background = MakeTex(2, 2, new Color(1f, 1f, 1f, .25f));
        }
        if (LevelBoxStyle == null) {
            LevelBoxStyle = new GUIStyle(GUI.skin.box);
            LevelBoxStyle.normal.background = MakeTex(2, 2, Color.cyan);
            LevelBoxStyle.normal.textColor = Color.black;
            LevelBoxStyle.richText = true;
            LevelBoxStyle.alignment = TextAnchor.UpperCenter;
            LevelBoxStyle.fontSize = (int)(Bounds.y * 0.1f);

        }
        if (HealthStyle == null) {
            HealthStyle = new GUIStyle();
            HealthStyle.richText = true;
            HealthStyle.alignment = TextAnchor.UpperCenter;
            HealthStyle.fontSize = (int)(Bounds.y);
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