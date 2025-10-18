// StartMenuController.cs
using UnityEngine;
using DG.Tweening; // 确保你也在这里导入了 DOTween

public class StartMenuController : MonoBehaviour
{
    [Header("动画对象引用")]
    [Tooltip("需要淡入的 Logo (请确保该对象有 CanvasGroup 组件)")]
    public CanvasGroup logoCanvasGroup;

    [Tooltip("需要从左侧移入的按钮数组，按播放顺序排列")]
    public RectTransform[] buttons;

    [Header("动画参数设置")]
    [Tooltip("Logo 淡入动画的时长")]
    public float logoFadeDuration = 1.5f;

    [Tooltip("每个按钮移入动画的时长")]
    public float buttonMoveDuration = 0.8f;

    [Tooltip("每个按钮之间动画的间隔时间")]
    public float delayBetweenButtons = 0.3f;

    void Start()
    {
        // 启动开场动画序列
        PlayIntroSequence();
    }

    private void PlayIntroSequence()
    {
        // --- 步骤 0: 初始化状态 ---
        // 确保 Logo 开始时是透明的 (UIAnimator.FadeIn 内部也会设置，但这里写更清晰)
        logoCanvasGroup.alpha = 0f;
        // 确保按钮开始时是不可见的（UIAnimator.MoveInFromLeft 会把它们移到屏幕外）
        foreach (var btn in buttons)
        {
            // 你也可以在这里把按钮的 GameObject 设置为 false，然后在动画前再打开
            // 但让动画函数自己处理位置更简单
        }

        // --- 步骤 1: 播放 Logo 淡入动画 ---
        UIAnimator.FadeIn(logoCanvasGroup, logoFadeDuration,0.5f);

        // --- 步骤 2: 依次播放按钮移入动画 ---
        // 计算第一个按钮开始动画的延迟时间，应该在 Logo 动画开始后不久
        // 这里我们让它在 Logo 动画开始后 0.5 秒开始
        float initialButtonDelay = 1f;

        for (int i = 0; i < buttons.Length; i++)
        {
            // 计算当前按钮的总延迟时间
            // = 初始延迟 + 它在队列中的位置 * 间隔时间
            float currentDelay = initialButtonDelay + (i * delayBetweenButtons);

            // 调用我们的动画工具函数来播放移动动画
            UIAnimator.MoveInFromLeft(buttons[i], buttonMoveDuration, currentDelay);
        }
    }
}