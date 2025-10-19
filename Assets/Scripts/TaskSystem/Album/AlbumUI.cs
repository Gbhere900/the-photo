using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlbumUI : MonoBehaviour
{
    [SerializeField] private Image photoImage;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] Animator animator;
    string RTL = "<rotate=90>";
    public void ChangePage(string description,Material material)
    {
       
        this.description.text = RTL + description + RTL;

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
            animator.Play("Show");
        }

    }
}
