using System.Collections.Generic;
using UnityEngine;

public class WorldTextTest : MonoBehaviour
{
    public Transform targetNPC; // 拖一个测试目标物体，比如 NPC、Cube 等
    private bool isLocked = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !isLocked)
        {
            // 在调用前最好加一个空值检查，防止忘记在 Inspector 中拖拽 targetNPC
            if (targetNPC == null)
            {
                Debug.LogError("targetNPC has not been assigned in the Inspector!", this);
                return;
            }

            isLocked = true;
            List<string> testLines = new List<string>
            {
                "你好，冒险者。",
                "这是任务 1 的提示对话。",
                "文字太长了？我们会自动分页。"
            };

            // --- 修改这里 ---
            // 将 transform 改为 targetNPC
            WorldTextManager.Instance.ShowDialogue(targetNPC, testLines, () =>
            {
                isLocked = false; // 播完才解锁
            });
        }
    }
}