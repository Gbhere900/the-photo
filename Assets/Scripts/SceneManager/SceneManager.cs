using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

    [SerializeField] private Image targetImage;
    [SerializeField] private AnimationCurve convertTimeLine;
    private bool isConverting = false;
    private float timer = 0;

    [SerializeField] private Material[] skyBoxMaterial;
    public static SceneManager Instance()
    {

        return instance;
    }
    private void Awake()
    {
        instance = this;

     

    }

    private void OnEnable()
    {
        OnWorldStateChange += BeginCameraConvert;
        OnWorldStateChange += ChangeSkyBox;   
    }

    private void OnDisable()
    {
        OnWorldStateChange -= BeginCameraConvert;
        OnWorldStateChange -= ChangeSkyBox;
    }

    private void ChangeSkyBox(WorldState worldState)
    {
        RenderSettings.skybox = skyBoxMaterial[((int)worldState)];
    }
    private void Update()
    {
        if (isConverting)
        {
            if (timer <= 1)
            {
                timer += Time.deltaTime;
                float value = convertTimeLine.Evaluate(timer);
                Material material = targetImage.material;
                material.SetFloat("_Offset", value);
            }
            else
            {
                EndCameraConvert();
            }
        }
    }
    private void BeginCameraConvert(WorldState worldState)
    {
        isConverting = true;
        timer = 0;
        
       
    }

    private void EndCameraConvert()
    {
        isConverting = false;
        
    }

    public WorldState GetCurrentWorldState()
    {
        return worldState;
    }

    public void ChangeWorldState(WorldState worldState)
    {
        WorldState originWorldState = this.worldState;
        this.worldState = worldState;
        if (worldState != originWorldState)
        {
            OnWorldStateChange.Invoke(worldState);
        }
        
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
        ChangeWorldState(WorldState.Youth);
      
    }

    public void WorldStateChangeToAdult()
    {
        ChangeWorldState(WorldState.Adult);
      
    }
    public void WorldStateChangeToOld()
    {
        ChangeWorldState(WorldState.Old);
      
    }



}
