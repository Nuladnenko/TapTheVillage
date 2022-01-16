using UnityEngine;
using System.IO;

public class UpgradesDataContainer : MonoBehaviour
{
    [SerializeField] private GameObject[] upgradeObjectsGroup1;
    
    [SerializeField] private GameObject[] upgradeObjectsGroup2;
    [SerializeField] private UnitsDataContainer unitDataContainer;
    private string savePath;

    public GameObject[] UpgradeObjectsGroup1
    {
        get { return upgradeObjectsGroup1; }
        private set { upgradeObjectsGroup1 = value; }
    }
     public GameObject[] UpgradeObjectsGroup2
    {
        get { return upgradeObjectsGroup2; }
        private set { upgradeObjectsGroup2 = value; }
    }
    public int UnitButtonLength { get; private set; }
    public int UpgradeButtonLength { get; private set; }
    public bool[] isGroup1Activated { get; private set; }
    public bool[] isGroup2Activated { get; private set; }


    private void Awake()
    {
        savePath = Application.persistentDataPath + "/savefile.json";

        isGroup1Activated = new bool[UpgradeObjectsGroup1.Length];
        isGroup2Activated = new bool[UpgradeObjectsGroup2.Length];

        if (File.Exists(savePath))
            Load();
        UnitButtonLength = unitDataContainer.UnitButtonLength;
        ActivateUpgrade.IsGroup1Activated += ActivateGroup1Button;
        ActivateUpgrade.IsGroup2Activated += ActivateGroup2Button;
    }

    private void ActivateGroup1Button(int index)
    {
        UpgradeObjectsGroup1[index].SetActive(true);
        isGroup1Activated[index] = true;
    }
    private void ActivateGroup2Button(int index)
    {
        UpgradeObjectsGroup2[index].SetActive(true);
        isGroup2Activated[index] = true;
    }
    private void Load()
    {
        SaveData data = SaveSystem.Load(savePath);
        isGroup1Activated = data.isUpgradeGroup1Activated;
    }

    private void OnDestroy()
    {
        ActivateUpgrade.IsGroup1Activated -= ActivateGroup1Button;
        ActivateUpgrade.IsGroup2Activated -= ActivateGroup2Button;
    }

}