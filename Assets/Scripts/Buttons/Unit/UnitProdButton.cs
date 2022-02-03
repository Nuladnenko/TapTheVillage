using UnityEngine;


public class UnitProdButton : UnitButton
{
    [SerializeField] private int id;
    [SerializeField] private float currencyPerSec;
    private bool[] isUnitBought;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Click()
    {
        if (GameManager.Currency >= Cost)
        {
            GameManager.ClickOnUnitProdButton(currencyPerSec, Cost, id);  //���������� ������ �������� � GameManager
            base.Click();
            ActivateUpgrade.levelChange(id, Level);
            SaveUnitProdState();
        }
    }


    protected override void Load()
    {
        SaveData data = SaveSystem.Load(savePath);
        Cost = data.unitsCost[id];
        Level = data.unitsLevel[id];
        isUnitBought = data.isUnitBought;
    }
    protected void SaveUnitProdState()
    {
        if(isUnitBought == null)
            SaveSystem.SaveUnitProdState(id);
        else if (isUnitBought.Length<=id || !isUnitBought[id])
            SaveSystem.SaveUnitProdState(id);
    }
}
