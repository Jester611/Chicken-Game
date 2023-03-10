using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance  { get; private set; }
    public static event Action OnLevelUp;

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
    public float bulletDamage = 5;
    public float bulletExplosiveRadius = 0;
    public float bulletKnockback = 0;
    [Header("Weapon")]
    public float gunRateOfFire = 0.7f;
    public float gunBulletSpeed = 15f;
    public float gunRecoil = 0f;
    public float gunSpread = 0f;
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

    // ## PLAYER LEVELS ##
    private int playerXP = 0;
    private int playerLevelRequirement = 8;
    private int playerLevel = 1;

    private DefaultStats defaultstats;

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
        EnemyScript.OnGainXP += GainXP;
        PlayerController.OnPlayerDeath += DeathScreen;
    }

    private void OnDisable() {
        EnemyScript.OnGainXP -= GainXP;
        PlayerController.OnPlayerDeath += DeathScreen;
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
        OnLevelUp?.Invoke();
        playerLevel ++;
        playerXP = 0;
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
