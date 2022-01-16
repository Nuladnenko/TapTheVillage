using UnityEngine;
using System.IO;

public class UnitsDataContainer : MonoBehaviour   //�������� � ������ ��� ���������� �� Units
{
    [SerializeField] private GameObject unitActionObject;
    [SerializeField] private GameObject[] unitProdObjects;

    private UnitButton[] unitProdButton;
    private UnitActionButton unitActionButton;
    private string savePath;

    public GameObject[] UnitObjects
    {
        get { return unitProdObjects; }
        private set { unitProdObjects = value; }
    }
    public double[] UnitProdCost { get; private set; }
    public int[] UnitProdLevel { get; private set; }
    public double UnitActionCost { get; private set; }
    public int UnitActionLevel { get; private set; }
    public bool[] IsActivated { get; private set; }
    public int UnitButtonLength { get; private set; }


    private void Awake()
    {
        savePath = Application.persistentDataPath + "/savefile.json";

        SetArraySize();

        if (File.Exists(savePath))
            Load();
   
        for (int i = 0; i < unitProdObjects.Length; i++)
            unitProdButton[i] = unitProdObjects[i].GetComponent<UnitButton>();
        unitActionButton = unitActionObject.GetComponent<UnitActionButton>();
        GetButtonData();
        ActivateUnit.IsUnitActivated += ActivateButton;
    }

    private void SetArraySize()
    {
        unitProdButton = new UnitButton[unitProdObjects.Length];
        UnitProdCost = new double[unitProdObjects.Length];
        UnitProdLevel = new int[unitProdObjects.Length];
        IsActivated = new bool[unitProdObjects.Length];

    }

    protected void GetButtonData()
    {
        for (int i = 0; i < unitProdObjects.Length; i++)
        {
            UnitProdCost[i] = unitProdButton[i].Cost;
            UnitProdLevel[i] = unitProdButton[i].Level;
        }
        UnitActionCost = unitActionButton.Cost;
        UnitActionLevel = unitActionButton.Level;
        UnitButtonLength = unitProdObjects.Length;
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
