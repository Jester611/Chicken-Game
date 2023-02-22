using System;
using UnityEngine;

public class EnemyScript : MonoBehaviour, IDamagable
{
    PlayerController player;
    [HideInInspector] public Rigidbody rb {get; set;}
    [SerializeField] float movementSpeed;

    public float maxHealth {get; set;}
    public float currentHealth {get; set;} 

    private float distance;
    Vector3 direction;



    public static event Action OnEnemyKilled;
    public event Action OnDeath;
    public event Action OnDamaged;

    private void Awake() {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Start() {
        player = PlayerController.instance;
        movementSpeed = GlobalStats.instance.enemySpeed;
        maxHealth = GlobalStats.instance.enemyHP;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageTaken) {
        currentHealth -= damageTaken;
        if(currentHealth <= 0) Die();
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
        OnDeath?.Invoke();
        OnEnemyKilled?.Invoke();
        Destroy(gameObject);
    }

    private void UpdateStats() {
        movementSpeed = GlobalStats.instance.enemySpeed;
        maxHealth = GlobalStats.instance.enemyHP;
    }

}
