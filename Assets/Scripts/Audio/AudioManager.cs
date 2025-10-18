using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioType[] AudioTypes;

    // --- 新增: 用于跟踪正在淡出的协程 ---
    private Dictionary<string, Coroutine> fadeCoroutines = new Dictionary<string, Coroutine>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        foreach (var type in AudioTypes)
        {
            type.Source = gameObject.AddComponent<AudioSource>();
            type.Source.clip = type.Clip;
            type.Source.name = type.Name;
            type.Source.volume = type.Volume;
            type.Source.pitch = type.Pitch;
            type.Source.loop = type.Loop;

            if (type.Group != null)
            {
                type.Source.outputAudioMixerGroup = type.Group;
            }

            // --- 修改: 实现 Play On Start ---
            if (type.playOnStart)
            {
                type.Source.Play();
            }
        }
    }

    public void Play(string name)
    {
        AudioType type = FindAudioType(name);
        if (type != null)
        {
            // --- 新增: 播放前停止可能存在的淡出效果 ---
            if (fadeCoroutines.ContainsKey(name))
            {
                StopCoroutine(fadeCoroutines[name]);
                fadeCoroutines.Remove(name);
            }
            // 恢复原始音量再播放
            type.Source.volume = type.Volume;
            type.Source.Play();
        }
        else
        {
            Debug.LogWarning("AudioManager: 音频 " + name + " 没找到");
        }
    }

    public void Pause(string name)
    {
        AudioType type = FindAudioType(name);
        if (type != null)
        {
            type.Source.Pause();
        }
        else
        {
            Debug.LogWarning("AudioManager: 音频 " + name + " 没找到");
        }
    }

    public void Stop(string name)
    {
        AudioType type = FindAudioType(name);
        if (type != null)
        {
            // --- 修改: 实现淡出停止 ---
            if (type.fadeOutDuration > 0f)
            {
                // 如果已经有一个淡出正在进行，先停掉老的
                if (fadeCoroutines.ContainsKey(name))
                {
                    StopCoroutine(fadeCoroutines[name]);
                }
                // 开始新的淡出协程并记录下来
                Coroutine fadeCoroutine = StartCoroutine(FadeOutAndStop(type));
                fadeCoroutines[name] = fadeCoroutine;
            }
            else
            {
                // 如果淡出时间为0，则立即停止
                type.Source.Stop();
            }
        }
        else
        {
            Debug.LogWarning("AudioManager: 音频 " + name + " 没找到");
        }
    }

    // --- 新增: 淡出并停止的协程 ---
    private IEnumerator FadeOutAndStop(AudioType audioType)
    {
        float startVolume = audioType.Source.volume;
        float timer = 0f;

        while (timer < audioType.fadeOutDuration)
        {
            // 计算当前音量
            audioType.Source.volume = Mathf.Lerp(startVolume, 0f, timer / audioType.fadeOutDuration);
            timer += Time.deltaTime;
            yield return null; // 等待下一帧
        }

        // 确保完全停止和音量归零
        audioType.Source.Stop();
        audioType.Source.volume = audioType.Volume; // 将音量恢复到预设值，以便下次播放

        // --- 新增: 淡出完成后，从字典中移除记录 ---
        fadeCoroutines.Remove(audioType.Name);
    }

    // --- 新增: 辅助函数，避免代码重复 ---
    private AudioType FindAudioType(string name)
    {
        return System.Array.Find(AudioTypes, audio => audio.Name == name);
    }
}