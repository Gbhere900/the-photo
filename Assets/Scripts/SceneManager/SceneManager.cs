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
    [SerializeField] private AudioClip buttonClickedAudioClip;
    
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


    public void OnButtonYouthClicked()
    {
        WorldStateChangeToYouth();
        //TODO:播放音效
    }

    public void OnButtonAdultClicked()
    {
        WorldStateChangeToAdult();
        //播放音效
    }

    public void OnButtonOldClicked()
    {
        WorldStateChangeToOld();
        //播放音效
    }
    public void WorldStateChangeToYouth()
    {
        worldState = WorldState.Youth;
        OnWorldStateChange.Invoke(WorldState.Youth);
    }

    public void WorldStateChangeToAdult()
    {
        worldState = WorldState.Adult;
        OnWorldStateChange.Invoke(WorldState.Adult);
    }
    public void WorldStateChangeToOld()
    {
        worldState = WorldState.Old;
        OnWorldStateChange.Invoke(WorldState.Old);
    }



}
