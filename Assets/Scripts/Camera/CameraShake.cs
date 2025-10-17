using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("晃动参数")]
    [SerializeField] private AnimationCurve shakeCurve; // 晃动曲线（控制幅度变化）
    [SerializeField] private float shakeFrequency = 10f; // 晃动频率（每秒次数）
    [SerializeField] private float shakeIntensity = 0.02f; // 晃动幅度
    [SerializeField] private Transform cameraTransform; // 要晃动的相机

    private Vector3 originalLocalPos; // 相机初始局部位置
    private float shakeTime; // 晃动计时器
    private bool isShaking; // 是否正在晃动

    private void Awake()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        // 记录相机初始位置（用于晃动后复位）
        originalLocalPos = cameraTransform.localPosition;
    }

    // 外部调用：开始晃动（例如在角色移动时调用）
    public void StartShake()
    {
        if (isShaking == false)
        {
            isShaking = true;
            shakeTime = 0;
        }

    }

    // 外部调用：停止晃动（例如角色停止移动时调用）
    public void StopShake()
    {
        isShaking = false;
        // 复位相机位置
        //cameraTransform.localPosition = originalLocalPos;
    }

    private void Update()
    {
        if (isShaking)
        {
            ShakeCamera();
        }
    }

    private void ShakeCamera()
    {
        // 累加时间，结合频率计算周期
        shakeTime += Time.deltaTime * shakeFrequency;

        // 从曲线获取当前幅度比例（曲线Y轴范围建议0~1）
        float curveValue = shakeCurve.Evaluate(shakeTime % 1f); // 取0~1之间的循环值

        // 计算X和Y轴的随机晃动偏移（基于正弦函数使晃动更平滑）
        float offsetX = Mathf.Sin(shakeTime * 2) * curveValue * shakeIntensity;
        float offsetY = Mathf.Cos(shakeTime * 1.5f) * curveValue * shakeIntensity;

        // 应用晃动（在初始位置基础上叠加偏移）
        cameraTransform.localPosition = originalLocalPos + new Vector3(offsetX, offsetY, 0);
    }
}