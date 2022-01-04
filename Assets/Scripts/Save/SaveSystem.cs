using UnityEngine;
using System.IO;


public class SaveSystem 
{
    public static string path = Application.persistentDataPath + "/savefile.json";
    public static SaveData oldData;    //нужен, чтобы заменить новые элементы массива на старые, если новые равны 0
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
    public static void SaveUnitClick(float cost, int level)
    {
        data = new SaveData();
        if (File.Exists(path))
            data = Load(path);
        data.unitClickCost = cost;
        data.unitClickLevel = level;
        Save(data, path);
    }

    //UNIT
    public static void SaveUnit(float[] cost, int[] level, bool[] isActivated)
    {
        data = new SaveData();
        if (File.Exists(path))
            data = Load(path);
        data.unitsCost = cost;
        data.unitsLevel = level;
        data.isUnitActivated = isActivated;
        Save(data, path);
    }

    //UPGRADE
    public static void SaveUpgrade(bool[] isActivated)
    {
        data = new SaveData();
        if (File.Exists(path))
            data = Load(path);
        data.isUpgradeActivated = isActivated;
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

