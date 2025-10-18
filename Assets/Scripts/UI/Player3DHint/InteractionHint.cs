using UnityEngine;

public class InteractionHint : MonoBehaviour
{
    public GameObject hintObject;

    private void Start()
    {
        if (hintObject != null)
            hintObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && hintObject != null)
        {
            hintObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && hintObject != null)
        {
            hintObject.SetActive(false);
        }
    }
}
