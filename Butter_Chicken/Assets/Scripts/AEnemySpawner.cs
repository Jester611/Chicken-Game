using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AEnemySpawner : MonoBehaviour
{
    public static AEnemySpawner instance  { get; private set; } //yay singletons
    //make mama proud

    [SerializeField] Transform[] enemySpawnPoints = new Transform[7];
    [SerializeField] Enemy[] enemyList = new Enemy[2];


    // ## WAVE BUDGET ##
    [SerializeField] int startingPoints;
    int budgetPoints;
    private List<GameObject> enemiesToSpawn = new List<GameObject>();

    // ## INTERVALS ##
    [SerializeField] private float spawnIntervalDuration;
    [SerializeField] private float waveIntervalDuration;
    private float waveInterval;

    // ## GOAL ##
    private bool gameStarted = false;
    [SerializeField] private int presentEnemies = 0; // serialized for debug
    private bool currentlySpawningEnemies = false;
    private int killNumber = 0;
    [SerializeField] private int killGoal = 100;
    private bool killGoalAchieved = false;
    private bool eventInvoked = false;

    public static event System.Action OnKillGoal;

    private void Start() {
        enemySpawnPoints = GetComponentsInChildren<Transform>();
        budgetPoints = startingPoints;
        Invoke(nameof(StartGame), 2f);
    }

    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }

    private void OnEnable() {
        EnemyScript.OnGainXP += IncreaseKillNumber;
    }

    private void OnDisable() {
        EnemyScript.OnGainXP -= IncreaseKillNumber;
    }
    private void FixedUpdate() {
        if(gameStarted){
            if((presentEnemies <= 1 || waveInterval <= 0) && !killGoalAchieved && !currentlySpawningEnemies){
                GenerateWave();
            }
            else if (killGoalAchieved && presentEnemies == 0 && eventInvoked == false){
                OnKillGoal?.Invoke();
                eventInvoked = true;
            }
            else{
                waveInterval -= Time.fixedDeltaTime;
            }
        }
    }
    private void StartGame(){
        gameStarted = true;
    }

    private void GenerateWave(){
        waveInterval = waveIntervalDuration;
        float wavevalue = budgetPoints * Random.Range(0.85f, 1.15f);
        PickEnemies((int)wavevalue);
        StartCoroutine(SpawnEnemies(false));
        budgetPoints += 1;
    }

    public IEnumerator GenerateBossWave(){
        PickEnemies(5);
        yield return SpawnEnemies(true);
    }

    private void PickEnemies(int points){
        while(points > 0){
            // this is silly, but what it actually does is cause type 2 enemy have half spawn rate of type 1 enemy
            int random = Random.Range(2,5);
            if(random % 2 == 0){
                enemiesToSpawn.Add(enemyList[0].prefab);
                points -= enemyList[0].pointCost;
            }
            else{
                enemiesToSpawn.Add(enemyList[1].prefab);
                points -= enemyList[1].pointCost;
            }
        }
    }

    private IEnumerator SpawnEnemies(bool bosswave){
        currentlySpawningEnemies = true;
        if (enemiesToSpawn.Any()){
            for (int i = 0; i < enemiesToSpawn.Count; i++){
                int random = Random.Range(1, enemySpawnPoints.Count());
                if(!bosswave){
                    GameObject.Instantiate(enemiesToSpawn[i],enemySpawnPoints[random].position, Quaternion.identity);
                }
                else{
                    GameObject.Instantiate(enemiesToSpawn[i], BossScript.instance.gunPoint.position, Quaternion.identity);
                }
                presentEnemies++;
                yield return new WaitForSeconds(spawnIntervalDuration*Random.Range(0.5f, 1.5f));
            }
            enemiesToSpawn.Clear();
            currentlySpawningEnemies = false;
        }
        yield return true;
    }

    private void IncreaseKillNumber(int ignored){
        killNumber++;
        presentEnemies--;
        if (killNumber >= killGoal){
            killGoalAchieved = true;
        }
    }
}

[System.Serializable]
public class Enemy{
    public GameObject prefab;
    public int pointCost;
}
