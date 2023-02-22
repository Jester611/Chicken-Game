using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class playerhealth : MonoBehaviour
{
    public int maxhealth = 20;
    public int currenthealth;

   
    // Start is called before the first frame update
    void Start()
    {
        currenthealth = maxhealth;
    }
}
