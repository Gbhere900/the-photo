using System.Collections.Generic;
using UnityEngine;

// [System.Serializable] 的作用是让下面的这个类能在Unity的Inspector面板里显示出来，方便我们填写数据。
[System.Serializable]
public class TaskHintEntry
{
    public string taskId;
    public string lines;
}


public class HintData : MonoBehaviour
{
    public List<TaskHintEntry> entries;

    public string GetHint(string taskId)
    {
        foreach (var entry in entries)
        {
            if (entry.taskId == taskId)
            {
                // 确保如果策划忘记填对话了，也不会导致游戏崩溃
                if (entry.lines == null )
                {
                    return new string("暂无提示");
                }
                return entry.lines;
            }
        }

        // 如果上面的循环没有找到任何匹配的taskId，就返回一个默认的对话。
        Debug.LogWarning($"在物体 {gameObject.name} 上没有找到任务ID为 '{taskId}' 的提示。返回默认提示。");
        return new string("暂无提示");
    }
}