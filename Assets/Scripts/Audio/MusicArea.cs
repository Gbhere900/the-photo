using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicArea : MonoBehaviour
{
    public string musicname;
    private void OnTriggerEnter(Collider other)
    {
        AudioManager.instance.Play(musicname);
    }
}
