using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader: MonoBehaviour
{
    // 用一个静态变量来存储我们要加载的目标场景的名称
    public static string targetSceneName;

    /// <summary>
    /// 调用此方法来加载一个新场景（会经过加载场景）。
    /// </summary>
    /// <param name="sceneName">你最终想要加载的场景的名称</param>
    /// 

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static void LoadScene(string sceneName)
    {
        // 1. 将目标场景名称存起来
        targetSceneName = sceneName;

        // 2. 加载我们的“加载场景”
        UnityEngine.SceneManagement.SceneManager.LoadScene(targetSceneName); // <-- 确保你的加载场景就叫这个名字

        DynamicGI.UpdateEnvironment();
    }
}