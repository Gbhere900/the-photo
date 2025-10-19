using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Door : InteractiveObjectBase
{
    [FormerlySerializedAs("rotateAxis")]
    [Header("门参数")]
    //[SerializeField] private Transform rotatePivot;
    [SerializeField] private Vector3 rotateAxis = Vector3.up; // 旋转轴（本地坐标系）
    [SerializeField] private float rotateAngle = 90f;
    [SerializeField] private float rotateSpeed = 90f; // 度/秒
    
    private bool isOpen = false;
    private bool isRotating = false;
    //private Vector3 closedPosition;
    private Quaternion closedRotation;
    //private Vector3 openPosition;
    private Quaternion openRotation;
    
    private Coroutine rotateCoroutine;
    
    protected override void Initialized()
    {
        base.Initialized();
        
        // 计算旋转角度
        closedRotation = transform.localRotation;
        openRotation = closedRotation * Quaternion.AngleAxis(rotateAngle, rotateAxis);
        
        /*Vector3 pivotToDoor = transform.position - rotatePivot.position;
        Vector3 rotatedPivotToDoor = Quaternion.Euler(0, rotateAngle, 0) * pivotToDoor;
        openPosition = rotatePivot.position + rotatedPivotToDoor;
        openRotation = closedRotation * Quaternion.Euler(0, rotateAngle, 0);*/
    }

    protected override bool IsInteractionPossible()
    {
        return !isRotating;
    }

    protected override void PerformInteraction()
    {
        base.PerformInteraction();
        if (isRotating)
        {
            return;
        }

        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        if (isOpen)
        {
            return;
        }

        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
        }
        rotateCoroutine = StartCoroutine(RotateDoor(openRotation));
        isOpen = true;
    }

    private void CloseDoor()
    {
        if (!isOpen)
        {
            return;
        }

        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
        }
        rotateCoroutine = StartCoroutine(RotateDoor(closedRotation));
        isOpen = false;
    }

    private IEnumerator RotateDoor(Quaternion targetRotation)
    {
        isRotating = true;
        
        Quaternion startRotation = transform.localRotation;
        float angleDifference = Quaternion.Angle(startRotation, targetRotation);
        float rotationTime = angleDifference / rotateSpeed;
        float elapsedTime = 0f;
        
        // 平滑旋转
        while (elapsedTime < rotationTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / rotationTime);
            
            // 使用Slerp进行球面插值，适合旋转
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);
            
            yield return null;
        }
        
        transform.localRotation = targetRotation;
        isRotating = false;
    }
}
