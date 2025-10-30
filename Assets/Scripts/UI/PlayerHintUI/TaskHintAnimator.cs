using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TaskHintAnimator
{
    public static void FadeIn(CanvasGroup targetGroup, float duration, float delay = 0f)
    {
        // 先确保初始状态是完全透明的
        targetGroup.alpha = 0f;
        // 使用 DOTween 的 DOFade 方法来执行动画
        targetGroup.DOFade(1f, duration).SetDelay(delay).SetEase(Ease.OutQuad);
    }

    public static void FadeOut(CanvasGroup targetGroup, float duration, float delay = 0f)
    {
        // 确保初始状态是完全不透明的
        targetGroup.alpha = 1f;
        // 使用 DOTween 的 DOFade 方法来执行淡出动画
        targetGroup.DOFade(0f, duration).SetDelay(delay).SetEase(Ease.InQuad);
    }

    // 可选：同时淡入淡出两个元素
    public static void CrossFade(CanvasGroup fadeOutGroup, CanvasGroup fadeInGroup, float duration, float delay = 0f)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.SetDelay(delay);
        sequence.Append(fadeOutGroup.DOFade(0f, duration).SetEase(Ease.InQuad));
        sequence.Join(fadeInGroup.DOFade(1f, duration).SetEase(Ease.OutQuad));
    }
}