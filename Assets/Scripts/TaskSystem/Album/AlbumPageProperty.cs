using System;
using System.Collections;
using System.Collections.Generic;
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
    private AlbumPageProperty albumPageProperty;
    private Material photoMaterial;

    public AlbumPage(AlbumPageProperty _albumPageProperty, Texture2D photoTexture)
    {
        albumPageProperty = _albumPageProperty;
        photoTexture = photoTexture;
    }

    public void SetPhotoImage(Material photoMaterial)
    {
        this.photoMaterial = photoMaterial;
    }
    
    public AlbumPageProperty GetAlbumPageProperty() { return albumPageProperty; }

    public Material GetCurrentPhotoMaterial()
    {
        return photoMaterial;
    }
}