using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

    public Sprite front;
    public Sprite back;
    public Sprite left;
    public Sprite right;
    private Sprite currentSprite;
	SpriteRenderer spriteRenderer;

	public float movementDistance = .001f;
    public float upMovement = .001f;
    public float sideMovement = .05f;

	public float healthValue = 100;

    public float yMax = 1;
    public float yMin = -1;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        
        spriteRenderer.sprite = front;
        currentSprite = front;
        
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.W)) {
            //transform.Translate(0, movementDistance, 0);
			if(transform.position.y<yMax)
           		rigidbody2D.AddForce(Vector2.up * upMovement);
            if (currentSprite != back){
                spriteRenderer.sprite = back;
                currentSprite = back;
            }
		}
		if (Input.GetKey (KeyCode.S)) {
			if(transform.position.y>yMin)
            	rigidbody2D.AddForce(Vector2.up * -1 * upMovement);
			//transform.Translate(0,-movementDistance,0);
            if (currentSprite != front){
                spriteRenderer.sprite = front;
                currentSprite = front;
            }
		}
		if (Input.GetKey (KeyCode.A)) {
            rigidbody2D.AddForce(Vector2.right * -1 * sideMovement);
			//transform.Translate(-movementDistance,0,0);
            if (currentSprite != left){
                spriteRenderer.sprite = left;
                currentSprite = left;
            }

		}
		if (Input.GetKey (KeyCode.D)) {
            rigidbody2D.AddForce(Vector2.right * sideMovement);
			//transform.Translate(movementDistance,0,0);
            if(currentSprite != right){
                spriteRenderer.sprite = right;
                currentSprite = right;
            }
		}

        float distToBot = Mathf.Abs(yMin - transform.position.y);
        Vector3 newScale = new Vector3(-.3f * distToBot + 2f,-.3f * distToBot + 2f,1);
        transform.localScale = newScale;
        //float distToBot = Mathf.Abs(yMin - transform.position.y);
        //Vector3 newScale = new Vector3(-.5f * distToBot + .885f,-.125f * distToBot + .885f,1);
        //transform.localScale = newScale;
        
        ///if ymin = -1 is scale of 1, and 1 is scale of .75  (-1,1) and (1,.75)
        ///-.125 x +.885 = y
        // -

	}
}
