using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : InteractiveObjectBase
{
    [Header("抽屉参数")]
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private float moveDistance;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject letter;
    
    private bool isMoving = false;
    private bool isOpen = false;
    private Vector3 closePosition;
    private Vector3 openPosition;
    
    private Coroutine moveCoroutine;

    protected override void Initialized()
    {
        base.Initialized();
        closePosition = transform.position;
        openPosition = closePosition + moveDirection.normalized * moveDistance;
    }

    protected override bool IsInteractionPossible()
    {
        return !isMoving;
    }

    protected override void PerformInteraction()
    {
        base.PerformInteraction();
        if (isMoving)
        {
            return;
        }

        if (isOpen)
        {
            CloseDrawer();
        }
        else
        {
            OpenDrawer();
        }
    }

    private void CloseDrawer()
    {
        if (!isOpen || isMoving)
        {
            return;
        }
        // 将信重新设为子对象
        if (letter)
        {
            letter.transform.parent = transform;   
        }
        
        // 停止之前的移动协程
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        // 开始移动协程
        moveCoroutine = StartCoroutine(MoveDrawer(closePosition));
        isOpen = false;
    }

    private void OpenDrawer()
    {
        if (isOpen || isMoving)
        {
            return;
        }
        
        // 停止之前的移动协程
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        // 开始移动协程
        moveCoroutine = StartCoroutine(MoveDrawer(openPosition));
        isOpen = true;
    }

    private IEnumerator MoveDrawer(Vector3 targetPosition)
    {
        isMoving = true;
        
        Vector3 startPosition = transform.position;
        float length = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;
        
        // 移动抽屉
        while (transform.position != targetPosition)
        {
            // 计算已移动距离和已移动距离占总移动距离的占比
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / length;
            // 使用Lerp平滑移动
            transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
            yield return null;
        }
        
        // 确保最终位置准确
        transform.position = targetPosition;
        isMoving = false;
        // 若最终是打开状态
        if (targetPosition == openPosition)
        {
            // 解除信和抽屉的父子关系
            if (letter)
            {
                letter.transform.parent = null;   
            }
        }
    }
}
