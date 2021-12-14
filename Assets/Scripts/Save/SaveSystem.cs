using UnityEngine;
using System.IO;


public class SaveSystem 
{
    public static string path = Application.persistentDataPath + "/savefile.json";
    public static SaveData oldData;    //нужен, чтобы заменить новые элементы массива на старые, если новые равны 0
    public static SaveData data;

    //GAMEMANAGER
    public static void SaveGame(float currency, float currencyPerClick, float[] currencyPerSec, float[] unitCurrencyPerSec, int[] coroutineStartTime)
    {
        data = new SaveData();
        if (File.Exists(path))
        {
            oldData = Load(path);
            data = oldData;

            if(data.coroutineStartTime.Length < coroutineStartTime.Length)
                System.Array.Resize(ref data.coroutineStartTime, coroutineStartTime.Length);  //изменить размер массива в случае, если новый сохраняемый массив больше старого

            for (int i = 0; i < coroutineStartTime.Length; i++)
            {
                if (coroutineStartTime[i] == 0)
                    data.coroutineStartTime[i] = oldData.coroutineStartTime[i];
                else
                    data.coroutineStartTime[i] = coroutineStartTime[i];
            }
        }
        else
            data.coroutineStartTime = coroutineStartTime;

        data.savedCurrency = currency;
        data.toSavedCurrency = currencyPerClick;
        data.savedCurrencyPerSec = currencyPerSec;
        data.savedUnitCurrencyPerSec = unitCurrencyPerSec;
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

