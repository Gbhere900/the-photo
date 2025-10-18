using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AlbumManager : SingletonMonoBase<AlbumManager>
{
    [Header("相片列表")]
    // [SerializeField] private List<AlbumPageProperty> pageProperties = new List<AlbumPageProperty>();
    [SerializeField] private List<AlbumPage> pages;
    [SerializeField] private Transform albumCanvas;
    [SerializeField] private Image photoImage;
    [SerializeField] private TextMeshProUGUI description;

    private int currentPageIndex = 0;

    protected void Awake()
    {
        base.Awake();
        Initialized();
        //ShowPage();
        //ChangePage(currentPageIndex);
    }

    private void Initialized()
    {
        pages = new List<AlbumPage>(10);
       // ChangePage(currentPageIndex);
    }

    private AlbumPage GetPageByIndex(int taskIndex)
    {
        if (taskIndex >= pages.Count)
        {
            Debug.LogWarning("Index超过相册容量，返回null");
            return null;

        }

        return pages[taskIndex];

    }

    public AlbumPage GetPageByTaskId(string taskId)
    {
        foreach (AlbumPage page in pages)
        {
            if (page.GetAlbumPageProperty().GetTaskId() == taskId)
            {
                return page;
            }
        }
        Debug.LogWarning("未找到id为" + taskId + ",返回null");
        return null;
    }

    public void InsertPage(string taskId, string pageDescription, Texture2D photoTexture, int index)
    {
        AlbumPageProperty albumPageProperty = new AlbumPageProperty(taskId, pageDescription);
        AlbumPage page = new AlbumPage(albumPageProperty, photoTexture);
        pages[index] = page;
    }

    public void ShowPage()
    {

        albumCanvas.gameObject.SetActive(true);
    }
    public void ChangePage(int index)
    {
        description.text = GetPageByIndex(index).GetAlbumPageProperty().GetPageDescription();

        photoImage.material = GetPageByIndex(index).GetCurrentPhotoMaterial();
    }

    public void HidePage()
    {
        albumCanvas.gameObject.SetActive(false);
    }

    public void OnNextPageButtonDown()
    {
        NextPage();

        //TODO:PlayAudio
    }

    public void OnLastPageButtonDown()
    {
        LastPage();
        //TODO:PlayAudio
    }

    private void NextPage()
    {
        currentPageIndex++;
        ChangePage(currentPageIndex);
    }

    private void LastPage()
    {
        currentPageIndex--;
        ChangePage(currentPageIndex);
    }
}
