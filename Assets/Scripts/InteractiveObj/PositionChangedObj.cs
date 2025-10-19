using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionChangedObj : InteractiveObjectBase
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private GameObject otherObj;
    private bool isInteracted = false;
    
    protected override void Initialized()
    {
        base.Initialized();
        isInteracted = false;
        if (otherObj != null)
        {
            otherObj.SetActive(false);
        }
    }

    protected override bool IsInteractionPossible()
    {
        return !isInteracted;
    }

    protected override void PerformInteraction()
    {
        base.PerformInteraction();
        if (isInteracted)
        {
            return;
        }
        isInteracted = true;
        if (otherObj == null)
        {
            this.gameObject.transform.position = targetTransform.position;
            this.gameObject.transform.rotation = targetTransform.rotation;   
        }
        else
        {
            otherObj.gameObject.SetActive(true);
            if (InteractiveTooltip.Instance.GetDescriptionText() == name)
            {
                InteractiveTooltip.Instance.HideTooltip();
            }
            this.gameObject.SetActive(false);
        }
    }

    public bool IsInteracted()
    {
        return isInteracted;
    }
}
