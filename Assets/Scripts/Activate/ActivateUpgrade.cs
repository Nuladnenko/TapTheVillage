using UnityEngine;

public class ActivateUpgrade : Activate
{
    public delegate void LevelChange(int index, int level);
    public static LevelChange levelChange;

    public static event IsActivated IsGroup1Activated;
    public static event IsActivated IsGroup2Activated;

    private UpgradesDataContainer upgradesDataCon;

    private void Start()
    {
        savePath = Application.persistentDataPath + "/savefile.json";
        upgradesDataCon = gameObject.GetComponent<UpgradesDataContainer>();

        CheckButtonState();

        levelChange = ChekcLevel;

        activatedObject.SetActive(false);
    }
    
    private void ChekcLevel(int index, int level)
    {
        if (level == 5)
            ActivateGroup1(index);
        else if (level == 10)
            ActivateGroup2(index);
    }


    private void ActivateGroup1(int index)
    {
        bool b = upgradesDataCon.UpgradeObjectsGroup1[index].activeSelf == false;
        bool c = activatedObject.activeInHierarchy == false;
        if (b)
        {
            IsGroup1Activated(index);
            if (c)
                notification.SetActive(true);
        }
    }
    private void ActivateGroup2(int index)
    {
        bool b = upgradesDataCon.UpgradeObjectsGroup2[index].activeSelf == false;
        bool c = activatedObject.activeInHierarchy == false;
        if (b)
        {
            IsGroup2Activated(index);
            if (c)
                notification.SetActive(true);
        }
    }

    private void CheckButtonState()
    {
        for (int i = 0; i < upgradesDataCon.UpgradeObjectsGroup1.Length; i++)
        {
            if (upgradesDataCon.isGroup1Activated[i])
                upgradesDataCon.UpgradeObjectsGroup1[i].SetActive(true);
            else
                upgradesDataCon.UpgradeObjectsGroup1[i].SetActive(false);
        }
        for (int i = 0; i < upgradesDataCon.UpgradeObjectsGroup2.Length; i++)
        {
            if (upgradesDataCon.isGroup2Activated[i])
                upgradesDataCon.UpgradeObjectsGroup2[i].SetActive(true);
            else
                upgradesDataCon.UpgradeObjectsGroup2[i].SetActive(false);
        }
    }

}
