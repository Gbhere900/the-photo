// PhotoDisplayManager.cs (重构与优化版)
using DG.Tweening;
using UnityEngine;

/// <summary>
/// 负责管理单个照片的显示和隐藏动画。
/// 这是一个自包含的组件，封装了两种不同的展示动画流程。
/// </summary>
public class PhotoDisplayManager : MonoBehaviour
{
    [Header("对象引用")]
    [Tooltip("需要执行动画的照片对象")]
    public Transform photoObject;
    public TaskHintController hintController;

    [Header("动画位置参数")]
    [Tooltip("照片从屏幕外进入的相对偏移量。例如(0, -700, 0)表示从下方700单位处进入")]
    public Vector3 offScreenOffset = new Vector3(0, -700, 0);

    [Header("通用动画时序")]
    [Tooltip("照片进入屏幕所需时间")]
    public float showDuration = 1.2f;
    [Tooltip("照片在屏幕上停留显示的时间")]
    public float displayDuration = 3.0f;
    [Tooltip("照片移出屏幕所需时间")]
    public float hideDuration = 1.0f;

    [Header("特定动画效果")]
    [Tooltip("查看历史照片时的进入动画曲线（提供动态感）")]
    public Ease historyShowEase = Ease.OutCubic;

    // 私有状态变量
    private Vector3 onScreenPosition;            // 照片在屏幕上的最终位置
    private Vector3 offScreenPosition;           // 照片在屏幕外的起始/终点位置
    private Sequence currentPhotoSequence;       // 当前正在播放的完整动画序列

    private void Awake()
    {
        // 1. 检查引用是否设置
        if (photoObject == null)
        {
            Debug.LogError("请在 Inspector 中将照片对象拖拽到 Photo Object 字段！", this);
            this.enabled = false;
            return;
        }

        // 2. 初始化位置信息
        // 记录下照片在编辑器里摆放好的“屏幕内”目标位置
        onScreenPosition = photoObject.position;
        // 根据目标位置和偏移量计算出“屏幕外”的位置
        offScreenPosition = onScreenPosition + offScreenOffset;

        // 3. 确保初始状态是完全隐藏在屏幕外的
        photoObject.position = offScreenPosition;
    }

    // --- 公开接口 ---

    /// <summary>
    /// 场景1: 拍照后，以匀速慢速展示照片。
    /// </summary>
    public void ShowAfterCapture()
    {
        // 使用 Ease.Linear 来实现“匀速”效果
        PlayFullSequence(Ease.Linear);
    }

    /// <summary>
    /// 场景2: 查看前一张照片，以曲线速率展示。
    /// </summary>
    public void ShowFromHistory()
    {
        if (!Tagofphoto.Instance.hasphoto)
        {
            hintController.ShowHint("还未拍照");
            return;
        }    
        // 使用在 Inspector 中设置的曲线速率
        PlayFullSequence(historyShowEase);
    }

    /// <summary>
    /// 立即隐藏照片并停止所有动画。
    /// </summary>
    public void HideImmediately()
    {
        KillCurrentSequence();
        photoObject.position = offScreenPosition;
    }

    // --- 核心动画逻辑 ---

    /// <summary>
    /// 播放完整的“进入-等待-退出”动画序列。
    /// </summary>
    /// <param name="showEase">进入动画使用的缓动类型</param>
    private void PlayFullSequence(Ease showEase)
    {
        // 在播放新动画前，先确保旧的动画序列被彻底停止
        KillCurrentSequence();

        // 立即将照片置于屏幕外，作为动画的起始点
        photoObject.position = offScreenPosition;

        // 创建新的动画序列
        currentPhotoSequence = DOTween.Sequence();

        currentPhotoSequence
            // 步骤1: 照片从屏幕外移动到屏幕内
            .Append(photoObject.DOMove(onScreenPosition, showDuration).SetEase(showEase))

            // 步骤2: 在屏幕上停留指定时间
            .AppendInterval(displayDuration)

            // 步骤3: 照片从屏幕内移动回屏幕外（下滑离开）
            .Append(photoObject.DOMove(offScreenPosition, hideDuration).SetEase(Ease.InCubic))

            // 动画播放完毕后的收尾工作
            .OnComplete(() => {
                Debug.Log("照片完整动画播放完毕，已重置到屏幕外。");
                // 确保照片最终停留在屏幕外，为下一次调用做好准备
                photoObject.position = offScreenPosition;
            });
    }

    // --- 辅助方法 ---

    private void KillCurrentSequence()
    {
        if (currentPhotoSequence != null && currentPhotoSequence.IsActive())
        {
            currentPhotoSequence.Kill();
        }
    }

    private void OnDestroy()
    {
        // 好的编程习惯：在对象销毁时清理所有相关的DOTween动画
        KillCurrentSequence();
    }

    // --- 用于快速测试的 Update 函数 ---
    void Update()
    {
    }
}