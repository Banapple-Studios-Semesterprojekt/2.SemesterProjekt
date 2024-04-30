using UnityEngine;

public class BlackScreen : Singleton<BlackScreen>
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetBlackScreen(bool isBlack)
    {
        animator.SetBool("isBlack", isBlack);
    }
}