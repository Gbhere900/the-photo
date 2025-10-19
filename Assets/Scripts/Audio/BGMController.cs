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
        public bool randomPlay = false;
    }

    [Header("场景对应 BGM 配置")]
    public List<SceneBGM> sceneBGMs;

    private string currentPlayingBGM = "";
    private string lastSceneName = "";

    private IEnumerator Start()
    {
        // 等一帧，确保 AudioManager.instance 初始化完成
        yield return null;

        PlaySceneBGM(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaySceneBGM(scene.name);
    }

    private void PlaySceneBGM(string sceneName)
    {
        if (AudioManager.instance == null)
        {
            Debug.LogError("AudioManager 未初始化！");
            return;
        }

        SceneBGM config = sceneBGMs.Find(s => s.sceneName == sceneName);
        if (config != null && config.bgmNames.Count > 0)
        {
            string toPlay = config.randomPlay
                ? config.bgmNames[Random.Range(0, config.bgmNames.Count)]
                : config.bgmNames[0];


            if (!string.IsNullOrEmpty(currentPlayingBGM))
            {
                AudioManager.instance.Stop(currentPlayingBGM);
            }

            AudioManager.instance.Play(toPlay);
            currentPlayingBGM = toPlay;
            lastSceneName = sceneName; // 别忘了更新
        }
        else
        {
            if (!string.IsNullOrEmpty(currentPlayingBGM))
            {
                AudioManager.instance.Stop(currentPlayingBGM);
                currentPlayingBGM = "";
            }

            Debug.LogWarning($"当前场景 {sceneName} 没有配置 BGM");
        }
    }

    public void PlayAreaBGM(string bgmName)
    {
        if (AudioManager.instance == null)
        {
            Debug.LogError("AudioManager 未初始化！");
            return;
        }

        if (currentPlayingBGM == bgmName) return;

        if (!string.IsNullOrEmpty(currentPlayingBGM))
        {
            AudioManager.instance.Stop(currentPlayingBGM);
        }

        AudioManager.instance.Play(bgmName);
        currentPlayingBGM = bgmName;
    }
}
