using System;
using UnityEngine;

public class EnemyScript : MonoBehaviour, IDamagable
{
    PlayerController player;
    [HideInInspector] public Rigidbody rb {get; set;}
    [SerializeField] float maxHP;
    [SerializeField] float movementSpeed;
    [SerializeField] float attackDamage;

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
        currentHealth = maxHealth;
        UpdateStats();
    }

    public void TakeDamage(float damage){
        currentHealth -= damage;
        OnDamaged?.Invoke();
        if(currentHealth <= 0) {
            Die();
        }
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
        
        rb.AddForce(direction*movementSpeed, ForceMode.VelocityChange);

        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
    }

    private void Die() {
        OnEnemyKilled?.Invoke();
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    private void UpdateStats() {
        movementSpeed = GameManager.instance.enemySpeed;
        maxHP = GameManager.instance.enemyHP;
        attackDamage = GameManager.instance.enemyAttack;
    }

    // Aerial: on collision damage is boring as fuck but I'm too shit of a programmer to write anything better
    private void OnCollisionStay(Collision other) {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if(player != null){
            player.TakeDamage(attackDamage);
        }
    }

}