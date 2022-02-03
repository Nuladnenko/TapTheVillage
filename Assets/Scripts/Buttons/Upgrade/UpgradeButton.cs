using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MainButton
{
    [SerializeField] private int currencyPerSecMultiplier;
    [SerializeField] protected int indexOfUpgUnit;
    [SerializeField] protected int numberOfGroup;


    protected override void Awake()
    {
        base.Awake();
        button.onClick.AddListener(Click);
    }

    private void Click()
    {
        if (GameManager.Currency >= Cost)
        {
            GameManager.ClickOnUpgradeProdButton(currencyPerSecMultiplier, Cost, indexOfUpgUnit);  //Set variables to delegate
            UpCost();
            SaveSystem.SaveUpgradeProdState(indexOfUpgUnit, numberOfGroup);
            UpgradesDataContainer.DeactivateUpg(indexOfUpgUnit,numberOfGroup);
            gameObject.SetActive(false);
        }
    }
}
