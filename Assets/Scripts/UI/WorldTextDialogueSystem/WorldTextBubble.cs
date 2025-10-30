using UnityEngine;
using DG.Tweening; // 别忘了加这个

[RequireComponent(typeof(TextMesh))]
public class WorldTextBubble : MonoBehaviour
{
    public Vector3 localOffset = new Vector3(0, 2f, 0f);
    private Transform followTarget;
    private TextMesh textMesh;
    public float typeSpeed = 0.04f; // 打字效果的每字间隔
    private Sequence typingSequence; // 引用打字动画，以便可以中断它
    private Vector3 originalScale; // 用于存储预制体的原始大小

    void Awake()
    {
        textMesh = GetComponent<TextMesh>();
        originalScale = transform.localScale;
    }

    //初始化这个bubble的函数
    public void Initialize(string fullText, Transform target, float durationPerLine)
    {
        followTarget = target;
        textMesh.text = "";

        // 打字机动画：每字一个 DOTween 延迟
        typingSequence = DOTween.Sequence();
        for (int i = 0; i <= fullText.Length; i++)
        {
            string sub = fullText.Substring(0, i);
            typingSequence.AppendCallback(() => textMesh.text = sub);
            typingSequence.AppendInterval(typeSpeed);
        }

        Destroy(gameObject, durationPerLine); // 仍按总时间销毁
    }

    // --- 新功能：用于显示一个持续的提示 ---
    public void ShowAsHint(string text, Transform target, Vector3 offset)
    {
        followTarget = target;
        localOffset = offset;
        textMesh.text = text;

        // --- 核心改动 ---
        // 1. 立即将大小设置为0
        transform.localScale = Vector3.zero;

        // 2. 播放一个平滑的放大动画，恢复到原始大小
        // Ease.OutBack 会产生一个很棒的、轻微“弹出”的弹性效果
        transform.DOScale(originalScale, 0.5f).SetEase(Ease.OutBack);
    }

    // --- 新功能：用于外部调用的销毁方法 ---
    public void DestroyBubble()
    {
        // 停止所有正在进行的动画
        if (typingSequence != null && typingSequence.IsActive())
        {
            typingSequence.Kill();
        }
        transform.DOKill();

        // 播放一个平滑的缩小消失动画，动画结束后再销毁对象
        transform.DOScale(Vector3.zero, 0.5f)
                 .SetEase(Ease.InBack) // InBack 效果会有一个轻微的回弹，感觉很棒
                 .OnComplete(() => {
                     Destroy(gameObject);
                 });
    }

    //设置好字的位置和朝向
    void LateUpdate()
    {
        if (followTarget)
        {
            Vector3 worldOffset = followTarget.TransformPoint(localOffset);
            transform.position = worldOffset;

            Vector3 camForward = transform.position - Camera.main.transform.position;
            transform.rotation = Quaternion.LookRotation(camForward);
        }
    }
}