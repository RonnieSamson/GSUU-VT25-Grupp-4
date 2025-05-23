using UnityEngine;

public class GunShoot : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Shoot");
        }
    }
}

