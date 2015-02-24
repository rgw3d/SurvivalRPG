using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour, PlayerClass {

    public Sprite FrontSprite;
    public Sprite BackSprite;
    public Sprite LeftSprite;
    public Sprite RightSprite;
    private Sprite _currentSprite;
	private SpriteRenderer _spriteRenderer;

    public float VerticalMovement = 15f;
    public float HorizontalMovement = 15;
    public bool ScalePlayer = false;

    public KeyCode UpKey;
    public KeyCode DownKey;
    public KeyCode LeftKey;
    public KeyCode RightKey;

    public event ChangePlayerScore OnChangeScore;

    public float yMax = 1;
    public float yMin = -1;

    public int AttackCooldown = 100;
    private int _atkCoolCounter = 0;

	// Use this for initialization
	void Start () {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        
        _spriteRenderer.sprite = FrontSprite;
        _currentSprite = FrontSprite;
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKey(KeyCode.E)) {
            Debug.Log(OnChangeScore(StatType.Score, 1f));
        }

        playerMovement();
        if(ScalePlayer)
            playerScale();

	}

    public void playerScale() {
        float distToBot = Mathf.Abs(yMin - transform.position.y);
        Vector3 newScale = new Vector3(-.3f * distToBot + 2f, -.3f * distToBot + 2f, 1);
        transform.localScale = newScale;
    }

    public void playerMovement() {
        if (Input.GetKey(UpKey)) {
            if (transform.position.y < yMax)
                rigidbody2D.AddForce(Vector2.up * VerticalMovement);
            if (_currentSprite != BackSprite) {
                _spriteRenderer.sprite = BackSprite;
                _currentSprite = BackSprite;
            }
        }
        if (Input.GetKey(DownKey)) {
            if (transform.position.y > yMin)
                rigidbody2D.AddForce(Vector2.up * -1 * VerticalMovement);
            if (_currentSprite != FrontSprite) {
                _spriteRenderer.sprite = FrontSprite;
                _currentSprite = FrontSprite;
            }
        }
        if (Input.GetKey(LeftKey)) {
            rigidbody2D.AddForce(Vector2.right * -1 * HorizontalMovement);
            if (_currentSprite != LeftSprite) {
                _spriteRenderer.sprite = LeftSprite;
                _currentSprite = LeftSprite;
            }

        }
        if (Input.GetKey(RightKey)) {
            rigidbody2D.AddForce(Vector2.right * HorizontalMovement);
            if (_currentSprite != RightSprite) {
                _spriteRenderer.sprite = RightSprite;
                _currentSprite = RightSprite;
            }
        }
        
    }

    

    public void playerAttack() {
        if(Input.GetKey(KeyCode.Space)){

        }
    }
}
