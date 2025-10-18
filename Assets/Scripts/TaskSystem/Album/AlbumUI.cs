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

    public void ChangePage(AlbumPage page)
    {
        this.description.text = page.GetAlbumPageProperty().GetPageDescription();
        photoImage.material =page.GetPhotoMaterial();
    }

    private void OnEnable()
    {
        if (!AlbumManager.Instance.IsPagesEmpty())
        {
            ChangePage(AlbumManager.Instance.GetCurrentPage());
        }

    }
}
