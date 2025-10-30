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
                Debug.Log(string.Format("Ŀǰ����:{0}", currentTask.GetTaskId()));
            }
            else
            {
                if (TaskSystemManager.Instance.IsAllTaskCompleted() == true)
                {
                    Debug.Log("��������ȫ�����");
                }
            }
        }
    }
}
