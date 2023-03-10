using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    [SerializeField] GameObject waveRoom;
    [SerializeField] GameObject bossRoom;
    [SerializeField] GameObject doorsToOpen;
    [SerializeField] GameObject roomTrigger;
    [SerializeField] GameObject transitionNorthHideable;
    [SerializeField] GameObject transitionWaveRoomCollider;
    [SerializeField] GameObject transitionBossRoomCollider;
    [SerializeField] GameObject bossObject;

    public static event System.Action OnBeginBossFight;



    private void OnEnable() {
        AEnemySpawner.OnKillGoal += SceneTransition1;
        RoomTrigger.OnPlayerEnterBossRoom += SceneTransition2;
        //on boss kill scene transition 3??
    }

    private void OnDisable() {
        AEnemySpawner.OnKillGoal -= SceneTransition1;
        RoomTrigger.OnPlayerEnterBossRoom -= SceneTransition2;
    }
    private void SceneTransition1(){
        bossRoom.SetActive(true);
        doorsToOpen.transform.Rotate(0,-90f,0,Space.World);
        transitionBossRoomCollider.SetActive(false);
        transitionWaveRoomCollider.SetActive(false);
        transitionNorthHideable.SetActive(false);
        roomTrigger.SetActive(true);
        Debug.Log("scene transition 1 triggered");

    }

    private void SceneTransition2(){
        transitionBossRoomCollider.SetActive(true);
        doorsToOpen.transform.Rotate(0,90f,0,Space.World);
        Destroy(waveRoom);
        Debug.Log("scene transition 2 triggered");
        bossObject.SetActive(true);
        OnBeginBossFight?.Invoke();
    }
}
