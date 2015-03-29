using UnityEngine;
using System.Collections;


public delegate float ChangePlayerStat(StatType statType, float amountChange);
public delegate void PlayerAttack(bool isAttacking);
public delegate void GameCreated();
public delegate void GenerateAndRenderMap();
public delegate void MapGenerated(bool isHost);
public delegate void MapRendered(bool isHost);


public static class DelegateHolder {


    public static event ChangePlayerStat OnPlayerStatChange;
    public static event PlayerAttack OnPlayerAttack;
    public static event GameCreated OnGameCreated;
    public static event GenerateAndRenderMap OnGenerateAndRenderMap;
    public static event MapGenerated OnMapGenerated;
    public static event MapRendered OnMapRendered;


    public static void TriggerPlayerStatChange(StatType statType, float amountChange) {
        if (OnPlayerStatChange != null) {
            OnPlayerStatChange(statType, amountChange);
        }
    }

    public static void TriggerPlayerAttack(bool isAttacking) {
        if(OnPlayerAttack != null)
            OnPlayerAttack(isAttacking);
    }

    public static void TriggerGameCreated() {
        if (OnGameCreated != null) {
            Debug.Log("Delegate Called: Trigger Game Created");
            OnGameCreated();
        }
    }

    public static void TriggerGenerateAndRenderMap() {
        if (OnGenerateAndRenderMap != null) {
            Debug.Log("Delegate Called: Trigger Generate Render Map");
            OnGenerateAndRenderMap();
        }
    }

    public static void TriggerMapGenerated(bool isHost) {
        if (OnMapGenerated != null) {
            Debug.Log("Delegate Called: Trigger Map Generated - isHost:" + isHost);
            OnMapGenerated(isHost);
        }
    }

    public static void TriggerMapRendered(bool isHost) {
        if (OnMapRendered != null) {
            Debug.Log("Delegate Called: Trigger Map Rendered");
            OnMapRendered(isHost);
        }
    }

}
