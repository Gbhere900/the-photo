using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeableItem : MonoBehaviour
{
    [SerializeField] private WorldState worldState;
    private void OnEnable()
    {
        SceneManager.Instance().OnWorldStateChange += SceneManager_OnWorldStateChange;
        if (SceneManager.Instance().GetCurrentWorldState() != worldState)
        {
            gameObject.SetActive(false);
        }
    }


    private void OnDisable()
    {
        
    }
    private void OnDestroy()
    {
        SceneManager.Instance().OnWorldStateChange = SceneManager_OnWorldStateChange;
    }


    virtual protected void SceneManager_OnWorldStateChange(WorldState worldState)
    {
        if (worldState != this.worldState)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
        
    }
}
