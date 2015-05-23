using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour {

    public Texture XPBarEmpty;
    public Texture XPBarFull;
    public Texture HealthBarEmpty;
    public Texture HealthBarFull;
    public Texture ProgressBarEmpty;
    public Texture PowerBarFull;
    public Texture Ability1Full;
    public Texture Ability2Full;
    private GUIStyle GUIBox = null;
    private GUIStyle LevelBoxStyle = null;
    private GUIStyle HealthNumberStyle = null;
    private GUIStyle HealthWordStyle = null;    

    public Vector2 Bounds;

    void Start() {
        Bounds = new Vector2(Screen.width / 6, Screen.height/4 );
    }

    void OnGUI() {
        InitStyles();
        GUI.BeginGroup(new Rect(0, 0, Bounds.x, Bounds.y), GUIBox);//set the box
            //Top of box there is the Level # and XP -- takes up a 1/5th of the height
            DrawScore();
            //Then The health bar on the left under the level/xp
            DrawHealth();
            ProgressBars();

        GUI.EndGroup();
    }

    void DrawScore() {
        float height = Bounds.y / 5;
        //Draw Level: # . This will take up a third of the score
        GUI.Box(new Rect(0, 0, Bounds.x/3, height), "Level: " + PlayerStats.PlayerLevel, LevelBoxStyle);

        //Draw xp bar
        float progress = (float)PlayerStats.PlayerScore / PlayerStats.CalculateLevelUpXP();
        GUI.DrawTexture(new Rect(Bounds.x / 3, 0, Bounds.x * 2 / 3, height), XPBarEmpty);
        GUI.BeginGroup(new Rect(Bounds.x / 3, 0, Bounds.x * 2 / 3 * Mathf.Clamp01(progress), height));
        GUI.DrawTexture(new Rect(0, 0, Bounds.x * 2 / 3, height), XPBarFull);
        GUI.EndGroup();

    }

    void DrawHealth() {
        float startingHeight = Bounds.y / 5;
        float height = Bounds.y * 4 / 5;
        float width = Bounds.x / 4;

        //Draw bar
        float healthProgress = (float)(PlayerStats.MaxHealth-PlayerStats.PlayerHealth) / PlayerStats.MaxHealth;
        GUI.DrawTexture(new Rect(0, startingHeight, width,height), HealthBarFull);
        GUI.BeginGroup(new Rect(0,startingHeight, width, height * Mathf.Clamp01(healthProgress)));
        GUI.DrawTexture(new Rect(0, 0, width, height), HealthBarEmpty);
        GUI.Label(new Rect(0, 0, width, height), "" + PlayerStats.PlayerHealth, HealthNumberStyle);
        GUI.EndGroup();

        GUI.Label(new Rect(0, startingHeight, width, height), "Health", HealthWordStyle);

    }

    void ProgressBars() {
        float startingHeight = Bounds.y / 5 + Bounds.y / 40;
        float height = Bounds.y / 5;
        float startingWidth = Bounds.x / 4 + Bounds.x / 30;
        float width = Bounds.x * 3 / 4;

        //Power Attack
        float powerProgress = (float)ChipmunkPlayerControl._chargedValue / ChipmunkPlayerControl.MaxChargeTime;
        GUI.DrawTexture(new Rect(startingWidth, startingHeight, width, height), ProgressBarEmpty);
        GUI.BeginGroup(new Rect(startingWidth, startingHeight, width * Mathf.Clamp01(powerProgress), height));
        GUI.DrawTexture(new Rect(0, 0, width, height), PowerBarFull);
        GUI.EndGroup();

        //Ability 1
        float ability1Progress = (float)ChipmunkPlayerControl._ability1Cooldown/ ChipmunkPlayerControl.Ability1CooldownValue;
        GUI.DrawTexture(new Rect(startingWidth,  startingHeight * 2, width, height), ProgressBarEmpty);
        GUI.BeginGroup(new Rect(startingWidth, startingHeight * 2, width * Mathf.Clamp01(ability1Progress), height));
        GUI.DrawTexture(new Rect(0, 0, width, height), Ability1Full);
        GUI.EndGroup();

        //Ability 2
        float ability2Progress = (float)ChipmunkPlayerControl._ability2Cooldown / ChipmunkPlayerControl.Ability2CooldownValue;
        GUI.DrawTexture(new Rect(startingWidth, startingHeight * 3, width, height), ProgressBarEmpty);
        GUI.BeginGroup(new Rect(startingWidth, startingHeight * 3, width * Mathf.Clamp01(ability2Progress), height));
        GUI.DrawTexture(new Rect(0, 0, width, height), Ability2Full);
        GUI.EndGroup();

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
        if (HealthNumberStyle == null) {
            HealthNumberStyle = new GUIStyle();
            HealthNumberStyle.richText = true;
            HealthNumberStyle.alignment = TextAnchor.UpperCenter;
            HealthNumberStyle.fontSize = (int)(Bounds.y * .15f);
        }
        if (HealthWordStyle == null) {
            HealthWordStyle = new GUIStyle();
            HealthWordStyle.richText = true;
            HealthWordStyle.alignment = TextAnchor.LowerCenter;
            HealthWordStyle.fontSize = (int)(Bounds.y * .13f);
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