//using System.IO;
using UnityEngine;


public class ActivateUnit : Activate   
{                                       

    private UnitsDataContainer unitsDataCon;
    public static event IsActivated IsUnitActivated;

    private void Start()
    {
        savePath = Application.persistentDataPath + "/savefile.json";
        unitsDataCon = gameObject.GetComponent<UnitsDataContainer>();

        CheckButtonState();
        GameManager.OnCurrencyHasChanged += Activate;

        activatedObject.SetActive(false);

    }
  
    private void CheckButtonState()
    {
        for (int i = 0; i < unitsDataCon.UnitObjects.Length; i++)
        {
            if (unitsDataCon.IsActivated[i])
                unitsDataCon.UnitObjects[i].SetActive(true);
            else
                unitsDataCon.UnitObjects[i].SetActive(false);
        }
    }

    private void Activate()
    {
        for (int i = 0; i < unitsDataCon.UnitButtonLength; i++)
        {
            bool a = unitsDataCon.UnitProdCost[i] * 0.7f <= GameManager.Currency;
            bool b = unitsDataCon.UnitObjects[i].activeSelf == false;
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
