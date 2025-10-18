using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewTask", menuName = "TaskSystem/Task")]
public class Task : ScriptableObject
{
    public enum TaskStatus
    {
        Pending = 0,    // 待接取
        InProgress = 1, // 进行中
        Completed = 2   // 已完成
    };
    
    [SerializeField] private int id;
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private TaskItem taskItem;
    [FormerlySerializedAs("taskType")] [SerializeField] private TaskStatus taskStatus;

    private static readonly List<string> taskStatusString = new List<string>
    {
        "待接取",  // 索引0 - Pending
        "进行中",  // 索引1 - InProgress
        "已完成"   // 索引2 - Completed
    };
    
    //--------------------------工具方法--------------------------
    /// <summary>
    /// TaskStatus枚举转化为字符串
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public static string TaskStatus2String(TaskStatus status)
    {
        return taskStatusString[(int)status];
    }
    
    //--------------------------公共接口--------------------------
    public int GetTaskId()
    {
        return id;
    }

    public string GetTaskName()
    {
        return name;
    }

    public string GetTaskDescription()
    {
        return description;
    }
    
    public TaskItem GetTaskItem()
    {
        return taskItem;
    }
    
    public void SetTaskType(TaskStatus taskStatus)
    {
        this.taskStatus = taskStatus;
    }

    public TaskStatus GetTaskStatus()
    {
        return taskStatus;
    }
}
