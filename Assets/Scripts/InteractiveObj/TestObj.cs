using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObj : InteractiveObjectBase
{
    private Renderer renderer;
    private bool isMaterial1;
    [SerializeField] private Material material1;
    [SerializeField] private Material material2;

    protected override void Initialized()
    {
        base.Initialized();
        
        renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material1;
            isMaterial1 = true;
        }
        else
        {
            isMaterial1 = false;
        }
    }

    protected override bool IsInteractionPossible()
    {
        return true;
    }

    protected override void PerformInteraction()
    {
        Debug.Log("PerformInteraction");
        if (renderer == null)
        {
            return;
        }
        if (isMaterial1)
        {
            Debug.Log("1");
            renderer.material = material2;
            isMaterial1 = false;
        }
        else
        {
            Debug.Log("2");
            renderer.material = material1;
            isMaterial1 = true;
        }
    }
}
