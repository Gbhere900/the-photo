using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private bool isPlayerInRange = false;
    private Player player;

    // 1. 添加一个状态锁，用于防止在对话播放期间重复触发。
    //    默认为 false，表示当前没有对话在播放。
    private bool isDialoguePlaying = false;

    void Update()
    {
        // 2. 在触发条件中，增加对 isDialoguePlaying 的检查。
        //    只有当玩家在范围内、按下了F键、并且当前没有对话正在播放时，才执行。
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F) && !isDialoguePlaying)
        {
            // --- 立即上锁 ---
            // 在开始播放对话之前，立刻将锁设为 true，防止玩家在下一帧继续触发。
            isDialoguePlaying = true;

            // --- 这部分是你的原有逻辑，保持不变 ---
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

                // 3. 调用对话，并在最后一个参数传入一个回调函数。
                //    这个函数会在所有对话行都显示完毕后被 WorldTextManager 自动调用。
                WorldTextManager.Instance.ShowDialogue(transform, lines, () =>
                {
                    // --- 对话播放完毕，解锁 ---
                    // 当回调被执行时，意味着对话流程结束了，
                    // 此时我们将锁重新设为 false，允许玩家再次触发对话。
                    isDialoguePlaying = false;
                });
            }
            else
            {
                // 如果没有DialogueData组件，也应该解锁，防止永久锁定。
                isDialoguePlaying = false;
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
        {
            isPlayerInRange = false;
        }
        // 注意：玩家离开触发区时，我们不应该重置 isDialoguePlaying。
        // 因为即使玩家走开了，已经开始的对话也应该继续播放完。
        // 锁的状态只由对话的开始和结束来控制。
    }
}