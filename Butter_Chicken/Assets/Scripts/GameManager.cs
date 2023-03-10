using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance  { get; private set; }
    public static event Action OnLevelUp;

    // ## MENUS ##
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject levelUpMenu;
    [SerializeField] GameObject deathScreen;
    [SerializeField] Image blackoutPanel;
    [SerializeField] GameObject winScreen;

    public static bool isPaused;

    // ## COUNTERS ##
    [SerializeField] Image healthBar;
    [SerializeField] Image expBar;
    [SerializeField] TextMeshProUGUI killCounter;
    int killCount = 0;

    // ## VARIABLES RELATED TO UPGRADES ##
    [Header("Bullet")]
    public float bulletDamage = 5;
    public float bulletExplosiveRadius = 0;
    public float bulletKnockback = 0;
    [Header("Weapon")]
    public float gunRateOfFire = 0.7f;
    public float gunBulletSpeed = 15f;
    public float gunRecoil = 0f;
    public float gunSpread = 0f;
    public int gunBurstSize = 1;
    [Header("Player")]
    public float playerSpeed = 1f;
    public float playerMaxHP = 50f;
    public float playerInvincibilityTimer = 0.4f;
    public float playerWeight = 1f;
    public float playerDrag = 8f;
    [Header("Enemy")]
    public float enemyHP = 10f;
    public float enemySpeed = 0.02f;
    public float enemySize = 1f;
    public float enemyAttack = 8f;
    public float enemyWeight = 0.2f;

    // ## ##
    [Header("Boss")]
    [SerializeField] private GameObject uiBossHPHolder;
    [SerializeField] private Image bossHealthBar;

    // ## ##
    [Header("Player Level")]
    [SerializeField] private int playerXP = 0;
    [SerializeField] private int playerLevelRequirement = 3;

    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }

    private void Start() {
        UpdateExpBar();
        ResumeGame();
    }

    private void OnEnable() {
        EnemyScript.OnGainXP += GainXP;
        PlayerController.OnPlayerDeath += DeathScreen;
        RoomTransition.OnBeginBossFight += BeginBossfight;
        BossScript.OnBossKill += BossFightWin;
    }

    private void OnDisable() {
        EnemyScript.OnGainXP -= GainXP;
        PlayerController.OnPlayerDeath -= DeathScreen;
        RoomTransition.OnBeginBossFight -= BeginBossfight;
        BossScript.OnBossKill -= BossFightWin;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (!isPaused){
                PauseGame();
            }
            else if (levelUpMenu.activeSelf == true || deathScreen.activeSelf == true){
                return;
            }
            else{
                ResumeGame();
                
            }
        }
    }

    //## PAUSING ##

    private void PauseGame(){
        pauseMenu.SetActive(true);
        MenuMode();
    }
    public void ResumeGame(){
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        GameplayMode();
    }
    public void MenuMode(){
        isPaused = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void GameplayMode(){
        isPaused = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
    public void UpdateHealthBar(){
        healthBar.fillAmount = (PlayerController.instance.currentHealth/playerMaxHP);
    }

    //## XP ##

    private void UpdateExpBar(){
        if(playerLevelRequirement > 0){
            expBar.fillAmount = (((float)playerXP)/playerLevelRequirement);
        }
    }
    private void GainXP(int value) {
        playerXP += value;
        if (playerXP >= playerLevelRequirement)
        {
            LevelUp();
        }
        UpdateExpBar();
        killCount++;
        killCounter.text = (killCount.ToString());
    }
    private void LevelUp(){
        MenuMode();
        levelUpMenu.SetActive(true);
        OnLevelUp?.Invoke();
        if (playerLevelRequirement <= 6){
            playerLevelRequirement +=3;
        }
        else if (playerLevelRequirement <= 10){
            playerLevelRequirement +=2;
        }
        else if (playerLevelRequirement <= 20){
            playerLevelRequirement +=1;
        }
        playerXP = 0;
    }

    //## DYING ##

    public void QuitGame(){
        SceneManager.LoadScene("MainMenu");
    }
    private void DeathScreen(){
        MenuMode();
        deathScreen.SetActive(true);
    }
    public void RestartScene(){
        GameplayMode();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //## BOSS FIGHT ##

    public void BeginBossfight(){
        uiBossHPHolder.SetActive(true);
    }
    public void UpdateBossHealthBar(){
        bossHealthBar.fillAmount = BossScript.instance.currentHealth / BossScript.instance.maxHealth;
    }
    private void BossFightWin(){
        StartCoroutine(EndGameScreen());
    }
    private IEnumerator EndGameScreen(){
        yield return KillAllEnemies();
        yield return new WaitForSeconds(3f);
        yield return KillAllEnemies();  //in case boss dies while spawning a wave
        yield return YellowScreen();
        isPaused = true;                //disable movement for a sec
        BossTransition();
        yield return new WaitForSeconds(1f);
        blackoutPanel.gameObject.SetActive(false);
        isPaused = false;                //re enable for a sec
        yield return new WaitForSeconds(1f);
        winScreen.SetActive(true); // boom gg
        MenuMode(); 
    }

    private IEnumerator KillAllEnemies(){
        EnemyScript[] enemies = FindObjectsOfType<EnemyScript>();
        foreach(EnemyScript i in enemies){
            i.Die();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator YellowScreen(){
        Color objectColor = blackoutPanel.color;
        float fadeAmount;
        while (blackoutPanel.color.a < 1){
            fadeAmount = objectColor.a + (0.6f * Time.deltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackoutPanel.color = objectColor;
            yield return null;
        }
    }
    private void BossTransition(){
        uiBossHPHolder.SetActive(false);
        Destroy(BossScript.instance.gameObject);
    }
}
