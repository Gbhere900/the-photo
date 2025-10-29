using TMPro;
using UnityEngine;

public class InteractiveTooltip : SingletonMonoBase<InteractiveTooltip>
{
    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private TextMeshProUGUI desText;
    [SerializeField] private Vector3 positionOffset;

    private RectTransform tooltipRect;

    void Start()
    {
        tooltipRect = GetComponent<RectTransform>();
        this.gameObject.SetActive(false);
    }

    public void ShowTooltip(string key, string description, Vector3 position)
    {
        keyText.text = key;
        desText.text = description;
        tooltipRect.position = position + positionOffset;
        this.gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        this.gameObject.SetActive(false);
    }

    public string GetDescriptionText()
    {
        return desText.text;
    }

    public bool IsTooltipActive()
    {
        return this.gameObject.activeSelf;
    }
}
