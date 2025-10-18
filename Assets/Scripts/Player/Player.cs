using Unity.VisualScripting;
ďťż// Player.cs (ćçťäżŽć­Łç - UIćˇĄĺĽćˇĄĺş)
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("çŠĺŽść§ĺś")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private bool canWalk = true;
    [SerializeField] private bool canTurn = true;
    [SerializeField] private float mouseSensitivity = 2f;

    [Header("ĺŻščąĄĺźç¨")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CameraDetect cameraDetect;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private Transform buttons;
    [SerializeField] private Vector3 squatDownOffset;
    private bool isSquatDown = false;

    [SerializeField] private Animator cameraAnimator;

    [Header("ç¸ćşUI")] // <<< äżŽćšďźç°ĺ¨ç´ćĽĺźç¨ButtonsĺĺŽçCanvasGroup
    [Tooltip("ĺĺŤććç¸ćşćéŽççśĺŻščąĄ")]
    [SerializeField] private GameObject buttonsObject;
    [Tooltip("ćč˝˝ĺ¨ButtonsĺŻščąĄä¸çCanvasGroupďźç¨äşć§ĺśćˇĄĺĽćˇĄĺş")]
    [SerializeField] private CanvasGroup buttonsCanvasGroup;
    [Tooltip("UIćˇĄĺĽ/ćˇĄĺşçćçť­ćśé´")]
    [SerializeField] private float uiFadeDuration = 0.4f;

    private float xRotation = 0f;
    private bool isCameraOn = false;
    private bool isTransitioning = false; // çśćéďźé˛ć­˘ĺ¨ĺ¨çťćé´éĺ¤č§Śĺ

    private void Awake()
    {
        if (cameraTransform == null)
        {
            cameraTransform = transform.Find("MainCamera");
        }

        // <<< äżŽćšďźćŁćĽć°çĺźç¨
        if (buttonsObject == null || buttonsCanvasGroup == null)
        {
            Debug.LogError("čŻˇĺ¨ Inspector ä¸­čŽžç˝Ž Buttons Object ĺ Buttons Canvas Group!", this);
            this.enabled = false;
            return;
        }

        cameraDetect.OutPutToCamera();

        // ĺĺ§ćśĺ˝ťĺşéčUI
        buttonsCanvasGroup.alpha = 0f;
        buttonsObject.SetActive(false);
        // ĺĺ§čŽžç˝Žéź ć ĺç¸ćşçść
        SetCursorState(false);
        cameraAnimator.SetBool("isCameraOn", false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isTransitioning)
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

        if (!isCameraOn && !isTransitioning)
        {
            HandleMouseLook();
        }

        if (canWalk&&!isCameraOn)
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


    // ÉčÖĂĘóąę×´ĚŹŁ¨Ëřś¨/˝âËř + ĎÔĘž/Ňţ˛ŘŁŠ
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
            if (canTurn) HandleMouseLook();
            if (canWalk) HandleMovement();
        }
    }

    public void OpenCamera()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        // 1. çŤĺłĺć˘çŠĺŽśçśćĺç¸ćşĺ¨çť
        isCameraOn = true;
        SetCursorState(true);
        cameraDetect.gameObject.SetActive(true);
        cameraAnimator.SetBool("isCameraOn", true);
        cameraShake.StopShake();

        // 2. ĺĺ¤UIĺ¨çť
        buttonsObject.SetActive(true); // ĺćżć´ťGameObjectćč˝ć­ćžĺ¨çť
        buttonsCanvasGroup.alpha = 0f;   // çĄŽäżäťĺ¨éćĺźĺ§

        // 3. ć­ćžUIćˇĄĺĽĺ¨çť
        buttonsCanvasGroup.DOFade(1f, uiFadeDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => {
                isTransitioning = false;
                buttonsCanvasGroup.interactable = true; // ĺ¨çťçťćĺĺčŽ¸äş¤äş
            });
    }

    public void CloseCamera()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        buttonsCanvasGroup.interactable = false; // ĺ¨çťĺźĺ§ćśçŚć­˘äş¤äş

        // 1. ć­ćžUIćˇĄĺşĺ¨çť
        buttonsCanvasGroup.DOFade(0f, uiFadeDuration)
            .SetEase(Ease.InQuad)
            .OnComplete(() => {
                // 2. ĺ¨çťçťćĺďźĺć˘çŠĺŽśçśćĺšśĺ˝ťĺşéčUI
                isCameraOn = false;
                SetCursorState(false);
                cameraDetect.gameObject.SetActive(false);
                buttonsObject.SetActive(false); // ĺ¨çťćžĺŽĺéčGameObject
                cameraAnimator.SetBool("isCameraOn", false);
                isTransitioning = false;
            });
    }

    // čŽžç˝Žéź ć çśćďźéĺŽ/č§Łé + ćžç¤ş/éčďź
    private void SetCursorState(bool unlocked)
    {
        Cursor.lockState = unlocked ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = unlocked;
    }

    // --- äťĽä¸ćšćłäżćä¸ĺ ---
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
        if (moveDirection != Vector3.zero) { cameraShake.StartShake(); }
        else { cameraShake.StopShake(); }
    }
    private void OnDrawGizmosSelected() { /*...*/ }
    public CameraDetect GetCameraDetect() { return cameraDetect; }

    public bool IsCameraOn() { return isCameraOn; }

    public CameraDetect GetCameraDetect()
    {
        return cameraDetect;
    }

    public bool GetIsCameraOn()
    {
        return isCameraOn;
    }
}