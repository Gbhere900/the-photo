using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private bool canWalk = true;
    [SerializeField] private bool canTurn = true;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CameraDetect cameraDetect;

    private float xRotation = 0f;

    private bool isCameraOn = false;

    private void Awake()
    {

        if (cameraTransform == null)
        {
            cameraTransform = transform.Find("MainCamera");
        }

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
            if (!isCameraOn)
            {
                OpenCamera();
            }
        }

        // 只有鼠标锁定时才允许视角旋转（可选逻辑，根据需求调整）
        if (canTurn && !isCameraOn)
        {
            HandleMouseLook();
        }

        if (canWalk)
        {
            HandleMovement();
        }
    }

    // 设置鼠标状态（锁定/解锁 + 显示/隐藏）
    private void SetCursorState(bool locked)
    {
        if (locked)
        {
            // 锁定鼠标到屏幕中心并隐藏
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            // 解锁鼠标并显示
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void OpenCamera()
    {
        isCameraOn = true;
        SetCursorState(false);
        cameraDetect.enabled = true;
        //TODO:播放相机动画
    }

    public void CloseCamera()
    {
        isCameraOn = false;
        SetCursorState(true);
        cameraDetect.enabled = false;
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

    }

    private void OnDrawGizmosSelected()
    {
        if (cameraTransform != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(cameraTransform.position, 0.2f);
        }
    }
}