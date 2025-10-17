using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraDetect : MonoBehaviour
{
    
    [SerializeField] private AudioClip detectAudio;
    [SerializeField] private Transform detectUIPrefab;

    [SerializeField] private TaskItem requestTaskItem;
    private CapsuleCollider triggerCollider; // 胶囊体触发器


    [SerializeField] private Camera targetCamera; // 要捕获画面的相机（如主相机）
    [SerializeField] private Image targetImage; // 显示画面的UI图片
    //[SerializeField] private RenderTexture renderTexture; // 步骤1创建的渲染纹理

    [SerializeField] private Transform PlayerUI;
    private void Awake()
    {
       

    }

    private void OnEnable()
    {
        SceneManager.Instance().OnWorldStateChange += ResetDetectUI;
        CheckTrigger();


    }

    private void CheckTrigger()
    {
        triggerCollider = GetComponent<CapsuleCollider>();
        Vector3 center = transform.TransformPoint(triggerCollider.center); // 胶囊体中心（世界坐标）
        float height = triggerCollider.height; // 胶囊体高度（包含两个半球）
        float radius = triggerCollider.radius; // 胶囊体半径

        // 胶囊体的轴向（CapsuleCollider默认Y轴，可通过direction修改：0=X，1=Y，2=Z）
        Vector3 axis = Vector3.up; // 默认Y轴
        if (triggerCollider.direction == 0) axis = Vector3.right;
        else if (triggerCollider.direction == 2) axis = Vector3.forward;

        // 计算胶囊体的两个端点（世界坐标）
        Vector3 point1 = center + axis * (height / 2 - radius); // 上端点
        Vector3 point2 = center - axis * (height / 2 - radius); // 下端点

        // 手动检测胶囊体内的所有碰撞体
        Collider[] overlappedColliders = Physics.OverlapCapsule(
            point1,       // 胶囊体上端点
            point2,       // 胶囊体下端点
            radius,       // 胶囊体半径
            ~0            // 检测所有层级（可自定义层级掩码）
        );

        // 遍历检测到的碰撞体，模拟触发OnTriggerEnter
        foreach (var col in overlappedColliders)
        {
            if (col != triggerCollider) // 排除自身碰撞体
            {
                OnTriggerEnter(col); // 调用触发逻辑
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        TaskItem taskItem;
        if (other.TryGetComponent<TaskItem>(out taskItem))
        {
            if (taskItem == requestTaskItem)
            {
                Debug.Log("Detected" + other.gameObject.name);
                // 显示UI


                detectUIPrefab.gameObject.SetActive(true);
                //TODO: 播放音效，UI
            }

        }

    }

    private void OnDisable()
    {
        detectUIPrefab.gameObject.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        TaskItem taskItem;
        if (other.TryGetComponent<TaskItem>(out taskItem))
        {
            
        }
    }

    private void ResetDetectUI(WorldState worldState)
    {
        detectUIPrefab.gameObject.SetActive(false);
        CheckTrigger();
    }

    public void OutputPhoto()
    {
        StartCoroutine(OutputPhotoIEnumerator());
    }
    public IEnumerator OutputPhotoIEnumerator()
    {
             PlayerUI.gameObject.SetActive(false);
            // 初始化：设置相机的目标渲染纹理
            yield return null;

        int originalCullingMask = targetCamera.cullingMask;
            targetCamera.cullingMask = originalCullingMask & ~(1 << LayerMask.NameToLayer("UI"));

            RenderTexture tempRenderTexture = new RenderTexture(Screen.width, Screen.height, 24);
            targetCamera.targetTexture = tempRenderTexture;
            targetCamera.Render();
            yield return new WaitForEndOfFrame();

            Texture2D photoTexture = new Texture2D(
            tempRenderTexture.width,
            tempRenderTexture.height,
            TextureFormat.RGB24,
            false
            );

            photoTexture.ReadPixels(
            new Rect(0, 0, tempRenderTexture.width, tempRenderTexture.height),
            0, 0
             );
            photoTexture.Apply();
            Material photoMaterial = new Material(Shader.Find("Unlit/Texture"));
            photoMaterial.mainTexture = photoTexture;
            targetImage.material = photoMaterial;

             targetCamera.cullingMask = originalCullingMask;





        //// 创建一个使用该渲染纹理的材质，并赋值给UI图片
        //Material displayMaterial = new Material(Shader.Find("Unlit/Texture"));
        //displayMaterial.mainTexture = renderTexture;
        //targetImage.material = displayMaterial;

        targetCamera.targetTexture = null;

        PlayerUI.gameObject.SetActive(true);

    }
    //private void ShowPositionUI(Vector2 screenPos)
    //{
    //    // 销毁已存在的UI
    //    if (currentUI != null)
    //        Destroy(currentUI.gameObject);

    //    // 实例化UI预制体
    //    currentUI = Instantiate(detectUIPrefab, canvas.transform);
    //    RectTransform uiRect = currentUI.GetComponent<RectTransform>();

    //    // 1. 处理Canvas为Screen Space - Overlay的情况（最常见）
    //    // 转换Y轴：屏幕坐标Y（原点左下）→ UI坐标Y（原点左上）
    //    float uiY = Screen.height - screenPos.y;

    //    // 2. 处理UI锚点（假设锚点为中心，需修正偏移）
    //    // 若UI锚点是中心，需减去自身一半尺寸，避免位置偏移
    //    Vector2 uiPivotOffset = new Vector2(
    //        uiRect.rect.width * uiRect.pivot.x,
    //        uiRect.rect.height * uiRect.pivot.y
    //    );

    //    // 最终UI位置 = 转换后的屏幕坐标 - 锚点偏移
    //    Vector2 uiPosition = new Vector2(screenPos.x, uiY) - uiPivotOffset;

    //    // 设置UI位置
    //    uiRect.anchoredPosition = uiPosition;
    //}
}
