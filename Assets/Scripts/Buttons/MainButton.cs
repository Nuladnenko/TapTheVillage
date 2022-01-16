using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainButton : MonoBehaviour
{
    [SerializeField] protected string titleText;
    [SerializeField] protected string descriptionText;
    [SerializeField] protected double cost;
    public double Cost
    {
        get { return cost; }
        protected set { cost = value; }
    }

    [SerializeField] protected float costMultiplier;
    [SerializeField] protected Sprite avatar;

    protected TextMeshProUGUI[] textsTMP;
    protected Image image;
    protected Image avatarImage;
    protected Button button;

    protected string savePath;
    protected Color positiveColor { get ; private set ; }
    protected Color negativeColor { get ; private set ; }
    protected Color positiveTextColor { get; private set; }
    protected Color negativeTextColor { get; private set; }
    protected Color positiveCostTextColor { get; private set; }
    protected Color negativeCostTextColor { get; private set; }

    protected MainButton()
    {
        positiveColor = new Color(1, 1, 1);
        negativeColor = new Color(0.7f, 0.7f, 0.7f);
        positiveTextColor = new Color(1f, 1f, 1f);
        negativeTextColor = new Color(0.2f, 0.2f, 0.2f);
        positiveCostTextColor = new Color(0.7f, 0.7f, 0.7f);
        negativeCostTextColor = new Color(0.5f, 0.1f, 0.1f);
    }
    protected virtual void Awake()
    {

        button = GetComponent<Button>();
        image = GetComponent<Image>();

        GameManager.OnCurrencyHasChanged += CheckColor;
        AssignAvatar();
        AssignText();
        CheckColor();
    }

    protected void AssignAvatar()
    {
        avatarImage = gameObject.transform.GetChild(0).GetComponent<Image>();
        avatarImage.sprite = avatar;
    }
    protected virtual void AssignText()
    {
        textsTMP = GetComponentsInChildren<TextMeshProUGUI>();
        textsTMP[0].text = titleText;
        textsTMP[1].text = descriptionText;
        textsTMP[2].text = NumberFormatter.FormatNumTens(Cost);
    }
    protected void UpCost()
    {
        Cost *= costMultiplier;
        Cost = (int)Cost;

        textsTMP[2].text = NumberFormatter.FormatNumTens(Cost);
    }

   
    protected void CheckColor()
    {
        if (GameManager.Currency < Cost)
        {
            image.color = negativeColor;
            for (int i=0; i < textsTMP.Length; i++)
            {
                textsTMP[i].color = negativeTextColor;
                if (i == 2)                                 //������ ������ ���� ������ ���������
                    textsTMP[i].color = negativeCostTextColor;
            }
            avatarImage.color = negativeColor;
        }
        else
        {
            image.color = positiveColor;
            for (int i = 0; i < textsTMP.Length; i++)
            {
                textsTMP[i].color = positiveTextColor;
                if (i == 2)                                 //������ ������ ���� ������ ���������
                    textsTMP[i].color = positiveCostTextColor;
            }
            avatarImage.color = positiveColor;
        }         
    }
    protected void OnDestroy()
    {
        GameManager.OnCurrencyHasChanged -= CheckColor;
    }

}
