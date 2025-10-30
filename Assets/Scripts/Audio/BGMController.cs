using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMController : MonoBehaviour
{
    [System.Serializable]
    public class SceneBGM
    {
        public string sceneName; // 这个字段现在只起一个注释作用
        public List<string> bgmNames;
        public bool randomPlay = true;
    }

    private float checkTimer = 0f;
    private float checkInterval = 5f; // 每 5 秒检查一次是否播完

    [Header("全局 BGM 播放列表")]
    [Tooltip("现在只会使用列表中的第一个元素（Element 0）作为全局播放列表")]
    public List<SceneBGM> sceneBGMs; // <-- 重要：现在只会使用这个列表的第一个配置

    private string currentPlayingBGM = "";
    // private string lastSceneName = ""; // <-- 不再需要

    public static BGMController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            // --- 修改点 1: 注释掉对场景加载的监听 ---
            // UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- 新增 Start 方法，用于在游戏开始时启动BGM ---
    private void Start()
    {
        // 游戏一开始就准备播放BGM
        StartCoroutine(PlayBGMWhenReady());
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            // --- 修改点 2: 同样注释掉取消监听的代码 ---
            // UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void Update()
    {
        if (string.IsNullOrEmpty(currentPlayingBGM)) return;
        if (AudioManager.instance == null) return;

        checkTimer += Time.deltaTime;
        if (checkTimer >= checkInterval)
        {
            checkTimer = 0f;

            var type = FindCurrentAudioType(currentPlayingBGM);
            if (type != null && !type.Source.isPlaying && !type.Source.loop)
            {
                PlayNextRandomBGM();
            }
        }
    }

    // --- 修改点 3: OnSceneLoaded 方法可以完全注释掉或删除了，因为它不再被调用 ---
    /*
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 所有逻辑都已失效，因为我们不再监听这个事件
    }
    */

    // 这个方法取代了之前的 DelayedPlay
    private IEnumerator PlayBGMWhenReady()
    {
        // 等待 AudioManager.instance 初始化
        while (AudioManager.instance == null)
        {
            yield return null;
        }

        // 等待 AudioManager 的 Start() 执行完
        while (!IsAudioManagerReady())
        {
            yield return null;
        }

        PlayGlobalBGM(); // 开始播放全局BGM
    }

    private bool IsAudioManagerReady()
    {
        var am = AudioManager.instance;
        if (am.AudioTypes == null || am.AudioTypes.Length == 0)
            return false;

        foreach (var type in am.AudioTypes)
        {
            if (type == null || type.Source == null)
                return false;
        }

        return true;
    }

    // 这个方法取代了之前的 PlaySceneBGM
    public void PlayGlobalBGM()
    {
        if (AudioManager.instance == null)
        {
            Debug.LogError("AudioManager 未初始化！");
            return;
        }

        // --- 修改点 4: 不再根据场景名查找，而是直接使用第一个配置 ---
        if (sceneBGMs == null || sceneBGMs.Count == 0)
        {
            Debug.LogWarning("BGMController 没有配置任何 BGM 列表！");
            return;
        }
        SceneBGM config = sceneBGMs[0]; // 直接获取第一个配置作为全局列表

        if (config == null || config.bgmNames.Count == 0)
        {
            Debug.LogWarning($"全局BGM列表（Element 0）没有配置可用 BGM");
            return;
        }

        if (!string.IsNullOrEmpty(currentPlayingBGM)) return; // 如果已经播了就不再播

        string toPlay = config.randomPlay
            ? config.bgmNames[Random.Range(0, config.bgmNames.Count)]
            : config.bgmNames[0];

        AudioManager.instance.Play(toPlay);
        currentPlayingBGM = toPlay;
    }

    private void PlayNextRandomBGM()
    {
        // --- 修改点 5: 同样，直接使用第一个配置 ---
        if (sceneBGMs == null || sceneBGMs.Count == 0) return;
        SceneBGM config = sceneBGMs[0];

        if (config == null || config.bgmNames.Count == 0) return;

        string next;
        do
        {
            next = config.randomPlay
                ? config.bgmNames[Random.Range(0, config.bgmNames.Count)]
                : config.bgmNames[0];
        }
        while (next == currentPlayingBGM && config.bgmNames.Count > 1); // 不重复播

        AudioManager.instance.Play(next);
        currentPlayingBGM = next;
    }

    private AudioType FindCurrentAudioType(string name)
    {
        return System.Array.Find(AudioManager.instance.AudioTypes, a => a.Name == name);
    }
}