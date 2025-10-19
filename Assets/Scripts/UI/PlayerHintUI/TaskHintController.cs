// TaskHintController.cs (重构与优化版)
using DG.Tweening;
using UnityEngine;
using TMPro;

/// <summary>
/// 控制任务提示UI的完整动画流程，包括进场、显示、退场和状态重置。
/// 这是一个自包含的组件，负责管理其控制的所有UI元素。
/// </summary>
public class TaskHintController : MonoBehaviour
{
    [Header("UI元素引用")]
    [Tooltip("控制整体淡入淡出的CanvasGroup")]
    public CanvasGroup wholeCanvasGroup;
    [Tooltip("控制背景板移动的RectTransform")]
    public RectTransform backgroundRect;
    [Tooltip("仅控制文字淡入的CanvasGroup")]
    public CanvasGroup textCanvasGroup;
    [Tooltip("显示提示文本的TextMeshPro组件")]
    public TextMeshProUGUI hintTextComponent;

    [Header("动画参数")]
    [Tooltip("背景板从屏幕外滑入所需时间")]
    public float backgroundMoveDuration = 0.8f;
    [Tooltip("文字淡入所需时间")]
    public float textFadeInDuration = 1.0f;
    [Tooltip("文字在背景板滑入后延迟多久开始淡入")]
    public float textFadeInDelay = 0.2f;
    [Tooltip("提示内容完整显示后的停留时间")]
    public float displayDuration = 4f;
    [Tooltip("整体淡出所需时间")]
    public float fadeOutDuration = 0.8f;

    [Header("默认内容")]
    [Tooltip("当调用不带参数的ShowHint()时显示的默认文本")]
    [TextArea(3, 5)] // 让Inspector里的输入框更大
    public string defaultHintText = "这是一个默认提示...";

    // 私有状态变量
    private Vector2 backgroundOnScreenPosition;  // 背景在屏幕内的目标位置
    private Vector2 backgroundOffScreenPosition; // 背景在屏幕外的起始/终点位置
    private Sequence currentAnimationSequence;   // 当前正在播放的动画序列

    private void Awake()
    {
        // 1. 初始化位置信息
        // 记录下UI在编辑器里摆放好的最终位置
        backgroundOnScreenPosition = backgroundRect.anchoredPosition;

        // 根据最终位置和自身宽度，计算出一个可靠的屏幕外位置（这里假设从右侧进入）
        float offscreenX = backgroundOnScreenPosition.x + backgroundRect.rect.width;
        backgroundOffScreenPosition = new Vector2(offscreenX, backgroundOnScreenPosition.y);

        // 2. 保证初始状态是完全隐藏且位于屏幕外的
        // 这是防止游戏开始时UI闪一下的最佳实践
        wholeCanvasGroup.alpha = 0f;
        backgroundRect.anchoredPosition = backgroundOffScreenPosition;
        gameObject.SetActive(false); // 也可以用这个，取决于你的整体UI管理策略
    }

    // --- 公开接口 ---

    /// <summary>
    /// 显示提示框，使用默认的提示文本。
    /// </summary>
    public void ShowHint()
    {
        ShowHint(defaultHintText);
    }

    /// <summary>
    /// 显示提示框，并设置指定的提示文本。
    /// </summary>
    /// <param name="newHintText">要显示的新的提示内容</param>
    public void ShowHint(string newHintText)
    {
        // 如果上一个动画还在播放，先彻底杀死它，防止动画叠加或冲突
        if (currentAnimationSequence != null && currentAnimationSequence.IsActive())
        {
            currentAnimationSequence.Kill();
        }

        // 1. 准备工作：激活对象并设置文本
        gameObject.SetActive(true);
        hintTextComponent.text = newHintText;

        // 2. 立即重置所有UI元素到动画开始前的“初始状态”
        // 这一步至关重要，确保每次动画都从一个干净的状态开始
        backgroundRect.anchoredPosition = backgroundOffScreenPosition; // 确保背景在屏幕外
        textCanvasGroup.alpha = 0f;    // 文字初始为透明
        wholeCanvasGroup.alpha = 1f;   // 整体容器初始为不透明（为了能看到背景滑入）

        // 3. 创建并播放新的动画序列
        currentAnimationSequence = DOTween.Sequence();

        currentAnimationSequence
            // 第1步: 背景从屏幕外滑入到目标位置
            .Append(backgroundRect.DOAnchorPos(backgroundOnScreenPosition, backgroundMoveDuration).SetEase(Ease.OutCubic))

            // 第2步: 文字在稍作延迟后淡入
            .Append(textCanvasGroup.DOFade(1f, textFadeInDuration).SetDelay(textFadeInDelay))

            // 第3步: 等待指定时间
            .AppendInterval(displayDuration)

            // 第4步: 整体淡出
            .Append(wholeCanvasGroup.DOFade(0f, fadeOutDuration).SetEase(Ease.InCubic))

            // 第5步: 动画播放完毕后的收尾工作
            .OnComplete(() => {
                // 关键修正！在这里不仅要隐藏GameObject，还要把位置重置回屏幕外
                // 这样就为下一次调用做好了万全的准备。
                gameObject.SetActive(false);
                backgroundRect.anchoredPosition = backgroundOffScreenPosition;
            });
    }

    /// <summary>
    /// 立即隐藏提示框，并停止所有相关动画。
    /// </summary>
    public void HideImmediately()
    {
        if (currentAnimationSequence != null && currentAnimationSequence.IsActive())
        {
            currentAnimationSequence.Kill();
        }
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        // 这是一个好习惯：在对象被销毁时，确保杀死所有关联的DOTween动画，防止内存泄漏
        if (currentAnimationSequence != null)
        {
            currentAnimationSequence.Kill();
        }
    }
}