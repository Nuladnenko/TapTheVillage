using System;

[Serializable]
public class SaveData
{
    public double savedCurrency;    // GameManager
    public double toSavedCurrency;
    public double[] savedCurrencyPerSec;
    public double[] savedUnitCurrencyPerSec;
    public double[] savedUpgradeOfCurrencyPerSec;
    public float[] coroutineStartTime;
    public float[] coroutineAppLaunchStartTime;
    public int[] coroutineStartIndex;


    public double unitClickCost;  // UnitClickButton
    public int unitClickLevel;
    //public string title;
    //public string description;

    public double[] unitsCost;  // UnitButton
    public int[] unitsLevel;
    public bool[] isUnitActivated;
    public bool[] isUnitBought;

    public bool[] isUpgradeGroup1Activated;
    public bool[] isUpgradeGroup2Activated;
    public bool[] isUpgradeGroup1Bought;
    public bool[] isUpgradeGroup2Bought;

}
