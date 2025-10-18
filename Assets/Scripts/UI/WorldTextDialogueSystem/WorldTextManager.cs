using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTextManager : MonoBehaviour
{
    //这里是主要调用显示的接口

    public static WorldTextManager Instance;
    public GameObject textBubblePrefab; // 拖入 TextMeshBubble 预制体
    public float durationPerLine = 2.5f;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowDialogue(Transform target, List<string> lines, System.Action onComplete = null)
    {
        StartCoroutine(ShowLinesCoroutine(target, lines, onComplete));
    }

    private IEnumerator ShowLinesCoroutine(Transform target, List<string> lines, System.Action onComplete)
    {
        foreach (string line in lines)
        {
            GameObject go = Instantiate(textBubblePrefab);
            var bubble = go.GetComponent<WorldTextBubble>();
            bubble.Initialize(line, target, durationPerLine);
            yield return new WaitForSeconds(durationPerLine);
        }

        // 所有播放完毕，触发回调
        onComplete?.Invoke();
    }
}
