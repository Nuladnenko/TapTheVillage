using UnityEngine;


public class UnitProdButton : UnitButton
{
    [SerializeField] private int index;
    [SerializeField] private float currencyPerSec;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Click()
    {
        if (GameManager.Currency >= Cost)
        {
            GameManager.ClickOnUnitProdButton(currencyPerSec, Cost, index);  //���������� ������ �������� � GameManager
            base.Click();
            ActivateUpgrade.levelChange(index, Level);
        }
    }


    protected override void Load()
    {
        SaveData data = SaveSystem.Load(savePath);
        Cost = data.unitsCost[index];
        Level = data.unitsLevel[index];
    }
}
