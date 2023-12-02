using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    Transform cameraTarget;
    [SerializeField]
    float smoothTime = 1F;
    Vector3 velocity = Vector3.zero;

    void Update()
    {
        if (Mathf.Abs(transform.position.y - cameraTarget.position.y) > Mathf.Epsilon) {
            transform.position = Vector3.SmoothDamp(transform.position, cameraTarget.position, ref velocity, smoothTime * Time.timeScale);
        }
    }
}