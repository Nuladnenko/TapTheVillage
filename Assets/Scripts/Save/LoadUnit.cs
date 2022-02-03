using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadUnit : LoadMain
{
    protected new void Start()
    {
        base.Start();
    }

    protected override void Load()
    {
        SaveData data = SaveSystem.Load(Application.persistentDataPath + "/savefile.json");
        isBought = data.isUnitBought;
    }
}
