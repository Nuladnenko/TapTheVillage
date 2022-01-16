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
        SaveSystem.SaveUnit(unitsData.UnitProdCost, unitsData.UnitProdLevel, unitsData.IsActivated);
        SaveSystem.SaveUnitClick(unitsData.UnitActionCost, unitsData.UnitActionLevel);
    }
#else
    private void OnApplicationPause()
    {
        SaveSystem.SaveUnit(unitsData.UnitProdCost, unitsData.UnitProdLevel, unitsData.IsActivated);
        SaveSystem.SaveUnitClick(unitsData.UnitActionCost, unitsData.UnitActionLevel);
    }
#endif
}
