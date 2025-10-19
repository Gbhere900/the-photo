using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMController : MonoBehaviour
{
    [System.Serializable]
    public class SceneBGM
    {
        public string sceneName;
        public List<string> bgmNames;
        public bool randomPlay = true;
    }

    private float checkTimer = 0f;
    private float checkInterval = 5f; // 每 5 秒检查一次是否播完

    [Header("场景对应 BGM 配置")]
    public List<SceneBGM> sceneBGMs;

    private string currentPlayingBGM = "";
    private string lastSceneName = "";

    public static BGMController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!string.IsNullOrEmpty(currentPlayingBGM) && AudioManager.instance != null)
        {
            AudioManager.instance.Stop(currentPlayingBGM);
        }

        currentPlayingBGM = "";
        lastSceneName = scene.name;

        if (scene.name == "StartPage")
        {
            StartCoroutine(DelayedPlay());
        }
    }

    private IEnumerator DelayedPlay()
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

        PlaySceneBGM(); // 主页面自动播 BGM
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

    //播放当前场景bgm
    public void PlaySceneBGM()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (AudioManager.instance == null)
        {
            Debug.LogError("AudioManager 未初始化！");
            return;
        }

        SceneBGM config = sceneBGMs.Find(s => s.sceneName == sceneName);
        if (config == null || config.bgmNames.Count == 0)
        {
            Debug.LogWarning($"场景 {sceneName} 没有配置可用 BGM");
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
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        SceneBGM config = sceneBGMs.Find(s => s.sceneName == sceneName);
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
