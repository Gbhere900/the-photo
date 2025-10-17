using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WorldState
{
    Youth,
    Adult,
    Old
}
public class SceneManager : MonoBehaviour
{
    private static SceneManager instance;
   
    [SerializeField] private WorldState worldState = WorldState.Old;

    public Action<WorldState> OnWorldStateChange;

    public static SceneManager Instance()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }


    public WorldState GetCurrentWorldState()
    {
        return worldState;
    }

    public void ChangeWorldState(WorldState worldState)
    {
        this.worldState = worldState;
        OnWorldStateChange.Invoke(worldState);
    }

}
