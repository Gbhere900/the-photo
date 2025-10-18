using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(QuickOutline))]
public abstract class InteractiveObjectBase : MonoBehaviour
{
    [Header("交互参数")]
    //[SerializeField] protected float interactiveDistance;
    [SerializeField] protected float cooldownTime;
    [SerializeField] protected bool isHighlight = false;
    [SerializeField] protected float outlineWidth = 10f;

    private bool isIntersectingWithDetector = false;
    protected float lastInteractTime;
    protected GameObject player;

    private QuickOutline outline;
    
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        Initialized();
    }
    
    protected virtual void Update()
    {
        // 高亮逻辑
        isHighlight = CanInteract;
        if (outline)
        {
            if (isHighlight)
            {
                outline.enabled = true;
            }
            else
            {
                outline.enabled = false;
            }
        }
        
        // 检测玩家输入
        CheckPlayerInput();
    }
    
    public bool CanInteract
    {
        get
        {
            // 交互冷却时间判断
            if (cooldownTime > 0 && Time.time < lastInteractTime + cooldownTime)
            {
                return false;
            }
            // 交互距离判断
            if (!player || !isIntersectingWithDetector)
            {
                return false;
            }
            return IsInteractionPossible();
        }
    }

    /// <summary>
    /// 判断交互是否可行
    /// </summary>
    /// <returns></returns>
    protected abstract bool IsInteractionPossible();

    /// <summary>
    /// 初始化
    /// </summary>
    protected virtual void Initialized()
    {
        lastInteractTime = -cooldownTime;
        
        outline = GetComponent<QuickOutline>();
        if (outline)
        {
            outline.OutlineColor = Color.white;
            outline.OutlineWidth = outlineWidth;
            outline.OutlineMode = QuickOutline.Mode.OutlineVisible;
        }
    }

    /// <summary>
    /// 检测玩家输入
    /// </summary>
    private void CheckPlayerInput()
    {
        // F键交互
        if (Input.GetKeyDown(KeyCode.I) && CanInteract)
        {
            Interact();
        }
    }

    /// <summary>
    /// 交互函数
    /// </summary>
    private void Interact()
    {
        if (!CanInteract)
        {
            Debug.LogError("Can't interact with this object");
            return;
        }
        
        // Todo: 交互UI提示逻辑
        
        // 执行具体交互逻辑
        PerformInteraction();
        // 记录交互时间
        lastInteractTime = Time.time;
    }
    
    /// <summary>
    /// 具体交互逻辑
    /// </summary>
    protected abstract void PerformInteraction();

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "InteractiveDetect")
        {
            isIntersectingWithDetector = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "InteractiveDetect")
        {
            isIntersectingWithDetector = false;
        }
    }
}
