using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixCar : PositionChangedObj
{
    [SerializeField] private PositionChangedObj screwdriver;
    
    protected override bool IsInteractionPossible()
    {
        if (screwdriver == null)
        {
            return false;
        }
        return screwdriver.IsInteracted();
    }
}
