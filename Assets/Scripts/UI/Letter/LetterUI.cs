using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterUI : MonoBehaviour
{
    [SerializeField] private Animator animator;


    private void OnEnable()
    {
        animator.Play("Show");
    }

    

}
