using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LevelUpScript : MonoBehaviour
{
    public static event Action OnUpdateStats;

    // LIST OF UPGRADES NEEDS TO BE MANUALLY SPECIFIED FROM UPGRADE SCRIPTABLE OBJECTS
    [SerializeField] Upgrade[] upgrades;
    
    // RANDOMLY CHOSEN UPGRADES ARRAY
    private Upgrade[] option = new Upgrade [3];

    [SerializeField] GameObject upgradesHolder;

    // CHOOSE UPGRADE MENU ARRAYS

        //at this point it'd be better to use a hashtable for each value, variable and reference assigned to an upgrade but whetever, i can't be assed - A3

    [SerializeField] TextMeshProUGUI[] optionName = new TextMeshProUGUI[3];
    [SerializeField] TextMeshProUGUI[] optionDesc = new TextMeshProUGUI[3];
    [SerializeField] TextMeshProUGUI[] optionType = new TextMeshProUGUI[3];
    [SerializeField] Image[] optionFrame = new Image[3];

    // SELECTED UPGRADE DETAILS
    [SerializeField] GameObject selectedUpgradeHolder;
    [SerializeField] TextMeshProUGUI upgradeName;
    [SerializeField] TextMeshProUGUI upgradeDesc;
    [SerializeField] TextMeshProUGUI upgradeDesc2;

    // SEPARATE COLORS FOR UPGRADE TYPES
    [SerializeField] Color color_weapon = UnityEngine.Color.red;
    [SerializeField] Color color_player = UnityEngine.Color.cyan;
    [SerializeField] Color color_enemy = UnityEngine.Color.green;


    private void OnEnable() {
        GameManager.OnLevelUp += RandomizeUpgrades;
    }

    private void OnDisable() {
        GameManager.OnLevelUp -= RandomizeUpgrades;
    }

    private void Start() {
        upgrades = Resources.LoadAll<Upgrade>("Upgrades");
    }

    private void RandomizeUpgrades() {
        // i hate how it looks
        int upgradesIndex = upgrades.Length;
        int random1 = UnityEngine.Random.Range(0,upgradesIndex);
        int random2;
        int random3;
        do{
            random2 = UnityEngine.Random.Range(0,upgradesIndex);
        }while(random1 == random2);
        do{
        random3 = UnityEngine.Random.Range(0,upgradesIndex);
        }while(random3 == random1 || random3 == random2);

        option[0] = upgrades[random1];
        option[1] = upgrades[random2];
        option[2] = upgrades[random3];

        ApplyLabels();
    }

    private void ApplyLabels(){
        Color[] optioncolors = new Color[3];
        for (int i = 0; i < option.Length; i++){
            switch(option[i].upgradeType){
                case 1:{
                    optioncolors[i] = color_weapon;
                    continue;
                }
                case 2:{
                    optioncolors[i] = color_player;
                    continue;
                }
                case 3:{
                    optioncolors[i] = color_enemy;
                    continue;
                }
                default:{
                    continue;
                }
            }
        }
        for (int i=0; i<3; i++){
            optionName[i].text = option[i].upgradeName;
            optionDesc[i].text = option[i].description;
            optionFrame[i].color = optioncolors[i];
            optionType[i].color = optioncolors[i];
            switch(option[i].upgradeType){
                case 1:
                optionType[i].text = "SWAP WEAPON";
                continue;
                case 2:
                optionType[i].text = "ROOSTER CYBERNETIC";
                continue;
                case 3:
                optionType[i].text = "GENETIC MUTATION";
                continue;
            }
        }
        GameManager.instance.MenuMode();
        upgradesHolder.SetActive(true);
    }

    public void UpgradeOption1(){
        if(option[0] != null){
            option[0].Apply();
            ApplyUpgrade(option[0]);
        }
        else{Debug.Log("upgrade returned null what the fuck");}

    }
    public void UpgradeOption2(){
        if(option[1] != null){
            option[1].Apply();
            ApplyUpgrade(option[1]);
        }
        else{Debug.Log("upgrade returned null what the fuck");}

    }
    public void UpgradeOption3(){
        if(option[2] != null){
            option[2].Apply();
            ApplyUpgrade(option[2]);
        }
        else{Debug.Log("upgrade returned null what the fuck");}
    }

    private void ApplyUpgrade(Upgrade upgrade){
    upgradesHolder.SetActive(false);
    upgradeName.text = upgrade.upgradeName;
    upgradeDesc.text = upgrade.description;
    upgradeDesc2.text = upgrade.secondDescription;
    selectedUpgradeHolder.SetActive(true);
    OnUpdateStats?.Invoke();
    }
    // holy shit if it works at first attempt imma touch myself tonight
    // for the record it did not work at first attempt - A3
    public void UpgradeComplete(){

        Debug.Log("UPGRADE COMPLETE");
        GameManager.instance.GameplayMode();
        selectedUpgradeHolder.SetActive(false);
        gameObject.SetActive(false);
    }
}
