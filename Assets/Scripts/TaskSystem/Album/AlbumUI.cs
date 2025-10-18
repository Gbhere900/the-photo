using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlbumUI : MonoBehaviour
{
    [SerializeField] private Image photoImage;
    [SerializeField] private TextMeshProUGUI description;

    public void ChangePage(string description,Material material)
    {
       
        this.description.text = description;

        photoImage.material = material;
    }

    private void OnEnable()
    {
        //ChangePage();
    }
}
