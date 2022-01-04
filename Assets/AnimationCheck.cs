using UnityEngine;

public class AnimationCheck : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void OnAnimationEnded()
    {
        animator.SetBool("Clicked", false);
    }
}
