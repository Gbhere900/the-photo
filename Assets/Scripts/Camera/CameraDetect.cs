using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraDetect : MonoBehaviour
{

    [SerializeField] private AudioClip detectAudio;
    //[SerializeField] private Transform detectUI;
    [SerializeField] private Transform checkPassUI;

   //[SerializeField] private TaskItem requestTaskItem;
    private CapsuleCollider triggerCollider; // 胶囊体触发器


    [SerializeField] private Camera targetCamera; // 要捕获画面的相机（如主相机）
    [SerializeField] private Image targetImage_Photo; // 显示画面的UI图片
    public  Material currentPhotoMaterial;

    [SerializeField] private Camera secondaryCamera;
    [SerializeField] private Material displayMaterial;
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private Image targetImage_Camera;

    [SerializeField] private QuickOutline quickOutline;
    

    public  bool currentTaskDone = false;
    private void Awake()
    {
        
        
    }

    private void OnEnable()
    {
        SceneManager.Instance().OnWorldStateChange += ResetDetectUI;
        if (CheckTaskItemInTrigger())
        {
            quickOutline.enabled = true;
           // detectUI.gameObject.SetActive(true);
            //TODO: ������Ч
        }


    }
    private void OnDisable()
    {
        if (quickOutline)
        {
            quickOutline.enabled = false;
        }
        //if (detectUI)
        //{
        //    detectUI.gameObject.SetActive(false);
        //}

        
        SceneManager.Instance().OnWorldStateChange -= ResetDetectUI;
    }

    //private void CheckTrigger()
    //{
    //    triggerCollider = GetComponent<CapsuleCollider>();
    //    Vector3 center = transform.TransformPoint(triggerCollider.center); // 胶囊体中心（世界坐标）
    //    float height = triggerCollider.height; // 胶囊体高度（包含两个半球）
    //    float radius = triggerCollider.radius; // 胶囊体半径

    //    // 胶囊体的轴向（CapsuleCollider默认Y轴，可通过direction修改：0=X，1=Y，2=Z）
    //    Vector3 axis = Vector3.up; // 默认Y轴
    //    if (triggerCollider.direction == 0) axis = Vector3.right;
    //    else if (triggerCollider.direction == 2) axis = Vector3.forward;

    //    // 计算胶囊体的两个端点（世界坐标）
    //    Vector3 point1 = center + axis * (height / 2 - radius); // 上端点
    //    Vector3 point2 = center - axis * (height / 2 - radius); // 下端点

    //    // 手动检测胶囊体内的所有碰撞体
    //    Collider[] overlappedColliders = Physics.OverlapCapsule(
    //        point1,       // 胶囊体上端点
    //        point2,       // 胶囊体下端点
    //        radius,       // 胶囊体半径
    //        ~0            // 检测所有层级（可自定义层级掩码）
    //    );

    //    // 遍历检测到的碰撞体，模拟触发OnTriggerEnter
    //    foreach (var col in overlappedColliders)
    //    {
    //        if (col != triggerCollider) // 排除自身碰撞体
    //        {
    //            OnTriggerEnter(col); // 调用触发逻辑
    //        }
    //    }
    //}

    private bool CheckTaskItemInTrigger()
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
                TaskItem taskItem;
                if (col.TryGetComponent<TaskItem>(out taskItem))
                {
                    
                    if (taskItem == TaskSystemManager.Instance.GetCurrentTask().GetTaskItem())
                    {
                        quickOutline = taskItem.GetComponent<QuickOutline>();
                        return true;

                    }

                }
            }
        }
        return false;
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    TaskItem taskItem;
    //    if (other.TryGetComponent<TaskItem>(out taskItem))
    //    {
    //        if (taskItem == requestTaskItem)
    //        {
    //            Debug.Log("Detected" + other.gameObject.name);
    //            // 显示UI


    //            detectUIPrefab.gameObject.SetActive(true);
    //            //TODO: 播放音效，UI
    //        }

    //    }

    //}



    private void OnTriggerExit(Collider other)
    {
        TaskItem taskItem;
        if (other.TryGetComponent<TaskItem>(out taskItem))
        {

        }
    }



    private void ResetDetectUI(WorldState worldState)
    {
        //detectUI.gameObject.SetActive(false);
        if (quickOutline!= null)
        {
            quickOutline.enabled = false;
        }
       
        if (CheckTaskItemInTrigger())
        {
<<<<<<< Updated upstream
            //detectUI.gameObject.SetActive(true);
            // ��ʾUI

            quickOutline.enabled = true;
=======
            detectUI.gameObject.SetActive(true);
            // 显示UI
>>>>>>> Stashed changes
        }
    }

    public void OutputToPhoto()
    {
        Tagofphoto.Instance.settrue();
        StartCoroutine(OutputToPhotoIEnumerator());

        if (CheckTaskItemInTrigger())
        {
            checkPassUI.gameObject.SetActive(true);
            currentTaskDone = true;
            //TODO: 播放音效，UI
        }
        else
        {
            checkPassUI.gameObject.SetActive(false);
            currentTaskDone = false;
            //TODO: 播放音效，UI
        }
    }
    public IEnumerator OutputToPhotoIEnumerator()
    {

        //float timer
        quickOutline.enabled = false;
      
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

        RenderTexture.active = tempRenderTexture;

        photoTexture.ReadPixels(
        new Rect(0, 0, tempRenderTexture.width, tempRenderTexture.height),
        0, 0
         );
        photoTexture.Apply();

        RenderTexture.active = null;
        currentPhotoMaterial = new Material(Shader.Find("Unlit/Texture"));
        currentPhotoMaterial.mainTexture = photoTexture;
        
        targetImage_Photo.material = currentPhotoMaterial;



        //// 创建一个使用该渲染纹理的材质，并赋值给UI图片
        //Material displayMaterial = new Material(Shader.Find("Unlit/Texture"));
        //displayMaterial.mainTexture = renderTexture;
        //targetImage.material = displayMaterial;

        targetCamera.targetTexture = null;
        secondaryCamera.targetTexture = renderTexture;

        quickOutline.enabled = true;
    }

    private void OnDestroy()
    {
        //// 清理：恢复相机默认渲染目标（避免场景切换后相机画面异常）
        //if (targetCamera != null)
        //    targetCamera.targetTexture = null;

        //// 销毁动态创建的材质（避免内存泄漏）
        //if (targetImage_Photo != null && targetImage_Photo.material != null)
        //    Destroy(targetImage_Photo.material);
    }


    public  void OutPutToCamera()
    {


        // 自动获取组件（如果未在Inspector赋值）
        if (secondaryCamera == null)
        {
            Debug.LogError("secondaryCamera未赋值");
        }

        if (targetImage_Camera == null)
            targetImage_Camera = GetComponent<Image>();

        // 初始化：设置相机的目标渲染纹理
        if (renderTexture != null)
        {


            // 创建一个使用该渲染纹理的材质，并赋值给UI图片
            //Material displayMaterial = new Material(Shader.Find("Unlit/Texture"));
            displayMaterial.mainTexture = renderTexture;
            //targetImage_Camera.material = displayMaterial;
        }
        else
        {
            Debug.LogError("请赋值RenderTexture！");
        }

    }

    public void ResetPhoto()
    {
        checkPassUI.gameObject.SetActive(false);
        currentTaskDone = false;

    }

}