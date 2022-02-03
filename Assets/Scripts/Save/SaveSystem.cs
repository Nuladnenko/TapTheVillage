using UnityEngine;
using System.IO;
using System;


public class SaveSystem 
{
    public static string path = Application.persistentDataPath + "/savefile.json";
    public static SaveData data;

    //GAMEMANAGER
    public static void SaveGame(GameManager gameManager)
    {
        data = new SaveData();
        if (File.Exists(path))
            data = Load(path);
        data.coroutineStartTime = gameManager.CoroutineStartTime;
        data.savedCurrency = GameManager.Currency;
        data.toSavedCurrency = gameManager.CurrencyPerClick;
        data.savedCurrencyPerSec = gameManager.CurrencyPerSec;
        data.savedUpgradeOfCurrencyPerSec = gameManager.UpgradeOfCurrencyPerSec;
        data.savedUnitCurrencyPerSec = gameManager.UnitCurrencyPerSec;
        data.coroutineAppLaunchStartTime = gameManager.CoroutineAppLaunchStartTime;
        data.coroutineStartIndex = gameManager.CoroutineStartIndex;
        Save(data, path);
    }
    public static SaveData LoadGame()
    {
        return Load(path);
    }

    //JOHNY
    public static void SaveUnitAction(double cost, int level)
    {
        data = new SaveData();
        if (File.Exists(path))
            data = Load(path);
        data.unitClickCost = cost;
        data.unitClickLevel = level;
        Save(data, path);
    }

    //UNIT
    public static void SaveUnitProd(double[] cost, int[] level, bool[] isActivated)
    {
        data = new SaveData();
        if (File.Exists(path))
            data = Load(path);
        data.unitsCost = cost;
        data.unitsLevel = level;
        data.isUnitActivated = isActivated;
        Save(data, path);
    }
    public static void SaveUnitProdState(int id)
    {
        data = new SaveData();
        
        if (File.Exists(path))
            data = Load(path);
        else
            data.isUnitBought = new bool[]{};
            
        if (data.isUnitBought.Length<id+1)
            Array.Resize(ref data.isUnitBought, id+1);

        data.isUnitBought[id] = true;
        Save(data, path);
    }
    public static void SaveUpgradeProdState(int id, int numberOfGroup)
    {
        data = new SaveData();
        
        if (File.Exists(path))
            data = Load(path);
        else
        {
            data.isUpgradeGroup1Bought = new bool[]{};
            data.isUpgradeGroup2Bought = new bool[]{};
        }

        if (numberOfGroup==1)
        {        
            if(data.isUpgradeGroup1Bought.Length<=id)
                Array.Resize(ref data.isUpgradeGroup1Bought, id+1);
            if(data.isUpgradeGroup2Bought.Length!=0 && data.isUpgradeGroup2Bought[id]==true)
                data.isUpgradeGroup1Bought[id] = false;
            else 
                data.isUpgradeGroup1Bought[id] = true;
        }
        if (numberOfGroup==2)
        {
            if(data.isUpgradeGroup2Bought.Length<=id)
                Array.Resize(ref data.isUpgradeGroup2Bought, id+1);
            if(data.isUpgradeGroup1Bought.Length!=0 && data.isUpgradeGroup1Bought[id]==true)
                data.isUpgradeGroup1Bought[id] = false;
            data.isUpgradeGroup2Bought[id] = true;
            Debug.Log(data.isUpgradeGroup2Bought.Length);
        }
        Save(data, path);
        
    }

    //UPGRADE
    public static void SaveUpgrade(UpgradesDataContainer upgradesDataContainer)
    {
        data = new SaveData();
        if (File.Exists(path))
            data = Load(path);
        data.isUpgradeGroup1Activated = upgradesDataContainer.isGroup1Activated;
        data.isUpgradeGroup2Activated = upgradesDataContainer.isGroup2Activated;
        Save(data, path);
    }


    public static SaveData Load(string path)
    {
        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        return data;
    }
    private static void Save(SaveData data, string path)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }
}

