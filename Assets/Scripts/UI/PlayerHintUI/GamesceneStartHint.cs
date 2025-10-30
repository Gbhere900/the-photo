using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamesceneStartHint : MonoBehaviour
{
    public TaskHintController hint;
    // Start is called before the first frame update
    void Start()
    {
        hint.ShowHint("靠近老者 按F与之交谈");
    }


}
