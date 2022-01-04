using UnityEngine;
using System.IO;

public class UpgradesDataContainer : MonoBehaviour
{
    [SerializeField] private GameObject[] upgradeObjects;
    [SerializeField] private UnitsDataContainer unitDataContainer;
    private string savePath;

    public GameObject[] UpgradeObjects
    {
        get { return upgradeObjects; }
        private set { upgradeObjects = value; }
    }
    public int UnitButtonLength { get; private set; }
    public int UpgradeButtonLength { get; private set; }
    public bool[] IsActivated { get; private set; }


    private void Awake()
    {
        savePath = Application.persistentDataPath + "/savefile.json";

        IsActivated = new bool[UpgradeObjects.Length];

        if (File.Exists(savePath))
            Load();
        UnitButtonLength = unitDataContainer.UnitButtonLength;
        ActivateUpgrade.IsUpgradeActivated += ActivateButton;
    }

    private void ActivateButton(int index)
    {
        UpgradeObjects[index].SetActive(true);
        IsActivated[index] = true;
    }
    private void Load()
    {
        SaveData data = SaveSystem.Load(savePath);
        IsActivated = data.isUpgradeActivated;
    }

    private void OnDestroy()
    {
        ActivateUpgrade.IsUpgradeActivated -= ActivateButton;
    }

}