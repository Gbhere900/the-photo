using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(QuickOutline))]
public abstract class InteractiveObjectBase : MonoBehaviour
{
    [Header("��������")]
    [SerializeField] protected float interactiveDistance;
    [SerializeField] protected float cooldownTime;
    [SerializeField] protected bool isHighlight = false;
    [SerializeField] protected float outlineWidth = 10f;
    [SerializeField] protected string audioClip = "";
    [SerializeField] protected float audioDelay = 0f;
    [SerializeField] protected bool ifPlayAudio = false;
    
    [Header("��������")]
    [SerializeField] protected string name;

    private bool isIntersectingWithDetector = false;
    private bool isInSight = false;
    protected float lastInteractTime;
    protected GameObject player;
    protected Camera mainCamera;

    private QuickOutline outline;
    protected Coroutine audioCoroutine;
    
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        
        Initialized();
    }
    
    protected virtual void Update()
    {
        // �����߼�
        isHighlight = CanInteract;
        if (outline)
        {
            if (isHighlight)
            {
                outline.enabled = true;
            }
            else
            {
                outline.enabled = false;
            }
        }
        
        // ���߼��
        if (isIntersectingWithDetector)
        {
            RayDetect();
        }
        else
        {
            isInSight = false;
        }
        
        // UI������ʾ
        if (CanInteract)
        {
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(this.transform.position);
            InteractiveTooltip.Instance.ShowTooltip("I", name, screenPosition);
        }
        else
        {
            if (InteractiveTooltip.Instance.GetDescriptionText() == name && InteractiveTooltip.Instance.IsTooltipActive())
            {
                InteractiveTooltip.Instance.HideTooltip();   
            }
        }
        
        // ����������
        CheckPlayerInput();
    }
    
    public bool CanInteract
    {
        get
        {
            if (player)
            {
                Player playerScript = player.GetComponent<Player>();
                if (playerScript && playerScript.GetIsCameraOn())
                {
                    return false;
                }
            }
            // ������ȴʱ���ж�
            if (cooldownTime > 0 && Time.time < lastInteractTime + cooldownTime)
            {
                return false;
            }
            // ���������ж�
            if (!player || !isIntersectingWithDetector)
            {
                return false;
            }
            // ���߼��
            if (!isInSight)
            {
                return false;
            }

            return IsInteractionPossible();
        }
    }

    private void RayDetect()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        
        int playerLayer = LayerMask.NameToLayer("CameraModel");
        LayerMask layerMask = ~(1 << playerLayer);
        bool isHit = Physics.Raycast(ray, out hit, interactiveDistance, layerMask);
        // ����������
        if (isHit)
        {
            // �жϱ����е��Ƿ��ǿɽ������屾��
            if (hit.collider.gameObject == gameObject || hit.collider.transform.IsChildOf(transform))
            {
                isInSight = true;
            }
            else
            {
                isInSight = false;
            }
        }
    }
    
    /// <summary>
    /// �жϽ����Ƿ����
    /// </summary>
    /// <returns></returns>
    protected abstract bool IsInteractionPossible();

    /// <summary>
    /// ��ʼ��
    /// </summary>
    protected virtual void Initialized()
    {
        lastInteractTime = -cooldownTime;
        
        outline = GetComponent<QuickOutline>();
        if (outline)
        {
            outline.OutlineColor = Color.white;
            outline.OutlineWidth = outlineWidth;
            outline.OutlineMode = QuickOutline.Mode.OutlineVisible;
        }
    }

    /// <summary>
    /// ����������
    /// </summary>
    private void CheckPlayerInput()
    {
        // F������
        if (Input.GetKeyDown(KeyCode.I) && CanInteract)
        {
            Interact();
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    private void Interact()
    {
        if (!CanInteract)
        {
            Debug.LogError("Can't interact with this object");
            return;
        }
        
        // ִ�о��彻���߼�
        PerformInteraction();
        // ��¼����ʱ��
        lastInteractTime = Time.time;
    }

    /// <summary>
    /// ���彻���߼�
    /// </summary>
    protected virtual void PerformInteraction()
    {
        // ���Ž�����Ч
        if (ifPlayAudio && audioClip != string.Empty)
        {
            if (audioCoroutine != null)
            {
                StopCoroutine(audioCoroutine);
            }
            audioCoroutine = StartCoroutine(PlayAudioClip(audioClip, audioDelay));
        }
    }

    protected IEnumerator PlayAudioClip(string audioClip, float audioDelay)
    {
        yield return new WaitForSeconds(audioDelay);
        
        AudioManager.instance.Play(audioClip);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "InteractiveDetect")
        {
            isIntersectingWithDetector = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "InteractiveDetect")
        {
            isIntersectingWithDetector = false;
        }
    }
}
