using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlbumTestScript : MonoBehaviour
{
    private int currentIndex = 1;
    void Start()
    {
        AlbumManager.Instance.AddPageByTaskIndex(1, null);
        AlbumManager.Instance.AddPageByTaskIndex(2, null);
        AlbumManager.Instance.AddPageByTaskIndex(3, null);
        AlbumManager.Instance.AddPageByTaskIndex(4, null);
        AlbumManager.Instance.AddPageByTaskIndex(5, null);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            AlbumPage page = AlbumManager.Instance.GetPageByTaskIndex(currentIndex++);
            Debug.Log(page.GetAlbumPageProperty().GetPageDescription());

            if (currentIndex > 5)
            {
                currentIndex = 1;
            }
        }
    }
}
