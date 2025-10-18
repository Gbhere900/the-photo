using UnityEngine;
using DG.Tweening; // 别忘了加这个

[RequireComponent(typeof(TextMesh))]
public class WorldTextBubble : MonoBehaviour
{
    public Vector3 localOffset = new Vector3(0, 2f, 0f);
    private Transform followTarget;
    private TextMesh textMesh;
    public float typeSpeed = 0.04f; // 打字效果的每字间隔

    void Awake()
    {
        textMesh = GetComponent<TextMesh>();
    }

    //初始化这个bubble的函数
    public void Initialize(string fullText, Transform target, float durationPerLine)
    {
        followTarget = target;
        textMesh.text = "";

        // 打字机动画：每字一个 DOTween 延迟
        Sequence seq = DOTween.Sequence();
        for (int i = 0; i <= fullText.Length; i++)
        {
            string sub = fullText.Substring(0, i);
            seq.AppendCallback(() => textMesh.text = sub);
            seq.AppendInterval(typeSpeed);
        }

        Destroy(gameObject, durationPerLine); // 仍按总时间销毁
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