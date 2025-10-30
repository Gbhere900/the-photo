using System.Collections.Generic;
using UnityEngine;

// [System.Serializable] 的作用是让下面的这个类能在Unity的Inspector面板里显示出来，方便我们填写数据。
[System.Serializable]
public class TaskDialogueEntry
{
    // 【必填】任务ID：这个ID必须和任务系统里的任务ID完全一致，程序会用它来查找对应的对话。
    // 例如：任务系统里的任务ID是 "find_cat"，这里也要填 "find_cat"。
    public string taskId;

    // 【必填】对话列表：存放这个任务ID对应的所有对话文本。
    // 程序会按顺序（从上到下）一条一条地显示这些对话。
    // 你可以添加任意多句对话。
    // 为了让输入长对话更方便，我们把每一句话的输入框都设置成了可以拉伸的多行文本框。
    [TextArea(3, 10)] // 这个属性让输入框默认有3行高，最多可以拉伸到10行高。
    public List<string> lines;
}


// =================================================================================================
// 这个组件是用来配置一个NPC或一个触发点可以说的所有对话内容的。
// 策划或文案同学需要在这里把每个任务对应的对话一句一句填好。
// =================================================================================================
public class DialogueData : MonoBehaviour
{
    // 这是所有对话条目的列表。
    // 你可以在Unity编辑器的Inspector面板里添加任意多个“任务对话条目 (TaskDialogueEntry)”。
    // 比如：一个NPC身上可以配置“任务1”的对话，“任务2”的对话，“日常闲聊”的对话等等。
    public List<TaskDialogueEntry> entries;

    // --- 以下是程序逻辑，你的同学不需要关心 ---

    /// <summary>
    /// 根据任务ID获取对应的对话列表
    /// </summary>
    /// <param name="taskId">要查找的任务ID</param>
    /// <returns>返回一个包含所有对话句子的列表。如果找不到，返回一个默认的"......"</returns>
    public List<string> GetDialogue(string taskId)
    {
        foreach (var entry in entries)
        {
            if (entry.taskId == taskId)
            {
                // 确保如果策划忘记填对话了，也不会导致游戏崩溃
                if (entry.lines == null || entry.lines.Count == 0)
                {
                    Debug.LogWarning($"任务ID '{taskId}' 找到了，但是对话列表是空的。返回默认对话。");
                    return new List<string> { "......" };
                }
                return entry.lines;
            }
        }

        // 如果上面的循环没有找到任何匹配的taskId，就返回一个默认的对话。
        Debug.LogWarning($"在物体 {gameObject.name} 上没有找到任务ID为 '{taskId}' 的对话。返回默认对话。");
        return new List<string> { "......" };
    }
}