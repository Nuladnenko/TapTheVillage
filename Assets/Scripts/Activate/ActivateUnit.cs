//using System.IO;
using UnityEngine;


public class ActivateUnit : Activate   
{                                       

    private UnitsDataContainer unitsData;
    public static event IsActivated IsUnitActivated;

    private void Start()
    {
        savePath = Application.persistentDataPath + "/savefile.json";
        unitsData = gameObject.GetComponent<UnitsDataContainer>();

        CheckButtonState();
        GameManager.OnCurrencyHasChanged += Activate;

        activatedObject.SetActive(false);

    }
  
    private void CheckButtonState()
    {
        for (int i = 0; i < unitsData.UnitObjects.Length; i++)
        {
            if (unitsData.IsActivated[i])
                unitsData.UnitObjects[i].SetActive(true);
            else
                unitsData.UnitObjects[i].SetActive(false);
        }
    }

    private void Activate()
    {
        for (int i = 0; i < unitsData.UnitButtonLength; i++)
        {
            bool a = unitsData.UnitProdCost[i] * 0.7f <= GameManager.Currency;
            bool b = unitsData.UnitObjects[i].activeSelf == false;
            bool c = activatedObject.activeInHierarchy == false;
            if (a && b)
            {
                IsUnitActivated(i);
                if (c)
                    notification.SetActive(true);
            }
            
        }
    }

    public void OnDestroy()
    {
        GameManager.OnCurrencyHasChanged -= Activate;
    }
}
