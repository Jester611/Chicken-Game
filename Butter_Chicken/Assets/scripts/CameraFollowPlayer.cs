using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject camPos;
    [SerializeField] private GameObject cam;
    [SerializeField] private float speed;

    private void Update()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, camPos.transform.position, Time.deltaTime*speed);
    }
}
