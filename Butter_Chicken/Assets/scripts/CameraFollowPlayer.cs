using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    // Old script wasn't good enough
    //
    // [SerializeField] private GameObject camPos;
    // [SerializeField] private GameObject cam;
    // [SerializeField] private float speed;

    // private void Update() {
    //     cam.transform.position = Vector3.Lerp(cam.transform.position, camPos.transform.position, Time.deltaTime * speed);
    // }

public Transform player;
public Vector3 offset = new Vector3(-20,20,-20);
private void Start()
{
    transform.rotation = Quaternion.Euler(30f, 45f, 0);
}
void LateUpdate()
{
    Vector3 target = new Vector3(player.position.x, 0, player.position.z) + offset;
    transform.position = target;
}
}
