using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    //public string taskId; // 当前任务编号
    private bool isPlayerInRange = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            var data = GetComponent<DialogueData>();
            if (data != null)
            {
                var lines = data.GetDialogue(TaskSystemManager.Instance.GetCurrentTask().GetTaskId());
                WorldTextManager.Instance.ShowDialogue(transform, lines);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerInRange = false;
    }
}
