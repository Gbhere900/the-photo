using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskSystemTestScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TaskSystemManager.Instance.SetCurrentTaskCompleted();
            Task currentTask = TaskSystemManager.Instance.GetCurrentTask();
            if (currentTask != null)
            {
                Debug.Log(string.Format("目前任务:{0}", currentTask.GetTaskId()));
            }
            else
            {
                if (TaskSystemManager.Instance.IsAllTaskCompleted() == true)
                {
                    Debug.Log("所有任务全部完成");
                }
            }
        }
    }
}
