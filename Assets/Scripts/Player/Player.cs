using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening; // <--- 添加这一行
public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private bool canWalk = true;
    [SerializeField] private bool canTurn = true;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CameraDetect cameraDetect;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private Transform buttons;
    [SerializeField] private Vector3 squatDownOffset;
    private bool isSquatDown = false;

    private RectTransform buttonsRectTransform; // 用于操作UI位置
    private Vector2 buttonsInitialPosition; // 存储按钮的初始屏幕位置
    [SerializeField] private float buttonsAnimationDuration = 0.5f; // 动画时长
    [SerializeField] private float buttonsOffScreenOffsetY = -200f; // 按钮在屏幕下方时的Y轴偏移量

    [SerializeField] private Animator cameraAnimator;

    private float xRotation = 0f;

    private bool isCameraOn = false;

    private void Awake()
    {

        if (cameraTransform == null)
        {
            cameraTransform = transform.Find("MainCamera");
        }
        cameraDetect.OutPutToCamera();

        // --- 新增代码：获取并存储按钮初始位置 ---
        if (buttons != null)
        {
            // 假设buttons是一个UI元素，它拥有RectTransform组件
            buttonsRectTransform = buttons.GetComponent<RectTransform>();
            if (buttonsRectTransform != null)
            {
                buttonsInitialPosition = buttonsRectTransform.anchoredPosition;
            }
            else
            {
                Debug.LogError("Player.cs: 'buttons' does not have a RectTransform component. Animation will not work.");
            }
        }
        // --- 新增代码结束 ---

        //初始关闭相机
        CloseCamera();
    }

    private void Update()
    {

        // 处理E键切换鼠标状态
        if (Input.GetKeyDown(KeyCode.E))
        {

            if (isCameraOn)
            {
                CloseCamera();
            }
            else
            {
                OpenCamera();
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (isSquatDown)
            {
                SquatUp();
            }
            else
            {
                SquatDown();
            }
        }

        // 只有鼠标锁定时才允许视角旋转（可选逻辑，根据需求调整）
        if (canTurn && !isCameraOn)
        {
            HandleMouseLook();
        }

        if (canWalk && !isCameraOn)
        {
            HandleMovement();
        }
    }

    private void SquatDown()
    {
        cameraShake.StopShake();
        cameraTransform.position += squatDownOffset;
        isSquatDown = true;
    }
    private void SquatUp()
    {
        cameraShake.StopShake();
        cameraTransform.position -= squatDownOffset;
        isSquatDown = false;
    }


    // 设置鼠标状态（锁定/解锁 + 显示/隐藏）
    private void SetCursorState(bool locked)
    {
        if (locked)
        {

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void OpenCamera()
    {
        isCameraOn = true;
        SetCursorState(true);
        cameraDetect.gameObject.SetActive(true);

        // --- 修改部分：使用DOTween播放按钮进入动画 ---
        if (buttonsRectTransform != null)
        {
            buttons.gameObject.SetActive(true); // 先激活对象才能播放动画
            // 定义屏幕外的位置
            Vector2 offScreenPosition = new Vector2(buttonsInitialPosition.x, buttonsOffScreenOffsetY);
            // 立即将按钮设置到屏幕外
            buttonsRectTransform.anchoredPosition = offScreenPosition;
            // 使用DOTween从屏幕外移动到初始位置
            buttonsRectTransform.DOAnchorPos(buttonsInitialPosition, buttonsAnimationDuration).SetEase(Ease.OutCubic);
        }
        else
        {
            // 如果没有RectTransform，则使用旧的逻辑
            buttons.gameObject.SetActive(true);
        }
        // --- 修改结束 ---

        cameraAnimator.SetBool("isCameraOn", true);
        cameraShake.StopShake();
        //TODO:播放相机动画
    }

    public void CloseCamera()
    {
        isCameraOn = false;
        SceneManager.Instance().WorldStateChangeToOld();
        SetCursorState(false);
        cameraDetect.gameObject.SetActive(false);

        // --- 修改部分：使用DOTween播放按钮退出动画 ---
        if (buttonsRectTransform != null && buttons.gameObject.activeInHierarchy)
        {
            // 定义屏幕外的位置
            Vector2 offScreenPosition = new Vector2(buttonsInitialPosition.x, buttonsOffScreenOffsetY);
            // 播放动画移动到屏幕外，并在动画结束后禁用对象
            buttonsRectTransform.DOAnchorPos(offScreenPosition, buttonsAnimationDuration)
                .SetEase(Ease.InCubic)
                .OnComplete(() => {
                    buttons.gameObject.SetActive(false);
                });
        }
        else
        {
            // 如果没有RectTransform或者对象已经关闭，则使用旧的逻辑
            buttons.gameObject.SetActive(false);
        }
        // --- 修改结束 ---

        cameraAnimator.SetBool("isCameraOn", false);
        //TODO:播放相机动画
    }


    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
        moveDirection.Normalize();

        transform.position += moveDirection * Time.deltaTime * speed;

        if (moveDirection != Vector3.zero)
        {
            cameraShake.StartShake();
        }
        else
        {
            cameraShake.StopShake();
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (cameraTransform != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(cameraTransform.position, 0.2f);
        }
    }

    public CameraDetect GetCameraDetect()
    {
        return cameraDetect;
    }

    public bool GetIsCameraOn()
    { 
        return isCameraOn;
    }
}