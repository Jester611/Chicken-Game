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

    [SerializeField] private int xpValue;
    [SerializeField] private float statmultiplier;
    private float distance;
    Vector3 direction;
    public static event Action<int> OnGainXP;
    private bool isAlreadyDead = false; //prevents getting multiple exp for multiple bullets killing same enemy on a single frame

    private void Awake() {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Start() {
        player = PlayerController.instance;
        UpdateStats();
        currentHealth = maxHealth;
    }

    private void OnEnable() {
        LevelUpScript.OnUpdateStats += UpdateStats;
    }
    private void OnDisable() {
        LevelUpScript.OnUpdateStats -= UpdateStats;
    }

    private void FixedUpdate() {
        if (transform.position.y < -2 || transform.position.y > 8){
            Die();
        }

        distance = Vector3.Distance(transform.position, player.transform.position);
        direction = player.transform.position - transform.position;
        direction.Normalize();
        
        rb.AddForce(direction*movementSpeed, ForceMode.Impulse);

        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
    }
    public void TakeDamage(float damage){
        currentHealth -= damage;
        if(currentHealth <= 0 && !isAlreadyDead) {
            isAlreadyDead = true;
            Die();
        }
    }

    public void Die() {
        OnGainXP?.Invoke(xpValue);
        Destroy(gameObject);
    }

    private void UpdateStats() {
        movementSpeed = GameManager.instance.enemySpeed;
        maxHealth = GameManager.instance.enemyHP * statmultiplier;
        attackDamage = GameManager.instance.enemyAttack * statmultiplier;
        size = GameManager.instance.enemySize * statmultiplier;
        transform.localScale = new Vector3(size,size,size);
        rb.mass = GameManager.instance.enemyWeight * statmultiplier;
    }

    private void OnCollisionStay(Collision other) {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if(player != null && gameObject != null){ // lmao
            player.TakeDamage(attackDamage);
        }
    }

}