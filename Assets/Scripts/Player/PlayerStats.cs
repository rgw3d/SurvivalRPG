using UnityEngine;
using System.Collections;

public class PlayerStats: MonoBehaviour {

    public static int LevelUpScoreBase = 100;

    private static string _playerName;
    public static string PlayerName {
        get {
            if (System.String.IsNullOrEmpty(_playerName))
                _playerName = PhotonNetworkManager.selectedPlayerName;
            return _playerName;
        }
    }

    private static int _maxHealth = -1;
    public static int MaxHealth {
        get {
            if (_maxHealth == -1)
                _maxHealth = PlayerPrefs.GetInt(GameControl.PLAYER_MAX_HEALTH_KEY + PlayerName);
            return _maxHealth;
        }
        set {
            PlayerPrefs.SetInt(GameControl.PLAYER_MAX_HEALTH_KEY + PlayerName, value);
        }
    }

    private static int _maxMana = -1;
    public static int MaxMana {
        get {
            if (_maxMana == -1)
                _maxMana = PlayerPrefs.GetInt(GameControl.PLAYER_MAX_MANA_KEY + PlayerName);
            return _maxMana;
        }
        set {
            PlayerPrefs.SetInt(GameControl.PLAYER_MAX_MANA_KEY + PlayerName, value);
        }
    }

    private static int _attackValue = -1;
    public static int AttackValue {
        get {
            if (_attackValue == -1)
                _attackValue = PlayerPrefs.GetInt(GameControl.PLAYER_ATTACK_KEY + PlayerName);
            return _attackValue;
        }
        set {
            PlayerPrefs.SetInt(GameControl.PLAYER_ATTACK_KEY + PlayerName, value);
        }
    }

    private static int _rangedAttackValue = -1;
    public static int RangedAttackValue {
        get {
            if (_rangedAttackValue == -1)
                _rangedAttackValue = PlayerPrefs.GetInt(GameControl.PLAYER_RANGED_ATTACK_KEY + PlayerName);
            return _rangedAttackValue;
        }
        set {
            PlayerPrefs.SetInt(GameControl.PLAYER_RANGED_ATTACK_KEY + PlayerName, value);
        }
    }

    private static int _defenseValue = -1;
    public static int DefenseValue {
        get {
            if (_defenseValue == -1)
                _defenseValue = PlayerPrefs.GetInt(GameControl.PLAYER_DEFENSE_KEY + PlayerName);
            return _defenseValue;
        }
        set {
            PlayerPrefs.SetInt(GameControl.PLAYER_DEFENSE_KEY + PlayerName, value);
        }
    }

    private static float _movementSpeed = -1;
    public static float MovementSpeed {
        get {
            if (_movementSpeed == -1)
                _movementSpeed = PlayerPrefs.GetFloat(GameControl.PLAYER_MOVEMENT_KEY + PlayerName);
            return _movementSpeed;
        }
        set {
            PlayerPrefs.SetFloat(GameControl.PLAYER_MOVEMENT_KEY + PlayerName, value);
        }
    }

    private static CharacterClass _characterClass = CharacterClass.None;
    public static CharacterClass PlayerClass {
        get {
            if (_characterClass == CharacterClass.None)
                _characterClass = (CharacterClass)PlayerPrefs.GetInt(GameControl.PLAYER_CLASS_KEY + PlayerName);
            return _characterClass;
        }
    }

    private static int _playerLevel = -1;
    public static int PlayerLevel {
        get {
            if (_playerLevel == -1)
                _playerLevel = PlayerPrefs.GetInt(GameControl.PLAYER_LEVEL_KEY + PlayerName);
            return _playerLevel;
        }
        set {
            _playerLevel = value;
            PlayerPrefs.SetInt(GameControl.PLAYER_LEVEL_KEY + PlayerName, value);
        }
    }

    private static int _playerScore = -1;
    public static int PlayerScore {
        get {
            if (_playerScore == -1)
                _playerScore = PlayerPrefs.GetInt(GameControl.PLAYER_SCORE_KEY + PlayerName);
            return _playerScore;
        }
        set {
            _playerScore = value;
            if (_playerScore >= CalculateLevelUpXP()) {
                _playerScore -= CalculateLevelUpXP();
                PlayerLevel++;
            }
        }
    }

    private static float _playerHealth = -1;
    public static float PlayerHealth {
        get {
            if (_playerHealth == -1)
                _playerHealth = MaxHealth;
            return _playerHealth;
        }
        set {
            _playerHealth += value;
            if (_playerHealth <= 0)
                print("YOU DEAD SON");
            //figure out what to do when dead
        }
    }

    public static void SavePlayerScore() {
        PlayerPrefs.SetInt(GameControl.PLAYER_SCORE_KEY + PhotonNetworkManager.selectedPlayerName, PlayerScore);
    }

    public static int CalculateLevelUpXP() {
        return LevelUpScoreBase * PlayerLevel;
    }

    public enum CharacterClass {
        None = 0,
        Fighter = 1,
        Mage = 2,
        Healer = 3,
        Shrek = 4
    }


    

    
    

}