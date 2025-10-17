using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraDetect : MonoBehaviour
{

    [SerializeField] private AudioClip detectAudio;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Transform detectUIPrefab;

    private Transform currentUI;


    private void OnEnable()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        TaskItem taskItem;
        if (other.TryGetComponent<TaskItem>(out taskItem))
        {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(other.transform.position);

            // 显示UI
            ShowPositionUI(screenPos);
            //TODO: 播放音效，UI
        }

    }

    private void OnTriggerExit(Collider other)
    {
        TaskItem taskItem;
        if (other.TryGetComponent<TaskItem>(out taskItem))
        {
            if (currentUI != null)
            {
                Destroy(currentUI.gameObject);
                currentUI = null;
            }
            //TODO: 播放音效，UI
        }
    }

    private void ShowPositionUI(Vector2 screenPos)
    {
        // 销毁已存在的UI（避免重复显示）
        if (currentUI != null)
            Destroy(currentUI.gameObject);

        // 实例化UI预制体到Canvas下
        currentUI = Instantiate(detectUIPrefab, canvas.transform);

        // 获取UI的RectTransform组件，用于设置位置
        RectTransform uiRect = currentUI.GetComponent<RectTransform>();

        // 设置UI在屏幕上的位置（注意：ScreenPoint的原点在屏幕左下角，UI锚点需对应）
        // 若UI锚点为中心，需偏移UI自身尺寸的一半（这里简化处理，假设锚点在左下角）
        uiRect.anchoredPosition = screenPos;

    }
}
