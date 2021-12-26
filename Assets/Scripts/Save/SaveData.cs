using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{
    public float savedCurrency;    //данные GameManager
    public float toSavedCurrency;
    public float[] savedCurrencyPerSec;
    public float[] savedUnitCurrencyPerSec;
    public float[] savedUpgradeOfCurrencyPerSec;
    public float[] coroutineStartTime;
    public float[] coroutineAppLaunchStartTime;
    public int[] coroutineStartIndex;


    public float unitClickCost;  //данные UnitClickButton
    public int unitClickLevel;
    //public string title;
    //public string description;

    public float[] unitsCost;  //данные UnitButton
    public int[] unitsLevel;
    public bool[] isUnitActivated;

    public bool[] isUpgradeActivated;

}
