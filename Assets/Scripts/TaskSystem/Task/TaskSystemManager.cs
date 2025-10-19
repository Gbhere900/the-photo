using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// ��������൥��
public class TaskSystemManager : SingletonMonoBase<TaskSystemManager>
{
    [Header("�������")]
    [SerializeField] private List<Task> taskList = new List<Task>();

    [Header("�ŷ�����")]
    [SerializeField] private string letterEventId;
    private bool isLetterEventTriggered = false;
    [SerializeField] private Transform LetterUI;

    [Header("��������")]
    [SerializeField] private string oldManEventId;
    private bool isOldManEventTriggered = false;
    [SerializeField] private Transform oldMan;
    [SerializeField] private Transform albumUI;
    
    [Header("��ǰ����")]
    [SerializeField] private Task currentTask;
    private int currentTaskIndex;


    protected override void Awake()
    {
        base.Awake();
        Initialized();
    }
    
    /// <summary>
    /// ��ʼ������
    /// </summary>
    private void Initialized()
    {
        if (taskList.Count == 0)
        {
            return;
        }
        currentTaskIndex = 0;
        currentTask = taskList[currentTaskIndex];
        // ����������״̬����Ϊ����ȡ
        foreach (Task task in taskList)
        {
            task.SetTaskType(Task.TaskStatus.Pending);
        }
    }

    //--------------------------�����ӿ�--------------------------
    /// <summary>
    /// ��������Ϊ����ȡ״̬
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
    /// ��ȡ��ǰ����
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
    /// ����ǰ������Ϊ��ɣ����ۼ���������
    /// </summary>
    public void SetCurrentTaskCompleted()
    {
        if (currentTask == null || currentTaskIndex >= taskList.Count)
        {
            Debug.LogError("Current task is null");
            return;
        }
        currentTask.SetTaskType(Task.TaskStatus.Completed);
        Debug.Log(string.Format("����:{{%d}} ���!", currentTaskIndex));
        currentTask = null;
        
        // �ۼ�����������ָ����һ������
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
        albumUI.gameObject.SetActive(true) ;
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
