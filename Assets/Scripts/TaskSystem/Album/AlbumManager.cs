using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AlbumManager : SingletonMonoBase<AlbumManager>
{
    [Header("相片列表")] 
    [SerializeField] private List<AlbumPageProperty> pageProperties = new List<AlbumPageProperty>();
    private List<AlbumPage> pages;
    
    protected void Awake()
    {
        base.Awake();
        Initialized();
    }

    private void Initialized()
    {
        pages = new List<AlbumPage>();
    }

    private AlbumPageProperty GetPagePropertyByTaskIndex(int taskIndex)
    {
        foreach (AlbumPageProperty pageProperty in pageProperties)
        {
            if (pageProperty.GetTaskIndex() == taskIndex)
            {
                return pageProperty;
            }
        }
        return null;
    }

    public AlbumPage GetPageByTaskIndex(int taskIndex)
    {
        foreach (AlbumPage page in pages)
        {
            if (page.GetAlbumPageProperty().GetTaskIndex() == taskIndex)
            {
                return page;
            }
        }
        return null;
    }
    
    public void AddPageByTaskIndex(int taskIndex, Image image)
    {
        AlbumPageProperty pageProperty = GetPagePropertyByTaskIndex(taskIndex);
        if (pageProperty == null)
        {
            return;
        }

        AlbumPage page = new AlbumPage(pageProperty, image);
        pages.Add(page);
    }
    
    public void SetPageImageByTaskIndex(int taskIndex, Image image)
    {
        AlbumPage page = GetPageByTaskIndex(taskIndex);
        if (page == null)
        {
            Debug.LogError(string.Format("Page[{0}] is null!", taskIndex));
            return;
        }
        page.SetPhotoImage(image);
    }
}
