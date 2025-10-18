using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionChangedObj : InteractiveObjectBase
{
    [SerializeField] private Transform targetTransform;

    private bool isInteracted = false;
    
    protected override void Initialized()
    {
        base.Initialized();
        isInteracted = false;
    }

    protected override bool IsInteractionPossible()
    {
        return !isInteracted;
    }

    protected override void PerformInteraction()
    {
        if (isInteracted)
        {
            return;
        }
        isInteracted = true;
        this.gameObject.transform.position = targetTransform.position;
    }

    public bool IsInteracted()
    {
        return isInteracted;
    }
}
