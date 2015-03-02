using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour, PlayerClass {

    public Sprite FrontSprite;
    public Sprite BackSprite;
    public Sprite LeftSprite;
    public Sprite RightSprite;
    public Sprite FrontAttack;
    public Sprite BackAttack;
    public Sprite LeftAttack;
    public Sprite RightAttack;
    private Sprite _currentSprite;
	private SpriteRenderer _spriteRenderer;

    public float VerticalMovement = 15f;
    public float HorizontalMovement = 15;

    public KeyCode UpKey;
    public KeyCode DownKey;
    public KeyCode LeftKey;
    public KeyCode RightKey;
    public KeyCode AttackKey;



    private CardinalDirection _playerDirection = CardinalDirection.front;
    

	// Use this for initialization
	void Start () {


        _spriteRenderer = GetComponent<SpriteRenderer>();
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        
        _spriteRenderer.sprite = FrontSprite;
        _currentSprite = FrontSprite;
        
	}

    private enum CardinalDirection {
        front,
        back,
        left,
        right
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKey(KeyCode.E)) { //just a test of the ability to work
            DelegateHolder.TriggerPlayerStatChange(StatType.Score, 1f);   
        }

        playerMovement();
        playerSprite();
        playerAttack();

	}


    public void playerMovement() {
        if (Input.GetKey(UpKey)) {
            rigidbody2D.AddForce(Vector2.up * VerticalMovement);
            _playerDirection = CardinalDirection.back;
        }
        if (Input.GetKey(DownKey)) {
            rigidbody2D.AddForce(Vector2.up * -1 * VerticalMovement);
            _playerDirection = CardinalDirection.front;
        }
        if (Input.GetKey(LeftKey)) {
            rigidbody2D.AddForce(Vector2.right * -1 * HorizontalMovement);
            _playerDirection = CardinalDirection.left;

        }
        if (Input.GetKey(RightKey)) {
            rigidbody2D.AddForce(Vector2.right * HorizontalMovement);
            _playerDirection = CardinalDirection.right;
        }
        
    }

    public void playerSprite() {
        if (_playerDirection == CardinalDirection.back) {
            if(Input.GetKey(AttackKey))
                _spriteRenderer.sprite = BackAttack;
            else
                _spriteRenderer.sprite = BackSprite;
        }
        if (_playerDirection == CardinalDirection.front) {
            if(Input.GetKey(AttackKey))
                _spriteRenderer.sprite = FrontAttack;
            else
                _spriteRenderer.sprite = FrontSprite;
        }
        if (_playerDirection == CardinalDirection.left) {
            if(Input.GetKey(AttackKey))
                _spriteRenderer.sprite = LeftAttack;
            else
                _spriteRenderer.sprite = LeftSprite;
        }
        if (_playerDirection == CardinalDirection.right) {
            if(Input.GetKey(AttackKey))
                _spriteRenderer.sprite = RightAttack;
            else
                _spriteRenderer.sprite = RightSprite;
        }
    }

    public void playerAttack() {
        DelegateHolder.TriggerPlayerAttack(Input.GetKey(AttackKey));
    }
}
