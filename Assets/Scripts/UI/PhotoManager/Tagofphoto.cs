using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Tagofphoto : MonoBehaviour
{
    public static Tagofphoto Instance { get; private set; }

    public bool hasphoto = false;

    private void Awake()
    {
        // 单例初始化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 如果需要跨场景
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void setfalse()
    {
        hasphoto = false;
    }
    public void settrue()
    {
        hasphoto = true;
    }

}