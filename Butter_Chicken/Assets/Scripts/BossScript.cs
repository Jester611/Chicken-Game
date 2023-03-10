using System.Collections;
using UnityEngine;

public class BossScript : MonoBehaviour, IDamagable
{
    public static BossScript instance { get; private set; }

    PlayerController player;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject deadBossPuddle;


    public Transform gunPoint;
    [HideInInspector] public Rigidbody rb {get; set;}
    private BoxCollider bossCollider;
    [SerializeField] private float maxHP = 500f; // i fucking hate having to work around that interface
    public float maxHealth {get; set;}
    public float currentHealth {get; set;}

    [SerializeField] int longBurstSize = 9;
    [SerializeField] int wideBurstSize = 6;
    private bool nextBurstFour = false;
    private int pastRandom = 0;
    private bool bossdead = false;
    public static event System.Action OnBossKill;

    private void OnEnable() {
        RoomTransition.OnBeginBossFight += BeginBossfight;
    }

    private void OnDisable() {
        RoomTransition.OnBeginBossFight -= BeginBossfight;
    }
    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
        maxHealth = maxHP;
    }
    public void TakeDamage(float damage){
        currentHealth -= damage;
        GameManager.instance.UpdateBossHealthBar();
        if(currentHealth <= 0 && !bossdead){
            BossDies();
            bossdead = true; //prevent boss death routine triggered multiple times
        }
    }
    public void BeginBossfight(){
        bossCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        Debug.Log("boss script triggered");
        StartCoroutine(Bossfight());
    }

    private IEnumerator Bossfight(){
        int random;
        while (currentHealth > 0){
            do{
            random = Random.Range(1,4);
            }while(random == pastRandom);
            pastRandom = random;
            Debug.Log($"Rolled a {random}");
            switch (random){
                case 1:{ 
                    yield return BurstFire();
                    continue;
                }
                case 2:{
                    yield return SprayFire();
                    continue;
                }
                case 3:{
                    yield return SpawnEnemies();
                    continue;
                }
            }
        }
    }

    private IEnumerator BurstFire(){
        Vector3 aimPoint = Vector3.zero;
        Vector3 playerloc = Vector3.zero;
        for (int i = 0; i < longBurstSize; i++){
            playerloc = PlayerController.instance.transform.position;
            aimPoint = new Vector3(playerloc.x, gunPoint.position.y, playerloc.z);
            gunPoint.LookAt(aimPoint);

            GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);

            Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());

            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds((3f * (currentHealth/maxHealth))+ 0.2f);
    }
    private IEnumerator SprayFire(){
        gunPoint.rotation = Quaternion.identity;
        for (int i = 0; i < wideBurstSize; i++){
            if (nextBurstFour){
                SprayFour();
                nextBurstFour = false;
            }
            else{
                SprayThree();
                nextBurstFour = true;
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds((3f * (currentHealth/maxHealth))+ 0.1f);
    }
    private IEnumerator SpawnEnemies(){
        yield return AEnemySpawner.instance.GenerateBossWave();
        yield return new WaitForSeconds((4f * (currentHealth/maxHealth))+ 0.5f);
    }

    // here comes the ugly part
    private void SprayThree(){
    gunPoint.Rotate(0,180,0);
    GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
    Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());

    gunPoint.Rotate(0f, 30f, 0f);
    bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
    Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());

    gunPoint.Rotate(0f, -60f, 0f);
    bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
    Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());

    gunPoint.rotation = Quaternion.identity;
    }

    private void SprayFour(){
    gunPoint.Rotate(0,180,0);
    gunPoint.Rotate(0f, -45f, 0f);
    GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
    Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());

    gunPoint.Rotate(0f, 30f, 0f);
    bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
    Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());

    gunPoint.Rotate(0f, 30f, 0f);
    bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
    Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());

    gunPoint.Rotate(0f, 30f, 0f);
    bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
    Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());

    gunPoint.rotation = Quaternion.identity;
    }

    private void BossDies(){

        StopAllCoroutines();

        float endSceneTime = 3f;
        StartCoroutine(DeathShake(endSceneTime));
        StartCoroutine(DeathBubbles(endSceneTime));

        OnBossKill?.Invoke();
    }

    private IEnumerator DeathBubbles(float bubbleDuration){
        float bubbleTimer = 0f;
        float delayBetweenBubbles = 0.04f;
        while(bubbleTimer < bubbleDuration){
            bubbleTimer += Time.deltaTime;
            Vector3 loc = GetRandomPointInCollider();
            GameObject explosionFX = Instantiate(explosionPrefab, loc, Quaternion.identity);
            Destroy(explosionFX, 0.4f);
            yield return new WaitForSeconds(delayBetweenBubbles);
        }
    }

    private IEnumerator DeathShake(float shakeDuration)
	{
        Vector3 startPos = transform.position;
        Vector3 randomPos = transform.position;
		float shakeTimer = 0f;
        float delayBetweenShakes = 0.03f;
        float shakeDistance = 0.1f;

		while (shakeTimer < shakeDuration)
		{
			shakeTimer += Time.deltaTime;

			randomPos = startPos + (Random.insideUnitSphere * shakeDistance);

			transform.position = randomPos;
			
            yield return new WaitForSeconds(delayBetweenShakes);

		}
		transform.position = startPos;
	}

    private Vector3 GetRandomPointInCollider(){
        return bossCollider.bounds.center + new Vector3(
           (Random.value - 0.5f) * bossCollider.bounds.size.x,
           (Random.value - 0.5f) * bossCollider.bounds.size.y,
           (Random.value - 0.5f) * bossCollider.bounds.size.z);
    }

    private void OnDestroy() {
        StopAllCoroutines();
        deadBossPuddle.SetActive(true);
    }

    private void OnCollisionEnter(Collision other) {
        player = other.gameObject.GetComponent<PlayerController>();
        if(player != null){
            Vector3 knockback = transform.forward * 10f;
            player.rb.AddForce(knockback, ForceMode.Impulse);
            player.TakeDamage(10f);
        }
    }
}
