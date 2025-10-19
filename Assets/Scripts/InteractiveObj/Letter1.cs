using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Letter1 : InteractiveObjectBase
{
    public Transform letterUI;
    protected override bool IsInteractionPossible()
    {
        return true;
    }

    protected override void PerformInteraction()
    {
        base.PerformInteraction();
        letterUI.gameObject.SetActive(true);
    }
}
