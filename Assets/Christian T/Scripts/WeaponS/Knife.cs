using UnityEngine;

public class Knife : MonoBehaviour
{
    public WeaponData weaponData;
    public Animator animator;

    private float nextAttackTime = 0f;

    void Update()
    {
        if (Time.time >= nextAttackTime && Input.GetButtonDown("Fire1"))
        {
            Attack();
            nextAttackTime = Time.time + 1f / weaponData.fireRate;
        }
    }

    void Attack()
    {
        if (animator != null)
            animator.SetTrigger("Melee");

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, weaponData.range))
        {
            IDamageable target = hit.transform.GetComponentInParent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(weaponData.damage);
            }
        }
    }

}
