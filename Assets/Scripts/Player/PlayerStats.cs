using UnityEngine;
using System.Collections;

public class PlayerStats: MonoBehaviour {

    private static string _playerName;
    private static int _maxHealth = -1;
    private static int _maxMana = -1;
    private static int _attackValue = -1;
    private static int _rangedAttackValue = -1;
    private static int _defenseValue = -1;
    private static float _movementSpeed = -1;
    private static CharacterClass _characterClass = CharacterClass.None;
    private static int _playerLevel = -1;
    private static int _playerScore = -1;

    public enum CharacterClass {
        None = 0,
        Fighter = 1,
        Mage = 2,
        Healer = 3,
        Shrek = 4
    }

    /* Get Methods */
    public static string GetPlayerName(){
        if (System.String.IsNullOrEmpty(_playerName)) 
            _playerName = PhotonNetworkManager.selectedPlayerName;
        return _playerName;
    }

    public static int GetMaxHealth(){
        if(_maxHealth == -1)
            _maxHealth = PlayerPrefs.GetInt(GameControl.PLAYER_MAX_HEALTH_KEY + PhotonNetworkManager.selectedPlayerName);
        return _maxHealth;
    }

    public static int GetMaxMana(){
        if(_maxMana == -1)
            _maxMana = PlayerPrefs.GetInt(GameControl.PLAYER_MAX_MANA_KEY + PhotonNetworkManager.selectedPlayerName);
        return _maxMana;
    }

    public static int GetBaseAttackValue(){
        if(_attackValue == -1)
            _attackValue = PlayerPrefs.GetInt(GameControl.PLAYER_ATTACK_KEY + PhotonNetworkManager.selectedPlayerName);
        return _attackValue;
    }

    public static int GetBaseRangedAttackValue() {
        if(_rangedAttackValue == -1)
            _rangedAttackValue = PlayerPrefs.GetInt(GameControl.PLAYER_RANGED_ATTACK_KEY + PhotonNetworkManager.selectedPlayerName);
        return _rangedAttackValue;
    }

    public static int GetBaseDefenseValue(){
        if(_defenseValue == -1)
            _defenseValue = PlayerPrefs.GetInt(GameControl.PLAYER_DEFENSE_KEY + PhotonNetworkManager.selectedPlayerName);
        return _defenseValue;
    }

    public static float GetBaseMovementSpeed(){
        if(_movementSpeed == -1)
            _movementSpeed = PlayerPrefs.GetFloat(GameControl.PLAYER_MOVEMENT_KEY + PhotonNetworkManager.selectedPlayerName);
        return _movementSpeed;
    }

    public static CharacterClass GetPlayerClass(){
        if(_characterClass == CharacterClass.None)
            _characterClass = (CharacterClass)PlayerPrefs.GetInt(GameControl.PLAYER_CLASS_KEY + PhotonNetworkManager.selectedPlayerName);
        return _characterClass;
    }

    public static int GetPlayerLevel() {
        if(_playerLevel == -1)
            _playerLevel = PlayerPrefs.GetInt(GameControl.PLAYER_LEVEL_KEY + PhotonNetworkManager.selectedPlayerName);
        return _playerLevel;
    }

    public static int GetPlayerScore() {
        if (_playerScore == -1) 
            _playerScore = PlayerPrefs.GetInt(GameControl.PLAYER_SCORE_KEY + PhotonNetworkManager.selectedPlayerName);
        return _playerScore;
    }

   
    /* Set Methods */
    public static void SetMaxHealth(int maxHealth) {
        PlayerPrefs.SetInt(GameControl.PLAYER_MAX_HEALTH_KEY + PhotonNetworkManager.selectedPlayerName,maxHealth);
    }

    public static void SetMaxMana(int maxMana) {
        PlayerPrefs.SetInt(GameControl.PLAYER_MAX_MANA_KEY + PhotonNetworkManager.selectedPlayerName, maxMana);
    }

    public static void SetBaseAttackValue(int baseAttack) {
        PlayerPrefs.SetInt(GameControl.PLAYER_ATTACK_KEY + PhotonNetworkManager.selectedPlayerName, baseAttack);
    }

    public static void SetBaseRangedAttackValue(int baseRanged) {
        PlayerPrefs.SetInt(GameControl.PLAYER_RANGED_ATTACK_KEY + PhotonNetworkManager.selectedPlayerName, baseRanged);
    }

    public static void SetBaseDefenseValue(int baseDefense) {
        PlayerPrefs.SetInt(GameControl.PLAYER_DEFENSE_KEY + PhotonNetworkManager.selectedPlayerName, baseDefense);
    }

    public static void SetPlayerLevel(int level) {
        PlayerPrefs.SetInt(GameControl.PLAYER_LEVEL_KEY + PhotonNetworkManager.selectedPlayerName, level);
    }

    public static void SetPlayerScore(int score) {
        PlayerPrefs.SetInt(GameControl.PLAYER_SCORE_KEY + PhotonNetworkManager.selectedPlayerName, score);
    }
    

}
