using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// 任务管理类单例
public class TaskSystemManager : SingletonMonoBase<TaskSystemManager>
{
    [Header("任务队列")]
    [SerializeField] private List<Task> taskList = new List<Task>();

    [Header("信封任务")]
    [SerializeField] private string letterEventId;
    private bool isLetterEventTriggered = false;
    [SerializeField] private Transform LetterUI;

    [Header("老人任务")]
    [SerializeField] private string oldManEventId;
    private bool isOldManEventTriggered = false;
    [SerializeField] private Transform oldMan;
    [SerializeField] private Transform AlbumUI;
    
    [Header("当前任务")]
    [SerializeField] private Task currentTask;
    private int currentTaskIndex;


    protected override void Awake()
    {
        base.Awake();
        Initialized();
    }
    
    /// <summary>
    /// 初始化函数
    /// </summary>
    private void Initialized()
    {
        if (taskList.Count == 0)
        {
            return;
        }
        currentTaskIndex = 0;
        currentTask = taskList[currentTaskIndex];
        // 将所有任务状态重置为待接取
        foreach (Task task in taskList)
        {
            task.SetTaskType(Task.TaskStatus.Pending);
        }
    }

    //--------------------------公共接口--------------------------
    /// <summary>
    /// 重置任务为待接取状态
    /// </summary>
    public void ResetCurrentTask()
    {
        if (currentTask == null || currentTaskIndex >= taskList.Count)
        {
            Debug.LogError("Current task is null");
            return;
        }
        currentTask.SetTaskType(Task.TaskStatus.Pending);
    }
    
    /// <summary>
    /// 接取当前任务
    /// </summary>
    public void AcceptCurrentTask()
    {
        if (currentTask == null || currentTaskIndex >= taskList.Count)
        {
            Debug.LogError("Task index out of range");
            return;
        }
        currentTask.SetTaskType(Task.TaskStatus.InProgress);
    }
    
    /// <summary>
    /// 将当前任务设为完成，并累加任务索引
    /// </summary>
    public void SetCurrentTaskCompleted()
    {
        if (currentTask == null || currentTaskIndex >= taskList.Count)
        {
            Debug.LogError("Current task is null");
            return;
        }
        currentTask.SetTaskType(Task.TaskStatus.Completed);
        Debug.Log(string.Format("任务:{{%d}} 完成!", currentTaskIndex));
        currentTask = null;
        
        // 累加任务索引，指向下一个任务
        currentTaskIndex++;
        if (currentTaskIndex >= taskList.Count)
        {
            Debug.LogError("Task index is out of range");
            return;
        }
        currentTask = taskList[currentTaskIndex];
    }

    public int GetCurrentTaskIndex()
    {
        return currentTaskIndex;
    }

    public Task GetCurrentTask()
    {
        return currentTask;
    }

    public Task GetTaskByIndex(int index)
    {
        if (index < 0 || index >= taskList.Count)
        {
            Debug.LogError("Task index is out of range");
            return null;
        }
        return taskList[index];
    }

    public bool IsAllTaskCompleted()
    {
        foreach (Task task in taskList)
        {
            if (task.GetTaskStatus() != Task.TaskStatus.Completed)
            {
                return false;
            }
        }
        return true;
    }

    public void CheckTaskEvent()
    {
        if (currentTask.GetTaskId() == letterEventId && !isLetterEventTriggered)
        {
            TriggerLetterEvent();
            isLetterEventTriggered = true;
        }
        if (currentTask.GetTaskId() == oldManEventId && !isOldManEventTriggered)
        {
            TriggerOldmanEvent();
            isOldManEventTriggered = true;
        }
    }
    public void TriggerOldmanEvent()
    {
        oldMan.gameObject.SetActive(false) ;
        AlbumUI.gameObject.SetActive(true) ;
        AlbumManager.Instance.ChangePage(0);
    }

    public void TriggerLetterEvent()
    {
        LetterUI.gameObject.SetActive(true);
    }

    public void OnXButtonClicked()
    {
        HideLetterUI();
        //TODO: Play Audio
    }
    private void HideLetterUI()
    {
        LetterUI.gameObject.SetActive(false);
    }

    
}
