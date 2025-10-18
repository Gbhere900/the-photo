// WorldSpaceHintTrigger.cs
using UnityEngine;
using System.Collections.Generic; // 如果你想用多行文本

public class WorldSpaceHintTrigger : MonoBehaviour
{
    [Header("提示设置")]
    [Tooltip("拖入您的 3D Text Bubble 预制体")]
    public GameObject textBubblePrefab;

    [Tooltip("要显示的提示文字")]
    [TextArea(3, 5)] // 让 Inspector 里的输入框变大
    public string hintText = "按 E 互动";

    [Tooltip("文字气泡相对于此触发器中心的位置偏移")]
    public Vector3 textOffset = new Vector3(0, 2f, 0f);

    private WorldTextBubble currentBubble; // 用于存储当前生成的文字气泡实例

    private void OnTriggerEnter(Collider other)
    {
        // 检查进入的是否是玩家，并且当前没有正在显示的提示
        if (other.CompareTag("Player") && currentBubble == null)
        {
            // 实例化预制体
            GameObject bubbleObject = Instantiate(textBubblePrefab, transform.position, transform.rotation);
            currentBubble = bubbleObject.GetComponent<WorldTextBubble>();

            if (currentBubble != null)
            {
                // 调用我们即将添加的新方法，来显示一个持续的提示
                currentBubble.ShowAsHint(hintText, this.transform, textOffset);
            }
            else
            {
                Debug.LogError("textBubblePrefab 上没有找到 WorldTextBubble 脚本！", this.gameObject);
                Destroy(bubbleObject); // 清理错误的实例
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 检查离开的是否是玩家，并且当前有正在显示的提示
        if (other.CompareTag("Player") && currentBubble != null)
        {
            // 调用我们即将添加的销毁方法
            currentBubble.DestroyBubble();
            currentBubble = null; // 清空引用，以便玩家下次进入时可以重新创建
        }
    }
}