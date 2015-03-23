using UnityEngine;
using System.Collections;


public delegate float ChangePlayerStat(StatType statType, float amountChange);
public delegate void PlayerAttack(bool isAttacking);
public delegate void GenerateAndRenderMap();
public delegate void MapGenerated(bool isHost);
public delegate void MapRendered(bool isHost);


public static class DelegateHolder {


    public static event ChangePlayerStat OnPlayerStatChange;
    public static event PlayerAttack OnPlayerAttack;
    public static event GenerateAndRenderMap OnGenerateAndRenderMap;
    public static event MapGenerated OnMapGenerated;
    public static event MapRendered OnMapRendered;


    public static void TriggerPlayerStatChange(StatType statType, float amountChange) {
        if(OnPlayerStatChange != null)
            OnPlayerStatChange(statType, amountChange);
    }

    public static void TriggerPlayerAttack(bool isAttacking) {
        if(OnPlayerAttack != null)
            OnPlayerAttack(isAttacking);
    }

    public static void TriggerGenerateAndRenderMap() {
        if (OnGenerateAndRenderMap != null) {
            Debug.Log("Triggerd Generate And Render Map");
            OnGenerateAndRenderMap();
        }
    }

    public static void TriggerMapGenerated(bool isHost) {
        if(OnMapGenerated != null)
            OnMapGenerated(isHost);
    }

    public static void TriggerMapRendered(bool isHost) {
        if(OnMapRendered != null)    
            OnMapRendered(isHost);
    }

}
