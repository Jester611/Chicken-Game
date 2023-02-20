using System;
using UnityEngine;

public class LevelUpScript : MonoBehaviour
{
    public static event Action OnUpdateStats;


    //this will link to level up menu and update stats in singleton

    private void OnEnable() {
        GlobalStats.OnPlayerLevel += LevelUp;
    }
    private void OnDisable() {
        GlobalStats.OnPlayerLevel -= LevelUp;
        
    }

    private void LevelUp(){
        Time.timeScale = 0f;
    }
}
