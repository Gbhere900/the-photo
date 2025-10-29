using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 这是一个全局控制器，用于根据当前场景的名称来管理鼠标的锁定状态。
/// 在指定场景中，它会自动解锁并显示鼠标；在其他所有场景中，它会锁定并隐藏鼠标。
/// </summary>
public class SceneCursorUnlocker : MonoBehaviour
{
    public static SceneCursorUnlocker instance;

    [Header("配置")]
    [Tooltip("请在此处填写需要解锁鼠标的场景的【确切】名称，例如 'MainMenu' 或 'StartPage'")]
    [SerializeField]
    private string targetSceneName = "YourSceneNameHere"; // <-- 在 Inspector 中修改这个值

    private void Awake()
    {
        // 实现单例模式，确保全局只有一个实例
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        // 当此对象启用时，开始监听场景加载事件
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // 当此对象禁用或销毁时，停止监听，防止内存泄漏
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 每次有新场景加载完成时，这个方法就会被调用
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 检查加载的场景名称是否是我们指定的目标场景
        if (scene.name == targetSceneName)
        {
            // 如果是，就解锁鼠标
            UnlockCursor();
        }
        else
        {
            // 如果不是，就锁定鼠标（这是默认的游戏状态）
            LockCursor();
        }
    }

    /// <summary>
    /// 解锁并显示鼠标。代码参考自你的 Player.cs。
    /// </summary>
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log($"进入场景 '{targetSceneName}'，鼠标已解锁。");
    }

    /// <summary>
    /// 锁定并隐藏鼠标。代码参考自你的 Player.cs。
    /// </summary>
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log($"进入非解锁场景，鼠标已锁定。");
    }
}