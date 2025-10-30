using System;
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
    private CapsuleCollider triggerCollider; // �����崥����


    [SerializeField] private Camera targetCamera; // Ҫ���������������������
    [SerializeField] private Image targetImage_Photo; // ��ʾ�����UIͼƬ
    public Material currentPhotoMaterial;

    [SerializeField] private Camera secondaryCamera;
    [SerializeField] private Material displayMaterial;
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private Image targetImage_Camera;

    [SerializeField] private QuickOutline quickOutline;

    [SerializeField] private Shader unlitTextureShader;


    public bool currentTaskDone = false;
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
    //    Vector3 center = transform.TransformPoint(triggerCollider.center); // ���������ģ��������꣩
    //    float height = triggerCollider.height; // ������߶ȣ�������������
    //    float radius = triggerCollider.radius; // ������뾶

    //    // �����������CapsuleColliderĬ��Y�ᣬ��ͨ��direction�޸ģ�0=X��1=Y��2=Z��
    //    Vector3 axis = Vector3.up; // Ĭ��Y��
    //    if (triggerCollider.direction == 0) axis = Vector3.right;
    //    else if (triggerCollider.direction == 2) axis = Vector3.forward;

    //    // ���㽺����������˵㣨�������꣩
    //    Vector3 point1 = center + axis * (height / 2 - radius); // �϶˵�
    //    Vector3 point2 = center - axis * (height / 2 - radius); // �¶˵�

    //    // �ֶ���⽺�����ڵ�������ײ��
    //    Collider[] overlappedColliders = Physics.OverlapCapsule(
    //        point1,       // �������϶˵�
    //        point2,       // �������¶˵�
    //        radius,       // ������뾶
    //        ~0            // ������в㼶�����Զ���㼶���룩
    //    );

    //    // ������⵽����ײ�壬ģ�ⴥ��OnTriggerEnter
    //    foreach (var col in overlappedColliders)
    //    {
    //        if (col != triggerCollider) // �ų�������ײ��
    //        {
    //            OnTriggerEnter(col); // ���ô����߼�
    //        }
    //    }
    //}

    private bool CheckTaskItemInTrigger()
    {
        triggerCollider = GetComponent<CapsuleCollider>();
        Vector3 center = transform.TransformPoint(triggerCollider.center); // ���������ģ��������꣩
        float height = triggerCollider.height; // ������߶ȣ�������������
        float radius = triggerCollider.radius; // ������뾶

        // �����������CapsuleColliderĬ��Y�ᣬ��ͨ��direction�޸ģ�0=X��1=Y��2=Z��
        Vector3 axis = Vector3.up; // Ĭ��Y��
        if (triggerCollider.direction == 0) axis = Vector3.right;
        else if (triggerCollider.direction == 2) axis = Vector3.forward;

        // ���㽺����������˵㣨�������꣩
        Vector3 point1 = center + axis * (height / 2 ); // �϶˵�
        Vector3 point2 = center - axis * (height / 2 ); // �¶˵�

        // �ֶ���⽺�����ڵ�������ײ��
        Collider[] overlappedColliders = Physics.OverlapCapsule(
            point1,       // �������϶˵�
            point2,       // �������¶˵�
            radius,       // ������뾶
            ~0            // ������в㼶�����Զ���㼶���룩
        );

        // ������⵽����ײ�壬ģ�ⴥ��OnTriggerEnter
        foreach (var col in overlappedColliders)
        {
            if (col != triggerCollider) // �ų�������ײ��
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
        quickOutline = null;
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
    //            // ��ʾUI


    //            detectUIPrefab.gameObject.SetActive(true);
    //            //TODO: ������Ч��UI
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
        if (quickOutline != null)
        {
            quickOutline.enabled = false;
        }

        if (CheckTaskItemInTrigger())
        {
            //detectUI.gameObject.SetActive(true);
            // ��ʾUI

            quickOutline.enabled = true;
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
            //TODO: ������Ч��UI
        }
        else
        {
            checkPassUI.gameObject.SetActive(false);
            currentTaskDone = false;
            //TODO: ������Ч��UI
        }

        if (currentTaskDone)
        {

            TaskSystemManager.Instance.CheckTaskEvent();
        }
    }
    public IEnumerator OutputToPhotoIEnumerator()
    {

        //float timer
        if (quickOutline)
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
        currentPhotoMaterial = new Material(unlitTextureShader);
        currentPhotoMaterial.mainTexture = photoTexture;

        targetImage_Photo.material = currentPhotoMaterial;



        //// ����һ��ʹ�ø���Ⱦ�����Ĳ��ʣ�����ֵ��UIͼƬ
        //Material displayMaterial = new Material(Shader.Find("Unlit/Texture"));
        //displayMaterial.mainTexture = renderTexture;
        //targetImage.material = displayMaterial;

        targetCamera.targetTexture = null;
        secondaryCamera.targetTexture = renderTexture;

        if (currentTaskDone)
        {
            if (TaskSystemManager.Instance.GetCurrentTask().GetTaskId() == "OldMan")
            {

                AlbumManager.Instance.AddPage(TaskSystemManager.Instance.GetCurrentTask().GetTaskId(), TaskSystemManager.Instance.GetCurrentTask().GetTaskDescription(), currentPhotoMaterial, TaskSystemManager.Instance.GetCurrentTaskIndex());
            }
        }

        if (quickOutline)
            quickOutline.enabled = true;


    }

    private void OnDestroy()
    {
        //// �������ָ����Ĭ����ȾĿ�꣨���ⳡ���л�����������쳣��
        //if (targetCamera != null)
        //    targetCamera.targetTexture = null;

        //// ���ٶ�̬�����Ĳ��ʣ������ڴ�й©��
        //if (targetImage_Photo != null && targetImage_Photo.material != null)
        //    Destroy(targetImage_Photo.material);
    }


    public void OutPutToCamera()
    {


        // �Զ���ȡ��������δ��Inspector��ֵ��
        if (secondaryCamera == null)
        {
            Debug.LogError("secondaryCameraδ��ֵ");
        }

        if (targetImage_Camera == null)
            targetImage_Camera = GetComponent<Image>();

        // ��ʼ�������������Ŀ����Ⱦ����
        if (renderTexture != null)
        {


            // ����һ��ʹ�ø���Ⱦ�����Ĳ��ʣ�����ֵ��UIͼƬ
            //Material displayMaterial = new Material(Shader.Find("Unlit/Texture"));
            displayMaterial.mainTexture = renderTexture;
            //targetImage_Camera.material = displayMaterial;
        }
        else
        {
            Debug.LogError("�븳ֵRenderTexture��");
        }

    }

    public void ResetPhoto()
    {
        checkPassUI.gameObject.SetActive(false);
        currentTaskDone = false;

    }

}