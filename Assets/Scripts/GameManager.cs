using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.IO;



public class GameManager : MonoBehaviour
{
    public static double Currency { get; private set; }          //������� ������ ����
    public double CurrencyPerClick { get; private set; }
    private double[] currencyPerSec;
    public double[] CurrencyPerSec {
        get { return currencyPerSec; }
        private set { currencyPerSec = value; }
    }
    private double[] unitCurrencyPerSec;
    public double[] UnitCurrencyPerSec {
        get { return unitCurrencyPerSec; }
        private set { unitCurrencyPerSec = value; }
    }
    private double[] upgradeOfCurrencyPerSec;
    public double[] UpgradeOfCurrencyPerSec {
        get { return upgradeOfCurrencyPerSec; }
        private set { upgradeOfCurrencyPerSec = value; }
    }
    private float[] coroutineStartTime;
    public float[] CoroutineStartTime {
        get { return coroutineStartTime; }
        private set { coroutineStartTime = value; }
    }
    private float[] oldCoroutineStartTime;

    public int[] CoroutineStartIndex { get; private set; }                          //������, � ������� ����������� ������� ��������� ������������ �������
    public float[] CoroutineAppLaunchStartTime { get; private set; }                //������, � ������� ����������� ����� ������� ��������� �������� ������������ ����������

    private int unitActionMult; 

    public delegate void ClickOnSingleButton(float multiplier, double cost);         //��� buttons, ������� �� �������� index
    public static ClickOnSingleButton ClickOnUnitActionButton { get; private set; }
     public delegate void ClickOnSingleUpgradeButton(double cost);         //��� buttons, ������� �� �������� index
    public static ClickOnSingleUpgradeButton ClickOnUpgradeActionButton { get; private set; }

    public delegate void ClickOnUnitButton(double currencyPerSec, double cost, int index);        //��� buttons, ������� �������� index
    public static ClickOnUnitButton ClickOnUnitProdButton { get; private set; }
    public delegate void ClickOnUpgradeButton(int currencyPerSecMultiplier, double cost, int index); 
    public static ClickOnUpgradeButton ClickOnUpgradeProdButton { get; private set; }

    public delegate void CurrencyChange();
    public static CurrencyChange CurrencyClick { get; private set; }
    public static event CurrencyChange OnCurrencyHasChanged;                  //��� ��������� currency ��������� ������ ������ �������, ����������� �� ��� �������

    private Coroutine[] unitCoroutine;


    private GameManager()
    {
        unitCoroutine = new Coroutine[1];
        currencyPerSec = new double[1];
        unitCurrencyPerSec = new double[1];
        upgradeOfCurrencyPerSec = new double[1];
        coroutineStartTime = new float[1];
        Currency = 0;
        CurrencyPerClick = 1;
        upgradeOfCurrencyPerSec[0] = 1;
        unitActionMult = 1;
    }
    private void Awake()
    {
        ClickOnUnitActionButton = ClickUnitActionButton;
        ClickOnUnitProdButton = ClickUnitProdButton;
        ClickOnUpgradeProdButton = ClickUpgradeButton;
        ClickOnUpgradeActionButton = ClickUpgradeActionButton;
        CurrencyClick = ClickOnScreen;

        if(File.Exists(Application.persistentDataPath + "/savefile.json"))
            Load();

        oldCoroutineStartTime = (float[])CoroutineStartTime.Clone();
    }
    private void ClickOnScreen()
    {
        Currency += CurrencyPerClick;
        OnCurrencyHasChanged();
    }

    private void ClickUnitActionButton(float multiplier, double cost)
    {
        CurrencyPerClick += multiplier*unitActionMult;
        Debug.Log(unitActionMult);
        Currency -= cost;
        OnCurrencyHasChanged();
    }
    private void ClickUpgradeActionButton(double cost)
    {
        unitActionMult *=2;
        CurrencyPerClick *= 2;
        Currency -= cost;
        OnCurrencyHasChanged();
    }
    private void ClickUnitProdButton(double currencyPerSec, double cost, int index)
    {
        if (unitCoroutine.Length <= index)
        {
            ResizeArrays(index);
            upgradeOfCurrencyPerSec[index] = 1;
        }

        unitCurrencyPerSec[index] += currencyPerSec;
        ClickButton(cost, index);

        if (unitCoroutine[index] == null)
            UnitCoroutineStart(index);
    }
    private void ClickUpgradeButton(int currencyPerSecMultiplier, double cost, int index)
    {
        upgradeOfCurrencyPerSec[index] *= currencyPerSecMultiplier;
        ClickButton(cost, index);
    }
    
    private void ClickButton(double cost, int index)
    {
        currencyPerSec[index] = unitCurrencyPerSec[index] * upgradeOfCurrencyPerSec[index];
        Currency -= cost;
        OnCurrencyHasChanged();
    }

