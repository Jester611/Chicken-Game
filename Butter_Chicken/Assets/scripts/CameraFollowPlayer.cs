using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    PlayerController player;
    public Vector3 offset = new Vector3(-20, 20, -20);
    private void Start() {
        player = PlayerController.instance;
        transform.rotation = Quaternion.Euler(30f, 45f, 0);
    }
    void LateUpdate() {
        Vector3 target = new Vector3(player.transform.position.x, 0, player.transform.position.z) + offset;
        transform.position = target;
    }
}
