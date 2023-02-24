using System;
using UnityEngine;

public class EnemyScript : MonoBehaviour, IDamagable
{
    PlayerController player;
    [HideInInspector] public Rigidbody rb {get; set;}

    // in retrospection this would be better handled by a scriptable object

    // ## UPGRADEABLE VARS ##
    private float movementSpeed;
    private float attackDamage;
    public float maxHealth {get; set;}
    private float size;
    // there's also weight value
    // GameManager.instance.enemyWeight is controlling rb.mass

    // ## END UPGRADEABLE VARS ##
    public float currentHealth {get; set;} 

    public int enemyLevel;

    private float distance;
    Vector3 direction;

    public static event Action OnGainXP;
    public event Action OnDeath;
    public event Action OnDamaged;

    private void Awake() {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Start() {
        player = PlayerController.instance;
        UpdateStats();
        currentHealth = maxHealth;

    }

    public void TakeDamage(float damage){
        Debug.Log($"current hp {currentHealth}, max {maxHealth} gonna take {damage} damage");
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
        if (transform.position.y < -20){
            Die();
        }

        distance = Vector3.Distance(transform.position, player.transform.position);
        direction = player.transform.position - transform.position;
        direction.Normalize();
        
        rb.AddForce(direction*movementSpeed, ForceMode.Impulse);

        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
    }

    private void Die() {
        OnGainXP?.Invoke();
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    private void UpdateStats() {
        movementSpeed = GameManager.instance.enemySpeed;
        maxHealth = GameManager.instance.enemyHP;
        attackDamage = GameManager.instance.enemyAttack;
        size = GameManager.instance.enemySize;
        transform.localScale = new Vector3(size,size,size);
        rb.mass = GameManager.instance.enemyWeight;
    }

    // Aerial: on collision damage is boring as fuck but I'm too shit of a programmer to write anything better
    private void OnCollisionStay(Collision other) {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if(player != null){
            player.TakeDamage(attackDamage);
        }
    }

}