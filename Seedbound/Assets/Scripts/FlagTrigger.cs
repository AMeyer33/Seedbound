using UnityEngine;

public class FlagTrigger : MonoBehaviour
{
    [Header("Assign in Inspector")]
    [SerializeField] private GameObject objectToShow;
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag(playerTag))
        {
            if (objectToShow != null)
            {
                objectToShow.SetActive(true);
            }
        }
    }
}