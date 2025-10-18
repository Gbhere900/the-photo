using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image targetImage;
    public Sprite normalSprite;
    public Sprite selectedSprite;

    public AudioSource audioSource;
    public AudioClip hoverClip;
    public AudioClip clickClip;

    [Header("Scaling Settings")]
    public float scaleUpFactor = 1.1f;
    public float scaleDuration = 0.1f;

    private Vector3 normalBaseScale;
    private Vector3 selectedBaseScale;
    private Coroutine scaleCoroutine;

    void Start()
    {
        normalBaseScale = transform.localScale;
        targetImage.sprite = normalSprite;

        if (normalSprite != null && selectedSprite != null && normalSprite.rect.width > 0)
        {
            float compensationX = selectedSprite.rect.width / normalSprite.rect.width;
            float compensationY = selectedSprite.rect.height / normalSprite.rect.height;
            selectedBaseScale = new Vector3(normalBaseScale.x * compensationX, normalBaseScale.y * compensationY, normalBaseScale.z);
        }
        else
        {
            selectedBaseScale = normalBaseScale;
            Debug.LogWarning("Button sprites are not set or have zero size. Compensation scale not calculated.", this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverClip != null) audioSource.PlayOneShot(hoverClip);
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);

        // --- 核心修改 ---
        // 1. 立即切换图片
        targetImage.sprite = selectedSprite;
        // 2. 立即应用补偿缩放，消除“抽搐”
        transform.localScale = selectedBaseScale;
        // 3. 从补偿后的状态，平滑放大到最终目标
        scaleCoroutine = StartCoroutine(ScaleTo(selectedBaseScale * scaleUpFactor));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);

        // --- 核心修改 ---
        // 1. 立即切换回普通图片
        targetImage.sprite = normalSprite;
        // 2. 立即恢复为普通缩放，同样为了消除“抽搐”
        transform.localScale = normalBaseScale;
        // 3. (可选) 如果你希望退出时也有一个平滑缩小的动画，可以从一个稍大的尺寸开始播放
        //    但通常来说，鼠标移开时，立即恢复是更常见的交互。
        //    如果你想要平滑退出，请看下面的方案二。当前这个版本是立即恢复。
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickClip != null) audioSource.PlayOneShot(clickClip);
    }

    private IEnumerator ScaleTo(Vector3 target)
    {
        Vector3 start = transform.localScale;
        float time = 0;
        while (time < scaleDuration)
        {
            transform.localScale = Vector3.Lerp(start, target, time / scaleDuration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = target;
    }
}