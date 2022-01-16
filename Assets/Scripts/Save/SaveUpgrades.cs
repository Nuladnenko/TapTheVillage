using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveUpgrades : MonoBehaviour
{
    private UpgradesDataContainer upgradesData;
    void Start()
    {
        upgradesData = gameObject.GetComponent<UpgradesDataContainer>();
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
    private void Save()
    {
        SaveSystem.SaveUpgrade(upgradesData);
    }
}
