using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    //object spawn
    public GameObject Butter;
    //place spawn
    public float Xrangemin;
    public float Xrangemax;
    public float Zrangemin;
    public float Zrangemax;

    public int waven; //number of the wave
    private float enemyn;
    bool spawningWave;

    public int maxenemy;
    private bool endround;

    // Update is called once per frame
    void Start() {
        waven = 1;
        spawningWave = false;
        endround = true;
    }

    void Update() {
        if (enemyn < maxenemy && endround == true) {
            //wave enemy
            Vector3 RandomPlanePosition = new Vector3(Random.Range(Xrangemin, Xrangemax), 5, Random.Range(Zrangemin, Zrangemax));

            if (Instantiate(Butter, RandomPlanePosition, Quaternion.identity)) {
                enemyn = enemyn + 1;
            }

            spawningWave = true;
            endround = false;
        }




        if (spawningWave == false) {
            //wavy number
            waven = waven + 1;
            maxenemy = maxenemy * 2;
        }

        //this was giving errors so commented out
        // int enemyspawn = GameObject.Find("butter");

        // if (enemyspawn = enemykill)
        // {
        //     endround = true;
        // }
    }
}