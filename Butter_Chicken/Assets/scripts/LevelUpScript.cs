using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class LevelUpScript : MonoBehaviour
{
    public static event Action OnUpdateStats;

    // LIST OF UPGRADES NEEDS TO BE MANUALLY SPECIFIED FROM UPGRADE SCRIPTABLE OBJECTS
    [SerializeField] List<Upgrade> upgrades;
    
    // RANDOMLY CHOSEN UPGRADES
    private Upgrade option1;
    private Upgrade option2;
    private Upgrade option3;

    // NAMES AND DESCRIPTIONS OF RANDOMIZED UPGRADES
    [SerializeField] GameObject upgradesHolder;
    [SerializeField] TextMeshProUGUI option1Name;
    [SerializeField] TextMeshProUGUI option1Desc;

    [SerializeField] TextMeshProUGUI option2Name;
    [SerializeField] TextMeshProUGUI option2Desc;

    [SerializeField] TextMeshProUGUI option3Name;
    [SerializeField] TextMeshProUGUI option3Desc;

    // SELECTED UPGRADE DETAILS
    [SerializeField] GameObject selectedUpgradeHolder;
    [SerializeField] TextMeshProUGUI upgradeName;
    [SerializeField] TextMeshProUGUI upgradeDesc;
    [SerializeField] TextMeshProUGUI upgradeDesc2;


    private void OnEnable() {
        GameManager.OnLevelUp += RandomizeUpgrades;
    }

    private void OnDisable() {
        GameManager.OnLevelUp -= RandomizeUpgrades;
    }

    private void RandomizeUpgrades() {
        // i hate how it looks
        int upgradesIndex = upgrades.Count;
        int random1 = UnityEngine.Random.Range(0,upgradesIndex);
        int random2;
        int random3;
        do{
            random2 = UnityEngine.Random.Range(0,upgradesIndex);
        }while(random1 == random2);
        do{
        random3 = UnityEngine.Random.Range(0,upgradesIndex);
        }while(random3 == random1 || random3 == random2);

        option1 = upgrades[random1];
        option2 = upgrades[random2];
        option3 = upgrades[random3];

        ApplyLabels();
    }

    private void ApplyLabels(){
        option1Name.text = option1.upgradeName;
        option1Desc.text = option1.description;

        option2Name.text = option2.upgradeName;
        option2Desc.text = option2.description;

        option3Name.text = option3.upgradeName;
        option3Desc.text = option3.description;

        upgradesHolder.SetActive(true);
    }

    public void UpgradeOption1(){
        if(option1 != null){
            option1.Apply();
            ApplyUpgrade(option1);
        }
        else{Debug.Log("upgrade returned null what the fuck");}

    }
    public void UpgradeOption2(){
        if(option2 != null){
            option2.Apply();
            ApplyUpgrade(option2);
        }
        else{Debug.Log("upgrade returned null what the fuck");}

    }
    public void UpgradeOption3(){
        if(option3 != null){
            option3.Apply();
            ApplyUpgrade(option3);
        }
        else{Debug.Log("upgrade returned null what the fuck");}
    }

    private void ApplyUpgrade(Upgrade upgrade){
    upgradesHolder.SetActive(false);
    OnUpdateStats?.Invoke();
    upgradeName.text = upgrade.upgradeName;
    upgradeDesc.text = upgrade.description;
    upgradeDesc2.text = upgrade.secondDescription;
    selectedUpgradeHolder.SetActive(true);
    }
    // holy shit if it works at first attempt imma touch myself tonight
    // for the record it did not work at first attempt - A3
    public void UpgradeComplete(){
        GameManager.isPaused = false;
        Time.timeScale = 1f;
        Debug.Log("UPGRADE COMPLETE");
        selectedUpgradeHolder.SetActive(false);
        gameObject.SetActive(false);
    }
}
