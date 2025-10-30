using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("�ζ�����")]
    [SerializeField] private AnimationCurve shakeCurve; // �ζ����ߣ����Ʒ��ȱ仯��
    [SerializeField] private float shakeFrequency = 10f; // �ζ�Ƶ�ʣ�ÿ�������
    [SerializeField] private float xShakeIntensity = 0.02f; // �ζ�����
    [SerializeField] private float yShakeIntensity = 0.02f; // �ζ�����
    [SerializeField] private Transform cameraTransform; // Ҫ�ζ������

    private Vector3 originalLocalPos; // �����ʼ�ֲ�λ��
    private float shakeTime; // �ζ���ʱ��
    private bool isShaking; // �Ƿ����ڻζ�

    private void Awake()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        // ��¼�����ʼλ�ã����ڻζ���λ��
        originalLocalPos = cameraTransform.localPosition;
    }

    // �ⲿ���ã���ʼ�ζ��������ڽ�ɫ�ƶ�ʱ���ã�
    public void StartShake()
    {
        if (isShaking == false)
        {
            isShaking = true;
            shakeTime = 0;
            originalLocalPos = cameraTransform.localPosition;
        }

    }

    // �ⲿ���ã�ֹͣ�ζ��������ɫֹͣ�ƶ�ʱ���ã�
    public void StopShake()
    {
        isShaking = false;
        // ��λ���λ��
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
        // �ۼ�ʱ�䣬���Ƶ�ʼ�������
        shakeTime += Time.deltaTime * shakeFrequency;

        // �����߻�ȡ��ǰ���ȱ���������Y�᷶Χ����0~1��
        float curveValue = shakeCurve.Evaluate(shakeTime % 1f); // ȡ0~1֮���ѭ��ֵ

        // ����X��Y�������ζ�ƫ�ƣ��������Һ���ʹ�ζ���ƽ����
        float offsetX = Mathf.Sin(shakeTime * 2) * curveValue * xShakeIntensity;
        float offsetY = Mathf.Cos(shakeTime * 1.5f) * curveValue * yShakeIntensity;

        // Ӧ�ûζ����ڳ�ʼλ�û����ϵ���ƫ�ƣ�
        cameraTransform.localPosition = originalLocalPos + new Vector3(offsetX, offsetY, 0);
    }
}