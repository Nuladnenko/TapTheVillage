using UnityEngine;
using UnityEngine.UI;

public class CurrencyClick : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem particle;
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(Click);
    }
    private void Click()
    {
        animator.SetBool("Clicked", true);
        GameManager.CurrencyClick();
        Instantiate<ParticleSystem>(particle);
    }
}
