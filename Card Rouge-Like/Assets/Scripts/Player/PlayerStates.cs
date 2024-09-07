using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{

    public static PlayerStates instance;
    public PlayerState currentState;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    public void SetState(PlayerState inState)
    {
        currentState = inState;
    }

    public PlayerState GetState()
    {
        return currentState; 
    }
}

public enum PlayerState
{
    Frozen,
    Idle,
    Running,
    Attack,
    TakingDamage,
    Dead
}