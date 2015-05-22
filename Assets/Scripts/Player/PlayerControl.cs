using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class PlayerControl : Photon.MonoBehaviour{

    public Sprite NormalSprite;
    public Sprite AttackSprite;

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

	public Fireball Ability1Prefab;
	GameObject ability1;
	Fireball ability1script;
	public int Ability1Cooldown = 0;
	public Chipmunk2Lunge Ability2;
	public int Ability2Cooldown = 0;
	public int Ability2RecoverTime = 0;

    private PlayerState _playerState = PlayerState.standing;

    public int AttackCooldownValue = 15;
    private int _attackCooldown = 0;
    public float RotationSpeed = 5;

    private Vector3 _latestCorrectPos;
    private Vector3 _onUpdatePos;
    private Quaternion _latestCorrectRot;
    private Quaternion _onUpdateRot;
    private float _lerpFraction;

    
	// Use this for initialization
	void Start () {
        _latestCorrectPos = transform.position;
        _onUpdatePos = transform.position;

        movementSpeed = PlayerStats.MovementSpeed;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = NormalSprite;
	
		ability1 = PhotonNetwork.Instantiate(Ability1Prefab.name, new Vector2(-100,-100), Quaternion.identity, 0);
		ability1script = ability1.GetComponent("Fireball") as Fireball;
	}

    private enum CardinalDirection {
        front = 3,
        back = 1,
        left = 4,
        right = 2
    }

    private enum PlayerState {
        attacking = 0,
        walking = 1,
        standing = 2,
		lunging = 3,
		charging = 4
    }

    void Update() {
        if (photonView.isMine) { 
            Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
            Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
            float angle = Mathf.Atan2(positionOnScreen.y - mouseOnScreen.y, positionOnScreen.x - mouseOnScreen.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, angle)), RotationSpeed * Time.deltaTime);
            Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

    }

	void FixedUpdate () {
        if (photonView.isMine) {
            if (ChatDisplay.ChatState == ChatDisplay.ChattingState.ChatClosedButShowing
                || ChatDisplay.ChatState == ChatDisplay.ChattingState.NoUsername) {
                    if (_playerState != PlayerState.attacking && _playerState != PlayerState.lunging) //only update movement if not attacking
                        PlayerMovement();
            }
            if (Input.GetKey(KeyCode.E)) { //just a test of the ability to work
                DelegateHolder.TriggerPlayerStatChange(StatType.Score, 1f);
            }
			if (Input.GetKey(KeyCode.Alpha1)){
				PlayerAbility(1);
			}
			if (Input.GetKey(KeyCode.Alpha2)){
				PlayerAbility(2);
			}
            PlayerAttack();
			LowerCooldowns();
        }
        else {
            SyncedMovement();
        }

        PlayerSprite();
	}


    public void PlayerMovement() {
        if (Input.GetKey(UpKey)) {
            rigidbody2D.AddForce(Vector2.up * movementSpeed);
            //_playerDirection = CardinalDirection.back;
            _playerState = PlayerState.walking;
        }
        if (Input.GetKey(DownKey)) {
            rigidbody2D.AddForce(Vector2.up * -1 * movementSpeed);
            _playerState = PlayerState.walking;
        }
        if (Input.GetKey(LeftKey)) {
            rigidbody2D.AddForce(Vector2.right * -1 * movementSpeed);
            _playerState = PlayerState.walking;

        }
        if (Input.GetKey(RightKey)) {
            rigidbody2D.AddForce(Vector2.right * movementSpeed);
            _playerState = PlayerState.walking;
        }
    }


    public void PlayerSprite() {
            if (_playerState == PlayerState.attacking)
                _spriteRenderer.sprite = AttackSprite;
            else
                _spriteRenderer.sprite = NormalSprite;
    }

    public void PlayerAttack() {
		if (_attackCooldown == 0 && Input.GetKeyDown(AttackKey) && _playerState != PlayerState.attacking && _playerState != PlayerState.lunging) {
            DelegateHolder.TriggerPlayerAttack(true);
            _attackCooldown = AttackCooldownValue;
            _playerState = PlayerState.attacking;
        }
        if(_attackCooldown > 0){
            _attackCooldown--;
        }
        
        if (_attackCooldown == 0 && _playerState == PlayerState.attacking) {
            _playerState = PlayerState.standing;
            DelegateHolder.TriggerPlayerAttack(false);
        }
    }

	void PlayerAbility(int abilityNumber){
		switch(abilityNumber){
		case 1:
			if(Ability1Cooldown == 0 && _playerState != PlayerState.attacking){
				ability1.transform.position = transform.position;
				ability1.transform.rotation = transform.rotation;
				ability1.rigidbody2D.velocity = Vector3.zero;
				ability1.rigidbody2D.AddRelativeForce(ability1script.velocity * -1 * Vector2.right);
				ability1script.activated = true;
				Ability1Cooldown = Ability1Prefab.cooldown;
			}
			break;
		case 2:
			if(Ability2Cooldown == 0 && _playerState != PlayerState.attacking){
				rigidbody2D.AddForce(Vector2.zero);
				rigidbody2D.AddRelativeForce(Ability2.lungeForce * Vector2.right);
				_playerState = PlayerState.lunging;
				Ability2.isLunging = true;
				Ability2Cooldown = Ability2.cooldown;
				Ability2RecoverTime = Ability2.recoverTime;
			}
			break;
		}
	}

	void LowerCooldowns(){
		if(Ability1Cooldown > 0){
			Ability1Cooldown--;
		}
		if(Ability2Cooldown > 0){
			Ability2Cooldown--;
		}
		if(Ability2RecoverTime > 0){
			Ability2RecoverTime--;
		}
		if(Ability2RecoverTime == 0 && _playerState == PlayerState.lunging) {
			_playerState = PlayerState.standing;
			Ability2.isLunging = false;
			Ability2.resetEnemyIDs();
		}
	}

    void SavePlayerData() {

    }


    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            Vector3 pos = transform.localPosition;
            Quaternion rot = transform.localRotation;
            short ste = (short)_playerState;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            stream.Serialize(ref ste);
        }
        else {
            // Receive latest state information
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);

            _latestCorrectPos = pos;                 // save this to move towards it in FixedUpdate()
            _onUpdatePos = transform.localPosition;  // we interpolate from here to latestCorrectPos
            _latestCorrectRot = rot;
            _onUpdateRot = transform.rotation;
            _lerpFraction = 0;                           // reset the fraction we alreay moved. see Update()


            short ste = 1;

            stream.Serialize(ref ste);
            _playerState = (PlayerState)ste;

        }
    }

    private void SyncedMovement() {
        // We get 10 updates per sec. sometimes a few less or one or two more, depending on variation of lag.
        // Due to that we want to reach the correct position in a little over 100ms. This way, we usually avoid a stop.
        // Lerp() gets a fraction value between 0 and 1. This is how far we went from A to B.
        //
        // Our fraction variable would reach 1 in 100ms if we multiply deltaTime by 10.
        // We want it to take a bit longer, so we multiply with 9 instead.

        _lerpFraction = _lerpFraction + Time.deltaTime * 9;
        transform.localPosition = Vector3.Lerp(_onUpdatePos, _latestCorrectPos, _lerpFraction);    // set our pos between A and B
        transform.rotation = Quaternion.Slerp(_onUpdateRot, _latestCorrectRot, _lerpFraction);

    }

}
