using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    //public string taskId; // 当前任务编号
    private bool isPlayerInRange = false;
    private Player player;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            CameraDetect cameraDetect = player.GetCameraDetect();
            if (cameraDetect.currentTaskDone)
            {
                TaskSystemManager.Instance.SetCurrentTaskCompleted();
                Task task = TaskSystemManager.Instance.GetCurrentTask();
                AlbumManager.Instance.AddPage(task.GetTaskId(), task.GetTaskDescription(), cameraDetect.currentPhotoMaterial);
                cameraDetect.ResetPhoto();
            }

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
        {
            isPlayerInRange = true;
            player = other.GetComponent<Player>();
        }
            
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerInRange = false;
    }
}
