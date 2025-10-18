using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class AlbumPageProperty 
{
    [SerializeField] private string taskId;
    [SerializeField] private string pageDescription;

    public AlbumPageProperty(string taskId, string pageDescription)
    {
        this.taskId = taskId;
        this.pageDescription = pageDescription;
    }

    public string GetTaskId() { return this.taskId; }
    
    public string GetPageDescription()
    {
        return pageDescription;
    }
}
[Serializable]
public class AlbumPage
{
    [SerializeField] private AlbumPageProperty albumPageProperty;
    [SerializeField] private Material photoMaterial;

    public AlbumPage(AlbumPageProperty _albumPageProperty,Material photoMaterial)
    {
        albumPageProperty = _albumPageProperty;
        this.photoMaterial = photoMaterial;
    }

    public void SetPhotoImage(Material photoMaterial)
    {
        this.photoMaterial = photoMaterial;
    }
    
    public AlbumPageProperty GetAlbumPageProperty() { return albumPageProperty; }

    public Material GetPhotoMaterial()
    {
        return photoMaterial;
    }
}