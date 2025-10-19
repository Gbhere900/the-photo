using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Door : InteractiveObjectBase
{
    [FormerlySerializedAs("rotateAxis")]
    [Header("门参数")]
    [SerializeField] private Transform rotatePivot;
    [SerializeField] private float rotateAngle = 90f;
    [SerializeField] private float rotateSpeed = 90f; // 度/秒
    
    private bool isOpen = false;
    private bool isRotating = false;
    private Vector3 closedPosition;
    private Quaternion closedRotation;
    private Vector3 openPosition;
    private Quaternion openRotation;
    
    private Coroutine rotateCoroutine;
    
    protected override void Initialized()
    {
        base.Initialized();
        
        if (rotatePivot == null)
        {
            rotatePivot = transform;
        }
        
        closedPosition = transform.position;
        closedRotation = transform.rotation;
        
        // 计算旋转角度
        Vector3 pivotToDoor = transform.position - rotatePivot.position;
        Vector3 rotatedPivotToDoor = Quaternion.Euler(0, rotateAngle, 0) * pivotToDoor;
        openPosition = rotatePivot.position + rotatedPivotToDoor;
        openRotation = closedRotation * Quaternion.Euler(0, rotateAngle, 0);
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
        rotateCoroutine = StartCoroutine(RotateDoor(openPosition, openRotation));
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
        rotateCoroutine = StartCoroutine(RotateDoor(closedPosition, closedRotation));
        isOpen = false;
    }

    private IEnumerator RotateDoor(Vector3 targetPosition, Quaternion targetRotation)
    {
        isRotating = true;
        
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.localRotation;
        
        float angleDifference = Quaternion.Angle(startRotation, targetRotation);
        float rotationTime = angleDifference / rotateSpeed;
        float elapsedTime = 0f;
        
        // 平滑旋转
        while (elapsedTime < rotationTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / rotationTime);
            
            // 使用球面插值计算旋转
            Quaternion newRotation = Quaternion.Slerp(startRotation, targetRotation, t);
            
            // 计算新位置（绕轴心点旋转）
            Vector3 pivotToDoor = startPosition - rotatePivot.position;
            Vector3 rotatedPivotToDoor = newRotation * Quaternion.Inverse(startRotation) * pivotToDoor;
            Vector3 newPosition = rotatePivot.position + rotatedPivotToDoor;
            
            // 应用新的位置和旋转
            transform.position = newPosition;
            transform.rotation = newRotation;
            
            yield return null;
        }
        
        transform.position = targetPosition;
        transform.rotation = targetRotation;
        isRotating = false;
    }
}
