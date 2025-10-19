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
    [SerializeField] private Transform TheEnd;
    string RTL = "<rotate=90>";
    public void ChangePage(string description,Material material)
    {
       
        

        if (material != null)
        {
            photoImage.gameObject.SetActive(true);
            photoImage.material = material;
        }
        else
        {
            photoImage.gameObject.SetActive(false);
        }
        if (description.Length == 0)
        {
            TheEnd.gameObject.SetActive(true);
        }
        else
        {
            TheEnd.gameObject.SetActive(false);
        }

        this.description.text = RTL + description + RTL;

        photoImage.material = material;
    }

    public void ChangePage(AlbumPage page)
    {
        Material material = page.GetPhotoMaterial();
        string description = page.GetAlbumPageProperty().GetPageDescription();

        this.description.text = RTL + description + RTL;
        
        if (material != null)
        {
            photoImage.gameObject.SetActive(true);
            photoImage.material = material;
        }
        else
        {
            photoImage.gameObject.SetActive(false);
        }
        if (description == "")
        {
            TheEnd.gameObject.SetActive(true);
        }
        else
        {
            TheEnd.gameObject.SetActive(false);
        }

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
