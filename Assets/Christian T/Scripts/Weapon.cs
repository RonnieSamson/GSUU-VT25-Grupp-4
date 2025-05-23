using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData weaponData;
    public Transform firePoint;
    public Animator animator;


    private float nextFireTime = 0f; //FIXA?

    public GameObject muzzleFlashPrefab;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / weaponData.fireRate;

            if (weaponData.isMelee)
            {
                MeleeAttack();
            }
            else
            {
                Shoot();
            }

            if (animator != null)
                animator.SetTrigger("Shoot");
        }
    }

    void Shoot()
{
    // Raycast från kameran – dit spelaren tittar
    Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
    RaycastHit hit;

    if (Physics.Raycast(ray, out hit, weaponData.range))
    {
        IDamageable target = hit.transform.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(weaponData.damage);
            Debug.Log("Zombie took damage: " + weaponData.damage);
        }
        else
        {
            Debug.Log("Träffade: " + hit.collider.name);
        }
    }

    // Spela animation
    if (animator != null)
        animator.SetTrigger("Shoot");
}


    void MeleeAttack()
    {
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, weaponData.range))
        {
            IDamageable target = hit.transform.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(weaponData.damage);
            }
        }
    }
}
