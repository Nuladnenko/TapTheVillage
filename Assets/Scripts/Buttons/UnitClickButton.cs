using UnityEngine;



public class UnitClickButton : UnitButton
{
    [SerializeField] private float addedCurrency;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Click()
    {
        if (GameManager.Currency >= Cost)
        {
            GameManager.ClickOnJohnyButton(addedCurrency, Cost);  //Set variables to delegate
            base.Click();
        }
    }
    protected override void AssignText()
    {
        base.AssignText();
        textsTMP[3].text = "lv: " + Level.ToString();
    }

    protected override void Load()
    {
        SaveData data = SaveSystem.Load(savePath);
        Cost = data.unitClickCost;
        Level = data.unitClickLevel;
    }
}