    private void ResizeArrays(int index)
    {
        unitCoroutine = new Coroutine[index + 1];
        Array.Resize(ref coroutineStartTime, index + 1);
        Array.Resize(ref currencyPerSec, index + 1);
        Array.Resize(ref unitCurrencyPerSec, index + 1);
        Array.Resize(ref upgradeOfCurrencyPerSec, index + 1);
    }


    //COROUTINE
    private void UnitCoroutineStart(int i)
    {
        coroutineStartTime[i] = DateTime.Now.Millisecond;
        coroutineStartTime[i] /= 1000;                              //������������ ����������� � �������
        unitCoroutine[i] = StartCoroutine(UnitCoroutine(i)); 
    }

    //private void UnitCoroutineStop(int i)           //���� ����� �����������, ���� ���� ������� ����������� ������� Unit � �� ���-�� ����� == 0
    //{
    //    if (unitCoroutine[i] != null)
    //    {
    //        StopCoroutine(unitCoroutine[i]);
    //        unitCoroutine[i] = null;
    //    }
    //}
    private IEnumerator UnitCoroutine(int i)
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            Currency += currencyPerSec[i];
            OnCurrencyHasChanged();
            Debug.Log("Unit = " + currencyPerSec[i]);
        }
    }
    private IEnumerator LaunchUnitCoroutines()                      //������ ��� ������������ ��������� ��� ��������� ����������
    {
        for (int i = 0; i < CoroutineStartIndex.Length; i++)
        {
            yield return new WaitForSeconds(CoroutineAppLaunchStartTime[i]);
            unitCoroutine[CoroutineStartIndex[i]] = StartCoroutine(UnitCoroutine(CoroutineStartIndex[i]));   
        }
    }

    public void CalculatingUnitsLaunchTime()                        //������ ������� � ������� ������� ��������� ��� ��������� ����������
    {
        float[] coroutineStartTimeSorting = (float[])CoroutineStartTime.Clone();     //����������� ������
        float[] coroutineStartTimeSorted = new float[CoroutineStartTime.Length];     //������, � ������� ����������� ������ ����� ����������
        CoroutineStartIndex = new int[CoroutineStartTime.Length];
        CoroutineAppLaunchStartTime = new float[CoroutineStartTime.Length];

        for (int i = 0; i < CoroutineStartTime.Length; i++)
        {
            coroutineStartTimeSorted[i] = coroutineStartTimeSorting.Min();                                                               //������� ���������� ������� �������
            CoroutineStartIndex[i] = Array.IndexOf(coroutineStartTimeSorting, coroutineStartTimeSorted[i]);                              //������� ������ ����������� ��������
            coroutineStartTimeSorting[CoroutineStartIndex[i]] = coroutineStartTimeSorting.Max() + coroutineStartTimeSorting.Min();       //������ min ������� max, ����� � ��������� ������ ���� ������� �������������
        }

        CoroutineAppLaunchStartTime[0] = coroutineStartTimeSorted[0];
        for (int i = 0; i < CoroutineStartTime.Length - 1; i++)
            CoroutineAppLaunchStartTime[i + 1] = coroutineStartTimeSorted[i + 1] - coroutineStartTimeSorted[i];
    }
    private bool CheckArraysEquality(float[] array1, float[] array2)
    {
        bool allElementsAreEqual = false;
        if (array1.Length == array2.Length)
        {
            allElementsAreEqual = true;
            for (int i = 0; i < array2.Length; i++)
                if (array1[i] != array2[i])
                {
                    allElementsAreEqual = false;
                    break;
                }
        }
        return allElementsAreEqual;
    }


    public void Load()
    {
        SaveData data = SaveSystem.LoadGame();

        unitCoroutine = new Coroutine[data.savedCurrencyPerSec.Length];  //�������������� ������� ��������
        unitCurrencyPerSec = new double[data.savedCurrencyPerSec.Length];
        coroutineStartTime = new float[data.savedCurrencyPerSec.Length];

        Currency = data.savedCurrency;
        CurrencyPerClick = data.toSavedCurrency;
        currencyPerSec = data.savedCurrencyPerSec;
        unitCurrencyPerSec = data.savedUnitCurrencyPerSec;
        upgradeOfCurrencyPerSec = data.savedUpgradeOfCurrencyPerSec;
        coroutineStartTime = data.coroutineStartTime;
        CoroutineStartIndex = data.coroutineStartIndex;
        CoroutineAppLaunchStartTime = data.coroutineAppLaunchStartTime;

        StartCoroutine(LaunchUnitCoroutines());
    }
#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        if(!CheckArraysEquality(oldCoroutineStartTime,CoroutineStartTime)) 
            CalculatingUnitsLaunchTime();
    }
#else
    private void OnApplicationPause()
    {
        if(!CheckArraysEquality(oldCoroutineStartTime,CoroutineStartTime)) 
            CalculatingUnitsStartTime();
    }
#endif
}
