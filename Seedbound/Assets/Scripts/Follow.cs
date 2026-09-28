using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform objectToFollow;
    public float speed = 3f;

    void Update()
    {
        if (objectToFollow == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            objectToFollow.position,
            speed * Time.deltaTime
        );
    }
}
