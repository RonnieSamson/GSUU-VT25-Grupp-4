using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public WeaponData weaponData;
    public Transform firePoint;
    public Animator animator;

    private float nextFireTime = 0f;
    private int currentAmmo;

    private int totalAmmo; 
    private bool isReloading = false;
    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public int GetTotalAmmo()
    {
        return totalAmmo;
    }


    void Start()
    {
        currentAmmo = weaponData.maxAmmo;
        totalAmmo = weaponData.maxAmmo * 2;
    }

    void Update()
    {
        if (isReloading)
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            if (currentAmmo <= 0)
            {
                Debug.Log("Out of ammo!");
                return;
            }

            nextFireTime = Time.time + 1f / weaponData.fireRate;
            Shoot();
        }
    }

    IEnumerator Reload()
{
    isReloading = true;
    Debug.Log("Reloading...");
    
    if (animator != null)
        animator.SetBool("Reload", true);

    yield return new WaitForSeconds(weaponData.reloadTime);

    int ammoNeeded = weaponData.maxAmmo - currentAmmo;
    int ammoToReload = Mathf.Min(ammoNeeded, totalAmmo);

    currentAmmo += ammoToReload;
    totalAmmo -= ammoToReload;

    isReloading = false;

    if (animator != null)
        animator.SetBool("Reload", false);

    Debug.Log("Reload klar! Ammo: " + currentAmmo + "/" + totalAmmo);
}


    void Shoot()
    {
        currentAmmo--;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, weaponData.range))
        {
            IDamageable target = hit.transform.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(weaponData.damage);
                Debug.Log("Sköt fiende: " + weaponData.damage + " skada");
            }
        }

        if (animator != null)
        {
            animator.SetBool("Shoot", true);
            StartCoroutine(ResetBool("Shoot", 1f / weaponData.fireRate));
        }
    }

    IEnumerator ResetBool(string param, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (animator != null)
            animator.SetBool(param, false);
    }
}
