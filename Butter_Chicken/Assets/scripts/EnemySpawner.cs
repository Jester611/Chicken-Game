using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<Wave> waves;
    [SerializeField] Bounds bounds;
    [SerializeField] int waveNum = 0;
    private int finishedTypes = 0;

    public event System.Action WaveFinished;

    private void Start() {
        Invoke("StartNextWave", 2f);
    }

    void StartNextWave(){
        foreach(EnemyWave wave in waves[waveNum].waveTypes){
            StartCoroutine(wave.Spawn(bounds, transform));
            wave.OnFinish += WaveTypeFinished;
        }
    }

    void WaveTypeFinished(){
        finishedTypes++;
        if(finishedTypes >= waves[waveNum].waveTypes.Count){
            WaveFinished?.Invoke();
            finishedTypes = 0;
            waveNum = (waveNum + 1) % waves.Count;
        }
        Debug.Log("Wave Finished");
        StartNextWave();
    }
}

[System.Serializable]
public class Wave
{
    public List<EnemyWave> waveTypes;
}