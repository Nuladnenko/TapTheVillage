using TMPro;
using System.Linq;
using UnityEngine;

public class TextOnMainScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI currencyPerSecText;
    [SerializeField] private TextMeshProUGUI currencyPerClickText;

    private GameManager gameManager;

    private float oldCurrency;
    private float oldCurrencyPerClick;
    void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();

        currencyText.text = $"{(long)GameManager.Currency} vp";
        currencyPerSecText.text = $"vp/sec: {gameManager.CurrencyPerSec.Sum()}";
        currencyPerClickText.text = gameManager.CurrencyPerClick.ToString();

        oldCurrency = GameManager.Currency;
        oldCurrencyPerClick = gameManager.CurrencyPerClick;

        GameManager.OnCurrencyHasChanged += OnCurrencyChange;
    }

    private void OnCurrencyChange()
    {
        currencyText.text = $"{(long)GameManager.Currency} vp";
        if (oldCurrency != GameManager.Currency)
        {
            currencyPerSecText.text = $"vp/sec: {gameManager.CurrencyPerSec.Sum()}";
            oldCurrency = GameManager.Currency;
        }
        if (oldCurrencyPerClick != gameManager.CurrencyPerClick) 
        { 
            currencyPerClickText.text = gameManager.CurrencyPerClick.ToString();
            oldCurrencyPerClick = gameManager.CurrencyPerClick;
        }
    }
}
