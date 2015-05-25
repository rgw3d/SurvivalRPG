using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class ChipmunkPlayerControl : Photon.MonoBehaviour{
    public Sprite NormalSprite;
    public Sprite AttackSprite;
	private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private PlayerState _playerState = PlayerState.Standing;

    public float ChargedSpeedMult;
    public float RotationSpeed = 5;

    public KeyCode UpKey;
    public KeyCode DownKey;
    public KeyCode LeftKey;
    public KeyCode RightKey;
    public KeyCode AttackKey;
    public KeyCode Ability1Key;
    public KeyCode Ability2Key;
	public KeyCode ChargeAttackKey;

    /*
     *Explanation of cooldowns -- there are two types
     *there is the cooldown between times you can do an abiliy
     *and there are cooldowns for how long the abiliy lasts
     *For the basic attack, there is a cooldown on how often you can jam the button
     *For the lunge ability, there is a cooldown on how long the actual lunge lasts
     *  and how often you can press the button 
     *  
     * confusing
     * 
     */

	public Chipmunk1AcornSpit Ability1Prefab;
	private GameObject _ability1GameObject;
	private Chipmunk1AcornSpit _ability1Script;
    public static int Ability1CooldownValue = 0;
    public static int _ability1Cooldown = 0;

	public Chipmunk2Lunge Ability2;
    public static int Ability2CooldownValue = 15;
	public static int _ability2Cooldown = 0;

    public int AttackCooldownValue = 15;
    private int _attackCooldown = 0;

    public static int MaxChargeTime = 120;
	public static int _chargedValue = 0;

    private Vector3 _latestCorrectPos;
    private Vector3 _onUpdatePos;
    private Quaternion _latestCorrectRot;
    private Quaternion _onUpdateRot;
    private float _lerpFraction;
		
	private enum PlayerState {
		Attacking = 0,
		Walking = 1,
		Standing = 2,
		Lunging = 3,
		Charging = 4
	}

	void Start () {
        _latestCorrectPos = transform.position;
        _onUpdatePos = transform.position;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = NormalSprite;

        _ability1GameObject = PhotonNetwork.Instantiate(Ability1Prefab.name, new Vector2(-100, -100), Quaternion.identity, 0);
        _ability1Script = _ability1GameObject.GetComponent("Chipmunk1AcornSpit") as Chipmunk1AcornSpit;
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
                
                /*if (Input.GetKey(Ability1Key)) {
                    PlayerAbility(1);
                }
                if (Input.GetKey(Ability2Key)) {
                    PlayerAbility(2);
                }
                 * */
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
        if (Input.GetKeyDown(Ability1Key)) {
            PlayerAbility(1);
        }
        if (Input.GetKeyDown(Ability2Key)) {
            PlayerAbility(2);
        }

		if (PlayerStats.AttackCooldown == 0 && Input.GetKeyDown(AttackKey) && (_playerState == PlayerState.Standing || _playerState == PlayerState.Walking)) {
            DelegateHolder.TriggerPlayerAttack(true, PlayerStats.AttackValue); 
            PlayerStats.PowerAttackCharge = 0;
            PlayerStats.AttackCooldown = PlayerStats.AttackCooldownReset;
            _playerState = PlayerState.Attacking;
        }

        if (PlayerStats.AttackCooldown == 0 && _playerState == PlayerState.Attacking) {
			DelegateHolder.TriggerPlayerAttack(false, 0);
			/*if(Input.GetKey(AttackKey)){
				_playerState = PlayerState.Charging;
			}*/

			_playerState = PlayerState.Standing;
        }

		if(Input.GetKey(ChargeAttackKey) && (_playerState == PlayerState.Standing || _playerState == PlayerState.Walking)){
			_playerState = PlayerState.Charging;
		}

		if(_playerState == PlayerState.Charging && Input.GetKeyUp(ChargeAttackKey)){
			DelegateHolder.TriggerPlayerAttack(true, PlayerStats.AttackValue + (PlayerStats.PowerAttackCharge / 2));
            PlayerStats.AttackCooldown = PlayerStats.AttackCooldownReset;
            _playerState = PlayerState.Standing;
            PlayerStats.PowerAttackCharge = 0;
		}

        if (PlayerStats.Ability2Cooldown == 0 && _playerState == PlayerState.Lunging) {
            _playerState = PlayerState.Standing;
            Ability2.isLunging = false;
            Ability2.resetEnemyIDs();
        }
    }

	void PlayerAbility(int abilityNumber){
		switch(abilityNumber){
		case 1:
			if(PlayerStats.Ability1Cooldown == 0 && _playerState != PlayerState.Attacking){
				_ability1GameObject.transform.position = transform.position;
				_ability1GameObject.transform.rotation = transform.rotation;
				_ability1GameObject.rigidbody2D.velocity = Vector3.zero;
				_ability1GameObject.rigidbody2D.AddRelativeForce(_ability1Script.velocity * -1 * Vector2.right);
				_ability1Script.activated = true;
                PlayerStats.Ability1Cooldown = PlayerStats.Ability1CooldownReset;
			}
			break;
		case 2:
			if(PlayerStats.Ability2Cooldown == 0 && _playerState != PlayerState.Attacking){
				rigidbody2D.AddForce(Vector2.zero);
				rigidbody2D.AddRelativeForce(Ability2.lungeForce * Vector2.right);
				_playerState = PlayerState.Lunging;
				Ability2.isLunging = true;
				PlayerStats.Ability2Cooldown = PlayerStats.Ability2CooldownReset;
				Ability2CooldownValue = Ability2.recoverTime;
			}
			break;
		}
	}

	void LowerCooldowns(){
        if (_attackCooldown > 0) {
            _attackCooldown--;
        }
        if (_playerState == PlayerState.Charging && Input.GetKey(ChargeAttackKey)) {
            if (PlayerStats.PowerAttackCharge < PlayerStats.PowerAttackMaxValue) {
                PlayerStats.PowerAttackCharge++;
            }
        } 
		if(PlayerStats.Ability1Cooldown > 0){
			PlayerStats.Ability1Cooldown--;
		}
        if (PlayerStats.Ability2Cooldown > 0) {
            if (rigidbody2D.velocity == Vector2.zero && _playerState == PlayerState.Lunging)
                _playerState = PlayerState.Standing;
            PlayerStats.Ability2Cooldown--;
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
