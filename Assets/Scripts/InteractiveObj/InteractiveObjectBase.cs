using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObjectBase : MonoBehaviour
{
    [Header("交互距离")]
    [SerializeField] protected float interactiveDistance;
    [SerializeField] protected float cooldownTime;
    [SerializeField] protected bool isHighlight = false;

    protected float lastInteractTime;

    protected virtual void Start() { Initialized(); }
    
    protected virtual void Update()
    {
        // 设置是否高亮
        isHighlight = CanInteract;
        // 检测玩家输入
        CheckPlayerInput();
    }
    
    public bool CanInteract
    {
        get
        {
            if (cooldownTime > 0 && Time.time < lastInteractTime + cooldownTime)
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
    protected abstract void Initialized();

    /// <summary>
    /// 检测玩家输入
    /// </summary>
    protected virtual void CheckPlayerInput()
    {
        // F键交互
        if (Input.GetKeyDown(KeyCode.F) && CanInteract)
        {
            Interact();
        }
    }

    /// <summary>
    /// 交互函数
    /// </summary>
    protected virtual void Interact()
    {
        if (!CanInteract)
        {
            Debug.LogError("Can't interact with this object");
            return;
        }
        
        // 执行具体交互逻辑
        PerformInteraction();
        // 记录交互时间
        lastInteractTime = Time.time;
    }
    
    /// <summary>
    /// 具体交互逻辑
    /// </summary>
    protected abstract void PerformInteraction();
}
