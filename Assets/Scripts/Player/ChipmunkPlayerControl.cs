using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class ChipmunkPlayerControl : Photon.MonoBehaviour{
    public Sprite NormalSprite;
    public Sprite AttackSprite;
	private SpriteRenderer _spriteRenderer;
    private PlayerState _playerState = PlayerState.Standing;

    public float ChargedSpeedMult;
    public float RotationSpeed = 5;

    public KeyCode UpKey;
    public KeyCode DownKey;
    public KeyCode LeftKey;
    public KeyCode RightKey;
    public KeyCode AttackKey;    

	public Chipmunk1AcornSpit Ability1Prefab;
	private GameObject _ability1GameObject;
	private Chipmunk1AcornSpit _ability1Script;
	private int _ability1Cooldown = 0;

	public Chipmunk2Lunge Ability2;
    public int Ability2CooldownValue = 15;
	private int _ability2Cooldown = 0;

    public int AttackCooldownValue = 15;
    private int _attackCooldown = 0;
	private int _chargedValue = 0;

    private Vector3 _latestCorrectPos;
    private Vector3 _onUpdatePos;
    private Quaternion _latestCorrectRot;
    private Quaternion _onUpdateRot;
    private float _lerpFraction;

    
	// Use this for initialization
	void Start () {
        _latestCorrectPos = transform.position;
        _onUpdatePos = transform.position;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = NormalSprite;

        _ability1GameObject = PhotonNetwork.Instantiate(Ability1Prefab.name, new Vector2(-100, -100), Quaternion.identity, 0);
        _ability1Script = _ability1GameObject.GetComponent("Chipmunk1AcornSpit") as Chipmunk1AcornSpit;
	}
	
	private enum PlayerState {
		Attacking = 0,
		Walking = 1,
		Standing = 2,
		Lunging = 3,
		Charging = 4
	}
	
    void Update() {
        if (photonView.isMine) { //point at camera
            Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
            Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
            float angle = Mathf.Atan2(positionOnScreen.y - mouseOnScreen.y, positionOnScreen.x - mouseOnScreen.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, angle)), RotationSpeed * Time.deltaTime);
            Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

    }

	void FixedUpdate () {
        if (photonView.isMine) {
            if (ChatDisplay.ChatState == ChatDisplay.ChattingState.ChatClosedButShowing // chat is closed then do normal stuff
                || ChatDisplay.ChatState == ChatDisplay.ChattingState.NoUsername) {
                if (_playerState != PlayerState.Attacking && _playerState != PlayerState.Lunging) //only update movement if not attacking
                    PlayerMovement();
                if (Input.GetKey(KeyCode.E)) {
                    PlayerStats.PlayerScore++;
                }
                if (Input.GetKey(KeyCode.Q)) {
                    PlayerStats.PlayerHealth--;
                }
                if (Input.GetKey(KeyCode.Z)) {
                    PlayerStats.PlayerHealth++;
                }
                if (Input.GetKey(KeyCode.Alpha1)) {
                    PlayerAbility(1);
                }
                if (Input.GetKey(KeyCode.Alpha2)) {
                    PlayerAbility(2);
                }
                PlayerAttack();
            }
            
			LowerCooldowns();
        }
        else {
            SyncedMovement();
        }

        PlayerSprite();
	}

    public void PlayerMovement() {
        if (Input.GetKey(UpKey)) {
			if(_playerState == PlayerState.Charging){
                rigidbody2D.AddForce(Vector2.up * PlayerStats.MovementSpeed * ChargedSpeedMult);
			}
			else{
				rigidbody2D.AddForce(Vector2.up * PlayerStats.MovementSpeed);
				_playerState = PlayerState.Walking;
			}
        }
        if (Input.GetKey(DownKey)) {
			if(_playerState == PlayerState.Charging){
                rigidbody2D.AddForce(Vector2.up * -1 * PlayerStats.MovementSpeed * ChargedSpeedMult);
			}
			else{
                rigidbody2D.AddForce(Vector2.up * -1 * PlayerStats.MovementSpeed);
				_playerState = PlayerState.Walking;
			}
        }
        if (Input.GetKey(LeftKey)) {
			if(_playerState == PlayerState.Charging){
                rigidbody2D.AddForce(Vector2.right * -1 * PlayerStats.MovementSpeed * ChargedSpeedMult);
			}
			else{
                rigidbody2D.AddForce(Vector2.right * -1 * PlayerStats.MovementSpeed);
				_playerState = PlayerState.Walking;
			}

        }
        if (Input.GetKey(RightKey)) {
			if(_playerState == PlayerState.Charging){
                rigidbody2D.AddForce(Vector2.right * PlayerStats.MovementSpeed * ChargedSpeedMult);
			}
			else{
                rigidbody2D.AddForce(Vector2.right * PlayerStats.MovementSpeed);
				_playerState = PlayerState.Walking;
			}
        }
    }


    public void PlayerSprite() {
            if (_playerState == PlayerState.Attacking)
                _spriteRenderer.sprite = AttackSprite;
            else
                _spriteRenderer.sprite = NormalSprite;
    }

    public void PlayerAttack() {
		if (_attackCooldown == 0 && Input.GetKeyDown(AttackKey) && (_playerState == PlayerState.Standing || _playerState == PlayerState.Walking)) {
			_chargedValue = 0;
            DelegateHolder.TriggerPlayerAttack(true, 20);
            _attackCooldown = AttackCooldownValue;
            _playerState = PlayerState.Attacking;
        }
        if(_attackCooldown > 0){
            _attackCooldown--;
        }
        
        if (_attackCooldown == 0 && _playerState == PlayerState.Attacking) {
			DelegateHolder.TriggerPlayerAttack(false, 0);
			if(Input.GetKey(AttackKey)){
				_playerState = PlayerState.Charging;
			}
			else{
				_playerState = PlayerState.Standing;
			}

        }

		if(_playerState == PlayerState.Charging && Input.GetKey(AttackKey)){
			if(_chargedValue < 120){
				_chargedValue++;
			}
		} 

		if(_playerState == PlayerState.Charging && Input.GetKeyUp(AttackKey)){
			DelegateHolder.TriggerPlayerAttack(true, 20 + (_chargedValue / 4));
			_attackCooldown = AttackCooldownValue;
			_playerState = PlayerState.Attacking;
			_chargedValue = 0;
		}
    }

	void PlayerAbility(int abilityNumber){
		switch(abilityNumber){
		case 1:
			if(_ability1Cooldown == 0 && _playerState != PlayerState.Attacking){
				_ability1GameObject.transform.position = transform.position;
				_ability1GameObject.transform.rotation = transform.rotation;
				_ability1GameObject.rigidbody2D.velocity = Vector3.zero;
				_ability1GameObject.rigidbody2D.AddRelativeForce(_ability1Script.velocity * -1 * Vector2.right);
				_ability1Script.activated = true;
				_ability1Cooldown = Ability1Prefab.cooldown;
			}
			break;
		case 2:
			if(_ability2Cooldown == 0 && _playerState != PlayerState.Attacking){
				rigidbody2D.AddForce(Vector2.zero);
				rigidbody2D.AddRelativeForce(Ability2.lungeForce * Vector2.right);
				_playerState = PlayerState.Lunging;
				Ability2.isLunging = true;
				_ability2Cooldown = Ability2.cooldown;
				Ability2CooldownValue = Ability2.recoverTime;
			}
			break;
		}
	}

	void LowerCooldowns(){
		if(_ability1Cooldown > 0){
			_ability1Cooldown--;
		}
		if(_ability2Cooldown > 0){
			_ability2Cooldown--;
		}
		if(Ability2CooldownValue > 0){
			Ability2CooldownValue--;
		}
		if(Ability2CooldownValue == 0 && _playerState == PlayerState.Lunging) {
			_playerState = PlayerState.Standing;
			Ability2.isLunging = false;
			Ability2.resetEnemyIDs();
		}
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
