using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject levelUpMenu;

    public static bool isPaused;
    private void Start() {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (!isPaused){ // gonna need clause for level up menu when it's done
                PauseGame();
            }
            else{
                ResumeGame();
            }
        }
    }

    private void OnEnable() {
        GlobalStats.OnPlayerLevel += LevelUp;
    }

    private void OnDisable() {
        GlobalStats.OnPlayerLevel -= LevelUp;
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
    
    private void LevelUp(){
        //todo: opens up level up menu
    }

    public void QuitGame(){
        //main menu scene name once it's done

        //SceneManager.LoadScene("MainMenu");
    }
}
