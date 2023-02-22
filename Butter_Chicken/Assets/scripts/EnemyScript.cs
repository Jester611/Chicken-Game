using System;
using UnityEngine;

public class EnemyScript : MonoBehaviour, IDamagable
{
    PlayerController player;
    [HideInInspector] public Rigidbody rb {get; set;}
    [SerializeField] float maxHP;
    [SerializeField] float movementSpeed;

    private float currentHP;
    private float distance;
    Vector3 direction;

    public static event Action OnEnemyKilled;
    public event Action OnKilled;

    private void Awake() {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Start() {
        player = PlayerController.instance;
        movementSpeed = GlobalStats.instance.enemySpeed;
        maxHP = GlobalStats.instance.enemyHP;
        currentHP = maxHP;
    }

    public void TakeDamage(float damageTaken) {
        currentHP -= damageTaken;
        if(currentHP <= 0) Die();
    }
    private void OnEnable() {
        LevelUpScript.OnUpdateStats += UpdateStats;
    }
    private void OnDisable() {
        LevelUpScript.OnUpdateStats -= UpdateStats;
    }

    private void FixedUpdate() {
        distance = Vector3.Distance(transform.position, player.transform.position);
        direction = player.transform.position - transform.position;
        direction.Normalize();

        rb.AddForce(direction * movementSpeed, ForceMode.VelocityChange);

        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
    }

    private void Die() {
        OnEnemyKilled?.Invoke();
        OnKilled?.Invoke();
        Destroy(gameObject);
    }

    private void UpdateStats() {
        if(GlobalStats.instance != null){
            movementSpeed = GlobalStats.instance.enemySpeed;
            maxHP = GlobalStats.instance.enemyHP;
        }
    }

}
