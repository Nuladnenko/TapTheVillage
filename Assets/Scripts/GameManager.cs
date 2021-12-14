using UnityEngine;
using TMPro;
using System.Collections;
using System;
using System.IO;
using System.Linq;




public class GameManager : MonoBehaviour

{
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI currencyPerSecText;
    [SerializeField] private TextMeshProUGUI currencyPerClickText;
    public static float Currency { get; private set; }         //главный ресурс игры
    [SerializeField] private float currencyPerClick;

    public delegate void ClickOnSingleButton(float multiplier, float cost);         //для buttons, которые не передают index
    public static ClickOnSingleButton ClickOnUnitActionButton { get; private set; }

    public delegate void ClickOnButton(float currencyPerSecMultiplier, float cost, int index);        //для buttons, которые передают index
    public static ClickOnButton ClickOnUnitProdButton { get; private set; }
    public static ClickOnButton ClickOnUpgradeButton { get; private set; }

    public delegate void CurrencyChange();
    public static CurrencyChange CurrencyClick { get; private set; }
    public static event CurrencyChange OnCurrencyHasChanged;       //При изменении currency запускает методы других классов, подписанные на это событие

    private Coroutine[] unitCoroutine;
    private float[] currencyPerSec;
    private float[] unitCurrencyPerSec;
    [SerializeField] private float upgradeOfCurrencyPerSec;

    private int[] coroutineStartTime;


    private GameManager()
    {
        Currency = 0;
        currencyPerClick = 1;
        unitCoroutine = new Coroutine[1];
        currencyPerSec = new float[1];
        unitCurrencyPerSec = new float[1];
        coroutineStartTime = new int[1];
        upgradeOfCurrencyPerSec=1;
    }
    private void Awake()
    {
        ClickOnUnitActionButton = ClickUnitActionButton;
        ClickOnUnitProdButton = ClickUnitProdButton;
        ClickOnUpgradeButton = ClickUpgradeButton;
        CurrencyClick = Click;
        if(File.Exists(Application.persistentDataPath + "/savefile.json"))
            Load();
        currencyText.text = $"{(long)Currency} vp";
        currencyPerSecText.text = $"vp/sec: {currencyPerSec.Sum()}";
        currencyPerClickText.text = currencyPerClick.ToString();
    }
    private void Click()
    {
        Currency += currencyPerClick;
        OnCurrencyChange();
    }

    private void ClickUnitActionButton(float multiplier, float cost)
    {
        currencyPerClick += multiplier;
        currencyPerClickText.text = currencyPerClick.ToString();
        Currency -= cost;
        OnCurrencyChange();
    }
    private void ClickUnitProdButton(float currencyPerSec, float cost, int i)
    {
        ResizeArrays(i);
        unitCurrencyPerSec[i] += currencyPerSec;
        ButtonClick(cost, i);
    }
    private void ClickUpgradeButton(float currencyPerSecMultiplier, float cost, int i)
    {
        upgradeOfCurrencyPerSec = currencyPerSecMultiplier;
        ButtonClick(cost, i);
    }
    private void ButtonClick(float cost, int i)
    {
        currencyPerSec[i] = unitCurrencyPerSec[i] * upgradeOfCurrencyPerSec;
        UnitCoroutineStart(i);
        Currency -= cost;
        OnCurrencyChange();
        currencyPerSecText.text = $"vp/sec: {currencyPerSec.Sum()}";
    }

    private void OnCurrencyChange()    //нужно вызывать при каждом изменении currency
    {
        currencyText.text = $"{(long)Currency} vp";
        OnCurrencyHasChanged();
    }

    private void ResizeArrays(int i)
    {
        if (unitCoroutine.Length <= i)
        {
            unitCoroutine = new Coroutine[i + 1];
            Array.Resize(ref currencyPerSec, i + 1);
            Array.Resize(ref unitCurrencyPerSec, i + 1);
            Array.Resize(ref coroutineStartTime, i + 1);
        }
    }


    //COROUTINE
    private void UnitCoroutineStart(int i)
    {
        if (unitCoroutine[i] == null) 
        { 
            coroutineStartTime[i] = DateTime.Now.Millisecond;
            unitCoroutine[i] = StartCoroutine(UnitCoroutine(i)); 
        }
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
            OnCurrencyChange();
            //Debug.Log("Unit = " + currencyPerSec[i]);
        }
    }

    
    //СИСТМЕМА SAVE И LOAD
    public void Save()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        SaveSystem.SaveGame(Currency, currencyPerClick, currencyPerSec, unitCurrencyPerSec, coroutineStartTime);
    }
    public void Load()
    {
        SaveData data = SaveSystem.LoadGame();

        unitCoroutine = new Coroutine[data.savedCurrencyPerSec.Length];  //восстановление размера массивов
        unitCurrencyPerSec = new float[data.savedCurrencyPerSec.Length];
        coroutineStartTime = new int[data.savedCurrencyPerSec.Length];


        Currency = data.savedCurrency;
        currencyPerClick = data.toSavedCurrency;
        currencyPerSec = data.savedCurrencyPerSec;
        unitCurrencyPerSec = data.savedUnitCurrencyPerSec;

        for (int i =0; i < currencyPerSec.Length; i++)
        {
            if (unitCurrencyPerSec[i] != 0) 
            { 
                Debug.Log(data.coroutineStartTime[i]);
                unitCoroutine[i] = StartCoroutine(UnitCoroutine(i));   //Запуск уже существующих корутинов
            }
        }

    }
#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        Save();
    }
#else
    private void OnApplicationPause()
    {
        Save();
    }
#endif
}
