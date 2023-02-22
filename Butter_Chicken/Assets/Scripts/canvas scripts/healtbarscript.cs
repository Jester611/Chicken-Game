using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class healtbarscript : MonoBehaviour
{
    public playerhealth playerhealth;
    private Image healthbar;

    private void Start()
    {
        healthbar = GetComponent<Image>();
     
    }

    private void Update()
    {
        healthbar.fillAmount = playerhealth.currenthealth / playerhealth.maxhealth;

    }

}