using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

    public Sprite frontSprite;
    public Sprite backSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;
    private Sprite currentSprite;
	private SpriteRenderer spriteRenderer;

	public float movementDistance = .001f;
    public float upMovement = .001f;
    public float sideMovement = .05f;
    public bool scalePlayer = false;

    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode leftKey;
    public KeyCode rightKey;

	public float healthValue = 100;

    public float yMax = 1;
    public float yMin = -1;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        
        spriteRenderer.sprite = frontSprite;
        currentSprite = frontSprite;
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        playerMovement();

        if(scalePlayer)
            playerScale();

	}

    private void playerScale() {
        float distToBot = Mathf.Abs(yMin - transform.position.y);
        Vector3 newScale = new Vector3(-.3f * distToBot + 2f, -.3f * distToBot + 2f, 1);
        transform.localScale = newScale;
    }

    private void playerMovement() {
        if (Input.GetKey(upKey)) {
            //transform.Translate(0, movementDistance, 0);
            if (transform.position.y < yMax)
                rigidbody2D.AddForce(Vector2.up * upMovement);
            if (currentSprite != backSprite) {
                spriteRenderer.sprite = backSprite;
                currentSprite = backSprite;
            }
        }
        if (Input.GetKey(downKey)) {
            if (transform.position.y > yMin)
                rigidbody2D.AddForce(Vector2.up * -1 * upMovement);
            //transform.Translate(0,-movementDistance,0);
            if (currentSprite != frontSprite) {
                spriteRenderer.sprite = frontSprite;
                currentSprite = frontSprite;
            }
        }
        if (Input.GetKey(leftKey)) {
            rigidbody2D.AddForce(Vector2.right * -1 * sideMovement);
            //transform.Translate(-movementDistance,0,0);
            if (currentSprite != leftSprite) {
                spriteRenderer.sprite = leftSprite;
                currentSprite = leftSprite;
            }

        }
        if (Input.GetKey(rightKey)) {
            rigidbody2D.AddForce(Vector2.right * sideMovement);
            //transform.Translate(movementDistance,0,0);
            if (currentSprite != rightSprite) {
                spriteRenderer.sprite = rightSprite;
                currentSprite = rightSprite;
            }
        }
        
    }
}
