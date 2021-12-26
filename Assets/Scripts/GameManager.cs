using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.IO;



public class GameManager : MonoBehaviour
{
    public static float Currency { get; private set; }          //главный ресурс игры
    public float CurrencyPerClick { get; private set; }
    private float[] currencyPerSec;
    public float[] CurrencyPerSec {
        get { return currencyPerSec; }
        private set { currencyPerSec = value; }
    }
    private float[] unitCurrencyPerSec;
    public float[] UnitCurrencyPerSec {
        get { return unitCurrencyPerSec; }
        private set { unitCurrencyPerSec = value; }
    }
    private float[] upgradeOfCurrencyPerSec;
    public float[] UpgradeOfCurrencyPerSec {
        get { return upgradeOfCurrencyPerSec; }
        private set { upgradeOfCurrencyPerSec = value; }
    }
    private float[] coroutineStartTime;
    public float[] CoroutineStartTime {
        get { return coroutineStartTime; }
        private set { coroutineStartTime = value; }
    }
    private float[] oldCoroutineStartTime;

    public int[] CoroutineStartIndex { get; private set; }                          //массив, в который сохраняются индексы элементов сортируемого массива
    public float[] CoroutineAppLaunchStartTime { get; private set; }                //массив, в который сохраняется время запуска следующей корутины относительно предыдущей


    public delegate void ClickOnSingleButton(float multiplier, float cost);         //для buttons, которые не передают index
    public static ClickOnSingleButton ClickOnUnitActionButton { get; private set; }

    public delegate void ClickOnButton(float currencyPerSecMultiplier, float cost, int index);        //для buttons, которые передают index
    public static ClickOnButton ClickOnUnitProdButton { get; private set; }
    public static ClickOnButton ClickOnUpgradeButton { get; private set; }

    public delegate void CurrencyChange();
    public static CurrencyChange CurrencyClick { get; private set; }
    public static event CurrencyChange OnCurrencyHasChanged;                  //При изменении currency запускает методы других классов, подписанные на это событие

    private Coroutine[] unitCoroutine;


    private GameManager()
    {
        unitCoroutine = new Coroutine[1];
        currencyPerSec = new float[1];
        unitCurrencyPerSec = new float[1];
        upgradeOfCurrencyPerSec = new float[1];
        coroutineStartTime = new float[1];
        Currency = 0;
        CurrencyPerClick = 1;
        upgradeOfCurrencyPerSec[0] = 1;
    }
    private void Awake()
    {
        ClickOnUnitActionButton = ClickUnitActionButton;
        ClickOnUnitProdButton = ClickUnitProdButton;
        ClickOnUpgradeButton = ClickUpgradeButton;
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

    private void ClickUnitActionButton(float multiplier, float cost)
    {
        CurrencyPerClick += multiplier;
        Currency -= cost;
        OnCurrencyHasChanged();
    }
    private void ClickUnitProdButton(float currencyPerSec, float cost, int index)
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
    private void ClickUpgradeButton(float currencyPerSecMultiplier, float cost, int index)
    {
        upgradeOfCurrencyPerSec[index] *= currencyPerSecMultiplier;
        ClickButton(cost, index);
    }
    private void ClickButton(float cost, int index)
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
        coroutineStartTime[i] /= 1000;                              //конвертируем милисекунды в секунды
        unitCoroutine[i] = StartCoroutine(UnitCoroutine(i)); 
    }

    //private void UnitCoroutineStop(int i)           //этот метод понадобится, если буду вводить возможность продажи Unit и их кол-во будет == 0
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
    private IEnumerator LaunchUnitCoroutines()                      //Запуск уже существующих корутинов при включении приложения
    {
        for (int i = 0; i < CoroutineStartIndex.Length; i++)
        {
            yield return new WaitForSeconds(CoroutineAppLaunchStartTime[i]);
            unitCoroutine[CoroutineStartIndex[i]] = StartCoroutine(UnitCoroutine(CoroutineStartIndex[i]));   
        }
    }

    public void CalculatingUnitsLaunchTime()                        //расчет времени и порядка запуска корутиров при включении приложения
    {
        float[] coroutineStartTimeSorting = (float[])CoroutineStartTime.Clone();     //сортируемый массив
        float[] coroutineStartTimeSorted = new float[CoroutineStartTime.Length];     //массив, в который сохраняются данные посли сортировки
        CoroutineStartIndex = new int[CoroutineStartTime.Length];
        CoroutineAppLaunchStartTime = new float[CoroutineStartTime.Length];

        for (int i = 0; i < CoroutineStartTime.Length; i++)
        {
            coroutineStartTimeSorted[i] = coroutineStartTimeSorting.Min();                                                               //находим наименьший элемент массива
            CoroutineStartIndex[i] = Array.IndexOf(coroutineStartTimeSorting, coroutineStartTimeSorted[i]);                              //находим индекс наименьшего элемента
            coroutineStartTimeSorting[CoroutineStartIndex[i]] = coroutineStartTimeSorting.Max() + coroutineStartTimeSorting.Min();       //делаем min элемент max, чтобы в следующих циклах этот элемент игнорировался
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

        unitCoroutine = new Coroutine[data.savedCurrencyPerSec.Length];  //восстановление размера массивов
        unitCurrencyPerSec = new float[data.savedCurrencyPerSec.Length];
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
