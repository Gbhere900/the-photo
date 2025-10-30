using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : InteractiveObjectBase
{
    [Header("信参数")]
    //[SerializeField] private Transform targetTransform;
    [SerializeField] private GameObject letterPaper;
    private bool isOpen = false;

    protected override void Initialized()
    {
        base.Initialized();
        isOpen = false;
        letterPaper.SetActive(false);
    }

    protected override bool IsInteractionPossible()
    {
        return !isOpen;
    }

    protected override void PerformInteraction()
    {
        base.PerformInteraction();
        if (isOpen)
        {
            return;
        }
        isOpen = true;
        letterPaper.SetActive(true);
        //GameObject.Instantiate(letterPaper, transform.position, Quaternion.identity);
    }
}
