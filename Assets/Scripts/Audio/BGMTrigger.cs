using UnityEngine;

public class BGMTrigger : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            BGMController.instance.PlaySceneBGM();
            triggered = true;
        }
    }
}