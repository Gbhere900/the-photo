using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterUI : MonoBehaviour
{
    [SerializeField] private Animator animator;


    private void OnEnable()
    {
        animator.Play("Show");
        SetCursorState(true);
    }


    private void SetCursorState(bool locked)
    {
        if (locked)
        {

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }



}
