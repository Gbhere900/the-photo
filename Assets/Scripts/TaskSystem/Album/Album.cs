using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Album :InteractiveObjectBase
{
    [SerializeField] private Transform albumUI;
    protected override bool IsInteractionPossible()
    {
        return true;
    }

    protected override void PerformInteraction()
    {
        albumUI.gameObject.SetActive(true);
    }

    
}
