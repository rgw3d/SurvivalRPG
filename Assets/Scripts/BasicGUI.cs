using UnityEngine;
using System.Collections;

public class BasicGUI : MonoBehaviour {
	public float barDisplayShield; //current progress
	public float barDisplayHealth;
	public Texture2D emptyTex;
	public Texture2D fullTex;
	public NewBehaviourScript playerScript;
	
	void FixedUpdate() {
		barDisplayHealth = playerScript.healthValue/100f;
	}

	void OnGUI() {
		drawShield ();
		drawHealth ();
	}

	void drawShield() {
		Vector2 pos = new Vector2(Screen.width*0.4f,Screen.height*0.95f);
		Vector2 size = new Vector2(Screen.width*0.1f,Screen.height*0.03f);

		GUI.BeginGroup(new Rect(pos.x,pos.y, size.x, size.y));
		
			GUI.Box(new Rect(0,0, size.x, size.y), emptyTex);

			GUI.BeginGroup(new Rect(0,0, size.x * barDisplayShield, size.y));
				GUI.Box(new Rect(0,0, size.x, size.y), fullTex);
			GUI.EndGroup();

			string shieldPer = (int)(barDisplayShield*100f) + "";
			GUI.Box (new Rect (0, 0, size.x, size.y), "Shield - "+shieldPer+"%");

		GUI.EndGroup();
	}

	void drawHealth() {
		Vector2 pos = new Vector2(Screen.width*0.5f,Screen.height*0.95f);
		Vector2 size = new Vector2(Screen.width*0.1f,Screen.height*0.03f);
		//draw the background:
		GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
		
			GUI.Box(new Rect(0,0, size.x, size.y), emptyTex);
		
			//draw the filled-in part
			GUI.BeginGroup(new Rect(0,0, size.x * barDisplayHealth, size.y));
				GUI.Box(new Rect(0,0, size.x, size.y), fullTex);
			GUI.EndGroup();

			string healthPer = (int)(barDisplayHealth * 100f) + "";
			GUI.Box (new Rect (0, 0, size.x, size.y), "Health - "+healthPer+"%");

		GUI.EndGroup();
	}

	/*void drawScore() {
		//string scoreNum = (int)(scoreControl.Score) + "";
		GUIStyle style = new GUIStyle ();
		style.richText = true;
		style.alignment = TextAnchor.UpperCenter;
		GUI.Label(new Rect (Screen.width*0.4f,Screen.height*0.9f,Screen.width*0.2f ,Screen.height*0.05f),"<size=30>"+scoreNum+"</size>",style);
		GUI.color = Color.black;
	}
	*/


}