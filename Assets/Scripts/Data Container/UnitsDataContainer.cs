using UnityEngine;
using System.IO;

public class UnitsDataContainer : MonoBehaviour   //Собирает и хранит всю информацию по Units
{
    [SerializeField] private GameObject unitClickObject;
    [SerializeField] private GameObject[] unitObjects;

    private UnitButton[] unitButton;
    private UnitClickButton unitClickButton;
    private string savePath;

    public GameObject[] UnitObjects
    {
        get { return unitObjects; }
        private set { unitObjects = value; }
    }
    public float[] UnitCost { get; private set; }
    public int[] UnitLevel { get; private set; }
    public float UnitClickCost { get; private set; }
    public int UnitClickLevel { get; private set; }
    public bool[] IsActivated { get; private set; }
    public int UnitButtonLength { get; private set; }


    private void Awake()
    {
        savePath = Application.persistentDataPath + "/savefile.json";

        SetArraySize();

        if (File.Exists(savePath))
            Load();
   
        for (int i = 0; i < unitObjects.Length; i++)
            unitButton[i] = unitObjects[i].GetComponent<UnitButton>();
        unitClickButton = unitClickObject.GetComponent<UnitClickButton>();
        GetButtonData();
        ActivateUnit.IsUnitActivated += ActivateButton;
    }

    private void SetArraySize()
    {
        unitButton = new UnitButton[unitObjects.Length];
        UnitCost = new float[unitObjects.Length];
        UnitLevel = new int[unitObjects.Length];
        IsActivated = new bool[unitObjects.Length];

    }

    protected void GetButtonData()
    {
        for (int i = 0; i < unitObjects.Length; i++)
        {
            UnitCost[i] = unitButton[i].Cost;
            UnitLevel[i] = unitButton[i].Level;
        }
        UnitClickCost = unitClickButton.Cost;
        UnitClickLevel = unitClickButton.Level;
        UnitButtonLength = unitObjects.Length;
    }
    
    private void ActivateButton(int index)
    {
        UnitObjects[index].SetActive(true);
        IsActivated[index] = true;
    }
    private void Load()
    {
        SaveData data = SaveSystem.Load(savePath);
        IsActivated = data.isUnitActivated;
    }

#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        GetButtonData();
    }
#else
    private void OnApplicationPause()
    {
        GetButtonData();
    }
#endif
    private void OnDestroy()
    {
        ActivateUnit.IsUnitActivated -= ActivateButton;
    }
}
