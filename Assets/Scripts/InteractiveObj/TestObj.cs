using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObj : InteractiveObjectBase
{
    Renderer renderer;
    [SerializeField] private Material material1;
    [SerializeField] private Material material2;

    protected override void Initialized()
    {
        base.Initialized();
        
        renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material1;
        }
    }

    protected override bool IsInteractionPossible()
    {
        return true;
    }

    protected override void PerformInteraction()
    {
        if (renderer == null)
        {
            return;
        }
        if (renderer.material == material1)
        {
            renderer.material = material2;
        }
        else
        {
            renderer.material = material1;
        }
    }
}
