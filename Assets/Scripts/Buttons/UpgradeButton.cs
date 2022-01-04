using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MainButton
{
    [SerializeField] private int currencyPerSecMultiplier;
    [SerializeField] protected int indexOfUnit;

    protected override void Awake()
    {
        base.Awake();
        //AssignText();
        button.onClick.AddListener(Click);
    }

    private void Click()
    {
        if (GameManager.Currency >= Cost)
        {
            GameManager.ClickOnUpgradeButton(currencyPerSecMultiplier, Cost, indexOfUnit);  //Set variables to delegate
            UpCost();
        }
    }
}
