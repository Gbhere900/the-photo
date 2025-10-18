using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TaskDialogueEntry
{
    public string taskId;
    [TextArea(2, 5)]
    public List<string> lines;
}

public class DialogueData : MonoBehaviour
//这个文件挂在触发对话的物体上，存储任务ID和对应的对话内容
{
    public List<TaskDialogueEntry> entries;

    public List<string> GetDialogue(string taskId)
    {
        foreach (var entry in entries)
        {
            if (entry.taskId == taskId)
                return entry.lines;
        }
        return new List<string> { "......" };
    }
}
