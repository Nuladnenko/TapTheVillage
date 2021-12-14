using UnityEngine;

public class SaveUnits : MonoBehaviour
{

    private UnitsDataContainer unitsData;

    private void Start()
    {
        unitsData = gameObject.GetComponent<UnitsDataContainer>();
    }

    #if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        SaveSystem.SaveUnit(unitsData.UnitCost, unitsData.UnitLevel, unitsData.IsActivated);
        SaveSystem.SaveUnitClick(unitsData.UnitClickCost, unitsData.UnitClickLevel);
    }
#else
    private void OnApplicationPause()
    {
        SaveSystem.SaveUnit(unitsDataContainer.Cost, unitsDataContainer.Level, unitsDataContainer.IsActivated);
        SaveSystem.SaveJohny(unitsDataContainer.UnitClickCost, unitsDataContainer.UnitClickLevel);
    }
#endif
}
