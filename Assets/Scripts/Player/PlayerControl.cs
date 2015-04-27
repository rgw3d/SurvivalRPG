using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class PlayerControl : Photon.MonoBehaviour{

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

	public float movementSpeed;

    public KeyCode UpKey;
    public KeyCode DownKey;
    public KeyCode LeftKey;
    public KeyCode RightKey;
    public KeyCode AttackKey;

	public GameObject southSword;
	public GameObject eastSword;
	public GameObject northSword;
	public GameObject westSword;

    private CardinalDirection _playerDirection = CardinalDirection.front;

    private Vector3 latestCorrectPos;
    private Vector3 onUpdatePos;
    private float lerpFraction;
    

	// Use this for initialization
	void Start () {
        latestCorrectPos = transform.position;
        onUpdatePos = transform.position;

        movementSpeed = PlayerStats.MovementSpeed;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = FrontSprite;

		southSword = gameObject.transform.FindChild("SouthSword").gameObject;
		eastSword = gameObject.transform.FindChild("EastSword").gameObject;
		northSword = gameObject.transform.FindChild("NorthSword").gameObject;
		westSword = gameObject.transform.FindChild("WestSword").gameObject;

	}

    private enum CardinalDirection {
        front,
        back,
        left,
        right
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        
        if (photonView.isMine) {
            if (GameControl.ChatState == GameControl.ChattingState.ChatClosedButShowing || GameControl.ChatState == GameControl.ChattingState.NoUsername) { 
                playerMovement();
                playerSprite();
            }
            if (Input.GetKey(KeyCode.E)) { //just a test of the ability to work
                DelegateHolder.TriggerPlayerStatChange(StatType.Score, 1f);
            }
			if (Input.GetKey(AttackKey)){
				playerAttack();
			}
        }
        else {
            SyncedMovement();
        }

	}


    public void playerMovement() {
        //Debug.Log("movement method called");
        //Debug.Log(movementSpeed);
        if (Input.GetKey(UpKey)) {
            rigidbody2D.AddForce(Vector2.up * movementSpeed);
            _playerDirection = CardinalDirection.back;
        }
        if (Input.GetKey(DownKey)) {
            rigidbody2D.AddForce(Vector2.up * -1 * movementSpeed);
            _playerDirection = CardinalDirection.front;
        }
        if (Input.GetKey(LeftKey)) {
			rigidbody2D.AddForce(Vector2.right * -1 * movementSpeed);
            _playerDirection = CardinalDirection.left;

        }
        if (Input.GetKey(RightKey)) {
			rigidbody2D.AddForce(Vector2.right * movementSpeed);
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
        //have all valid hitboxes spawned at the start of the map, and just move them to the proper location and back when we don't need them
		//this way we save on having to instantiate and delete objects all the time

		//on key hit, move the correct hitbox to the player, adjust for player rotation/click position, and move the hitbox, and then move it
		//back to an area we can't see when its offscreen
		if(_playerDirection == CardinalDirection.left){
		}
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            Vector3 pos = transform.localPosition;
            Quaternion rot = transform.localRotation;
            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
        }
        else {
            // Receive latest state information
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);

            latestCorrectPos = pos;                 // save this to move towards it in FixedUpdate()
            onUpdatePos = transform.localPosition;  // we interpolate from here to latestCorrectPos
            lerpFraction = 0;                           // reset the fraction we alreay moved. see Update()

            transform.localRotation = rot;          // this sample doesn't smooth rotation
        }
    }

    private void SyncedMovement() {
        // We get 10 updates per sec. sometimes a few less or one or two more, depending on variation of lag.
        // Due to that we want to reach the correct position in a little over 100ms. This way, we usually avoid a stop.
        // Lerp() gets a fraction value between 0 and 1. This is how far we went from A to B.
        //
        // Our fraction variable would reach 1 in 100ms if we multiply deltaTime by 10.
        // We want it to take a bit longer, so we multiply with 9 instead.

        lerpFraction = lerpFraction + Time.deltaTime * 9;
        transform.localPosition = Vector3.Lerp(onUpdatePos, latestCorrectPos, lerpFraction);    // set our pos between A and B
    }

}
