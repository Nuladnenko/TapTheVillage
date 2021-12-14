using UnityEngine;

public class ActivateUpgrade : Activate
{
    public delegate void LevelChange(int index, int level);
    public static LevelChange levelChange;

    public static event IsActivated IsUpgradeActivated;

    private UpgradesDataContainer upgradesData;

    private void Start()
    {
        savePath = Application.persistentDataPath + "/savefile.json";
        upgradesData = gameObject.GetComponent<UpgradesDataContainer>();

        CheckButtonState();

        levelChange = ChekcLevel;

        activatedObject.SetActive(false);
    }
    
    private void ChekcLevel(int index, int level)
    {
        if (level == 5)
            Activate(index);
        else if (level == 10)
            Activate(index + upgradesData.UpgradeObjects.Length);
    }


    private void Activate(int index)
    {
        bool b = upgradesData.UpgradeObjects[index].activeSelf == false;
        bool c = activatedObject.activeInHierarchy == false;
        if (b)
        {
            IsUpgradeActivated(index);
            if (c)
                notification.SetActive(true);
        }
    }

    private void CheckButtonState()
    {
        for (int i = 0; i < upgradesData.UpgradeObjects.Length; i++)
        {
            if (upgradesData.IsActivated[i])
                upgradesData.UpgradeObjects[i].SetActive(true);
            else
                upgradesData.UpgradeObjects[i].SetActive(false);
        }
    }

}
