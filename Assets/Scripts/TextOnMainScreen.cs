using TMPro;
using System.Linq;
using UnityEngine;

public class TextOnMainScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI currencyPerSecText;
    [SerializeField] private TextMeshProUGUI currencyPerClickText;

    private GameManager gameManager;

    private double oldCurrency;
    private double oldCurrencyPerClick;
    void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();

        currencyText.text = $"{NumberFormatter.FormatNumHunds(GameManager.Currency)} vp";
        currencyPerSecText.text = $"vp/sec: {NumberFormatter.FormatNumTens(gameManager.CurrencyPerSec.Sum())}";
        currencyPerClickText.text = NumberFormatter.FormatNumTens(gameManager.CurrencyPerClick);

        oldCurrency = GameManager.Currency;
        oldCurrencyPerClick = gameManager.CurrencyPerClick;

        GameManager.OnCurrencyHasChanged += OnCurrencyChange;
    }

    private void OnCurrencyChange()
    {
        currencyText.text = $"{NumberFormatter.FormatNumHunds(GameManager.Currency)} vp";
        if (oldCurrency != GameManager.Currency)
        {
            currencyPerSecText.text = $"vp/sec: {NumberFormatter.FormatNumTens(gameManager.CurrencyPerSec.Sum())}";
            oldCurrency = GameManager.Currency;
        }
        if (oldCurrencyPerClick != gameManager.CurrencyPerClick) 
        { 
            currencyPerClickText.text = NumberFormatter.FormatNumTens(gameManager.CurrencyPerClick);
            oldCurrencyPerClick = gameManager.CurrencyPerClick;
        }
    }
}
