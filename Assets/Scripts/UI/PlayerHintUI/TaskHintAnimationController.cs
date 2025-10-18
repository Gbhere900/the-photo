// TaskHintAnimationController.cs (已修正)
using DG.Tweening;
using UnityEngine;
using TMPro;
using System.Collections;

public class TaskHintAnimationController : MonoBehaviour
{
    [Header("动画对象引用")]
    [Tooltip("需要淡入的文字部分 (请确保该对象有 CanvasGroup 组件)")]
    public CanvasGroup TextCanvasGroup;
    [Tooltip("需要淡出的整个部分 (请确保该对象有 CanvasGroup 组件)")]
    public CanvasGroup WholeCanvasGroup;

    [Tooltip("需要从右侧移入的背景，按播放顺序排列")]
    public RectTransform BG;

    [Header("文字组件")]
    [Tooltip("TextMeshPro 文本组件，用于显示提示文字")]
    public TextMeshProUGUI hintTextComponent;

    [Header("动画参数设置")]
    [Tooltip("文字淡入动画的时长")]
    public float logoFadeDuration = 1.5f;

    [Tooltip("BG移入动画的时长")]
    public float buttonMoveDuration = 0.8f;

    [Tooltip("淡出动画的时长")]
    public float fadeOutDuration = 0.8f;

    [Tooltip("显示完成后等待多少秒开始淡出")]
    public float autoHideDelay = 5f;

    [Header("提示文字")]
    public string hintText = "这些是默认文字";

    private Coroutine autoHideCoroutine;
    private Vector2 bgFinalPosition; // 用于存储BG在编辑器里设置的最终位置

    // Awake 在对象加载时立即执行，甚至在第一帧渲染之前
    void Awake()
    {
        InitializeState();
    }

    void Start()
    {
        // Start中只保留那些不需要在第一帧前完成的操作
        if (hintTextComponent != null)
        {
            hintTextComponent.text = hintText;
        }
    }

    /// <summary>
    /// 新增方法：初始化UI元素到动画开始前的状态
    /// </summary>
    private void InitializeState()
    {
        // 1. 立即将所有需要动画的CanvasGroup设置为完全透明
        if (TextCanvasGroup != null) TextCanvasGroup.alpha = 0f;
        if (WholeCanvasGroup != null) WholeCanvasGroup.alpha = 0f; // 整个UI也应该从透明开始

        // 2. 记录BG的最终位置，然后立即把它移动到屏幕外
        if (BG != null)
        {
            // 记录它在编辑器里设置的最终位置
            bgFinalPosition = BG.anchoredPosition;

            // 计算一个屏幕外的起始位置并立即应用
            float offscreenX = bgFinalPosition.x + Screen.width; // 确保在屏幕右侧外
            BG.anchoredPosition = new Vector2(offscreenX, bgFinalPosition.y);
        }
    }

    // 这个方法现在只负责“播放”动画，而不是“设置并播放”
    public void HintPlayIntroSequence()
    {
        // 确保对象是激活的，以便动画可以播放
        if (WholeCanvasGroup != null) WholeCanvasGroup.gameObject.SetActive(true);
        if (WholeCanvasGroup != null) WholeCanvasGroup.alpha = 1f; // 如果有整体淡出，入场时应该让它不透明

        // 设置提示文字
        if (hintTextComponent != null)
        {
            hintTextComponent.text = hintText;
        }

        // 播放BG移入动画，移动到我们之前保存的最终位置
        if (BG != null)
        {
            BG.DOAnchorPos(bgFinalPosition, buttonMoveDuration).SetEase(Ease.OutQuad);
        }

        // 播放文字淡入动画
        if (TextCanvasGroup != null)
        {
            TaskHintAnimator.FadeIn(TextCanvasGroup, logoFadeDuration, 0.5f);
        }

        // 启动自动隐藏协程
        if (autoHideCoroutine != null)
        {
            StopCoroutine(autoHideCoroutine);
        }
        autoHideCoroutine = StartCoroutine(AutoHideAfterDelay());
    }

    // 自动隐藏协程
    private IEnumerator AutoHideAfterDelay()
    {
        float totalIntroTime = Mathf.Max(logoFadeDuration + 0.5f, buttonMoveDuration);
        yield return new WaitForSeconds(totalIntroTime + autoHideDelay);

        HintPlayOutroSequence();
    }

    // 淡出动画序列
    public void HintPlayOutroSequence()
    {
        // 使用整体的CanvasGroup来淡出所有内容，效果更统一
        if (WholeCanvasGroup != null)
        {
            TaskHintAnimator.FadeOut(WholeCanvasGroup, fadeOutDuration, 0f);
        }
    }

    // 立即隐藏（不播放动画），并重置状态以便下次播放
    public void HideImmediately()
    {
        if (autoHideCoroutine != null)
        {
            StopCoroutine(autoHideCoroutine);
            autoHideCoroutine = null;
        }

        // 确保DOTween动画被杀死，防止干扰
        if (BG != null) BG.DOKill();
        if (TextCanvasGroup != null) TextCanvasGroup.DOKill();
        if (WholeCanvasGroup != null) WholeCanvasGroup.DOKill();

        // 调用初始化方法，将UI重置到隐藏状态
        InitializeState();
    }

    // 设置提示文字
    public void SetHintText(string newText)
    {
        hintText = newText;
        if (hintTextComponent != null)
        {
            hintTextComponent.text = newText;
        }
    }

    // 获取当前提示文字
    public string GetHintText()
    {
        return hintText;
    }

    // 当对象禁用时停止协程
    private void OnDisable()
    {
        if (autoHideCoroutine != null)
        {
            StopCoroutine(autoHideCoroutine);
            autoHideCoroutine = null;
        }
    }
}