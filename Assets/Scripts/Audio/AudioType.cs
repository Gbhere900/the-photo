using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class AudioType
{
    // 我帮你做了一些小的调整，让 Inspector 更好看和易用
    public string Name;
    public AudioClip Clip;

    [Header("基本属性")]
    [Range(0f, 1f)]
    public float Volume = 1f;
    [Range(0.1f, 3f)]
    public float Pitch = 1f;
    public bool Loop;

    [Tooltip("如果勾选，该音效会在游戏开始时自动播放 (常用于BGM)")]
    public bool playOnStart; // --- 新增: 是否立即播放 ---

    [Header("高级设置")]
    public AudioMixerGroup Group;
    [Tooltip("调用 Stop() 时的淡出时长（秒）。设置为 0 则立即停止。")]
    public float fadeOutDuration = 1.0f; // --- 新增: 淡出时长 ---

    [HideInInspector]
    public AudioSource Source;
}