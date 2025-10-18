using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Album Page", menuName = "TaskSystem/AlbumPageProperty")]
public class AlbumPageProperty : ScriptableObject
{
    [SerializeField] private int taskIndex;
    [SerializeField] private string pageDescription;

    public int GetTaskIndex() { return this.taskIndex; }
    
    public string GetPageTaskName()
    {
        Task pageTask = TaskSystemManager.Instance.GetTaskByIndex(taskIndex);
        if (pageTask == null)
        {
            return string.Empty;
        }
        return pageTask.GetTaskId();
    }

    public string GetPageDescription()
    {
        return pageDescription;
    }
}

public class AlbumPage
{
    private AlbumPageProperty albumPageProperty;
    private Image image;

    public AlbumPage(AlbumPageProperty _albumPageProperty, Image _image)
    {
        albumPageProperty = _albumPageProperty;
        image = _image;
    }

    public void SetPhotoImage(Image _image)
    {
        this.image = _image;
    }
    
    public AlbumPageProperty GetAlbumPageProperty() { return albumPageProperty; }
}