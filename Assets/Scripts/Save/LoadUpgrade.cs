using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadUpgrade : LoadMain
{
    [SerializeField] private int numberOfGroup;
    protected new void Start()
    {
        base.Start();
    }

    protected override void Load()
    {
        SaveData data = SaveSystem.Load(Application.persistentDataPath + "/savefile.json");
        if(numberOfGroup==1)
            isBought = data.isUpgradeGroup1Bought;
        else if(numberOfGroup==2)
            isBought = data.isUpgradeGroup2Bought;
    }
}
