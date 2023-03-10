using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public static event System.Action OnPlayerEnterBossRoom;

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<PlayerController>()){
            OnPlayerEnterBossRoom?.Invoke();
            Destroy(gameObject, 1f);
        }
    }
}
