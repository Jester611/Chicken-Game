using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance  { get; private set; }
    public static event Action OnUpdateStats;

    // ## MENUS ##
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject levelUpMenu;
    [SerializeField] GameObject deathScreen;

    public static bool isPaused;

    // ## COUNTERS ##
    [SerializeField] Image healthBar;
    [SerializeField] Image expBar;

    // ## VARIABLES RELATED TO UPGRADES ##
    [Header("Bullet")]
    public float bulletDamage;
    public float bulletSize;
    public float bulletExplosiveRadius;
    public float bulletKnockback;
    [Header("Weapon")]
    public float gunRateOfFire;
    public float gunBulletSpeed;
    public float gunRecoil;
    [Header("Player")]
    public float playerSpeed;
    public float playerMaxHP;
    public float playerInvincibilityTimer;
    [Header("Enemy")]
    public float enemyHP;
    public float enemySpeed;
    public float enemySize;
    public float enemyAttack;

    // Player levels
    [Header(header: "Player Levels")]

    private int playerXP;
    private int playerLevelRequirement = 10;
    private int playerLevel;

    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }

    private void Start() {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    private void OnEnable() {
        EnemyScript.OnEnemyKilled += GainXP;
        PlayerController.instance.OnDeath += DeathScreen;
    }

    private void OnDisable() {
        EnemyScript.OnEnemyKilled -= GainXP;
        PlayerController.instance.OnDeath += DeathScreen;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (!isPaused){ // gonna need clause for level up menu when it's done
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

    private void PauseGame(){
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        //Cursor.visible = true;
        // are we gonna replace cursor with crosshair?
    }

    public void ResumeGame(){
        isPaused = false;
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        Time.timeScale = 1f;
        //Cursor.visible = false;
    }

    public void UpdateHealthBar(){
        healthBar.fillAmount = (PlayerController.instance.currentHealth/playerMaxHP);
    }
    
    private void GainXP() {
        playerXP++;
        if (playerXP >= playerLevelRequirement)
        {
            LevelUp();
            Debug.Log("player leveled up");
        }
        if(playerLevelRequirement > 0){
            expBar.fillAmount = (((float)playerXP)/playerLevelRequirement);
        }
    }

    private void LevelUp(){
        isPaused = true;
        Time.timeScale = 0f;
        levelUpMenu.SetActive(true);
    }

    public void QuitGame(){
        // quit to main menu
        // scene name once it's done

        //SceneManager.LoadScene("MainMenu");
    }
    
    private void DeathScreen(){
        isPaused = true; // prevents player movement
        deathScreen.SetActive(true);
    }

    public void RestartScene(){
        isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
