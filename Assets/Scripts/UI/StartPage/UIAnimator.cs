// UIAnimator.cs
using UnityEngine;
using DG.Tweening; // 确保你已经导入了 DOTween 命名空间

/// <summary>
/// 一个静态的 UI 动画工具类，提供常用的 UI 动画函数。
/// </summary>
public static class UIAnimator
{
    /// <summary>
    /// 让 UI 元素平滑淡入（修改 CanvasGroup 的 Alpha）。
    /// </summary>
    /// <param name="targetGroup">需要执行动画的 CanvasGroup 组件</param>
    /// <param name="duration">动画时长（秒）</param>
    /// <param name="delay">动画开始前的延迟（秒）</param>
    public static void FadeIn(CanvasGroup targetGroup, float duration, float delay = 0f)
    {
        // 先确保初始状态是完全透明的
        targetGroup.alpha = 0f;
        // 使用 DOTween 的 DOFade 方法来执行动画
        targetGroup.DOFade(1f, duration).SetDelay(delay).SetEase(Ease.OutQuad);
    }

    /// <summary>
    /// 让 UI 元素从左侧屏幕外平滑移入到它在编辑器里设置的最终位置。
    /// </summary>
    /// <param name="targetRect">需要执行动画的 RectTransform 组件</param>
    /// <param name="duration">动画时长（秒）</param>
    /// <param name="delay">动画开始前的延迟（秒）</param>
    public static void MoveInFromLeft(RectTransform targetRect, float duration, float delay = 0f)
    {
        // 1. 记录它在编辑器里设置的最终位置
        Vector2 finalPosition = targetRect.anchoredPosition;

        // 2. 计算一个屏幕外的起始位置（这里简单地在X轴上减去屏幕宽度，确保它在屏幕外）
        //    更精确的方式是计算 Canvas 的宽度，但这样通常足够了。
        float offscreenX = finalPosition.x - Screen.width;
        Vector2 startPosition = new Vector2(offscreenX, finalPosition.y);

        // 3. 立即把它放到起始位置
        targetRect.anchoredPosition = startPosition;

        // 4. 使用 DOTween 的 DOAnchorPos 方法，让它动画到最终位置
        targetRect.DOAnchorPos(finalPosition, duration).SetDelay(delay).SetEase(Ease.OutQuad);
    }
}