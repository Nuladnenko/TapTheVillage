using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeActionButton : MainButton
{
    protected override void Awake()
    {
        base.Awake();
        button.onClick.AddListener(Click);
    }

    private void Click()
    {
        if (GameManager.Currency >= Cost)
        {
            GameManager.ClickOnUpgradeActionButton(Cost);  //Set variables to delegate
            UpCost();
        }
    }

}
