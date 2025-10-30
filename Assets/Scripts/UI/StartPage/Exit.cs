using UnityEngine;

/// <summary>
/// 一个简单的游戏退出控制器。
/// 提供了可以被UI按钮调用的公共方法，也支持通过键盘按键退出。
/// </summary>
public class Exit : MonoBehaviour
{
    // 你可以在 Update 方法中监听一个特定的退出键，比如 Escape 键
    void Update()
    {
        // 检查玩家是否按下了 Escape 键
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 如果按下了，就调用退出函数
            QuitGame();
        }
    }

    /// <summary>
    /// 这是核心的退出游戏函数。
    /// 它可以被UI按钮或其他脚本调用。
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("游戏退出指令已接收！");

        // --- 这是最重要的部分 ---

        // 如果在 Unity 编辑器中运行
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

        // 如果是编译后的游戏程序
#else
        Application.Quit();
#endif
    }
}