using UnityEngine;

public class CrosshairScript : MonoBehaviour
{
    PlayerController player;
    void Start(){
        player = PlayerController.instance;
    }
    void LateUpdate()
    {
        Vector3 aimPoint = new Vector3(player.lookPoint.x, 0.64f, player.lookPoint.z);
        transform.position = aimPoint;
    }
}
