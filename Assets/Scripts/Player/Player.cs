using Unity.VisualScripting;
using UnityEngine;

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
        buttons.gameObject.SetActive(true);
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
        buttons.gameObject.SetActive(false);
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