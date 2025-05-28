using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public WeaponData weaponData;
    public Transform firePoint;
    public Animator animator;
    public GameObject bloodEffect;

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
            // Kolla om magasinet redan är fullt
            if (currentAmmo < weaponData.maxAmmo && totalAmmo > 0)
            {
                StartCoroutine(Reload());
            }
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
        Debug.DrawRay(ray.origin, ray.direction * weaponData.range, Color.red, 1f); //Debug for seeing where the raycast hits

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, weaponData.range))
        {
            Debug.Log("Ray hit: " + hit.transform.name);
            IDamageable target = hit.transform.GetComponentInParent<IDamageable>(); //Travel up the hierarchy to find the IDamageable in the Zombie root, as long as it hits the zombie

            if (target != null)
            {
                target.TakeDamage(weaponData.damage);
                Debug.Log("Sköt fiende: " + weaponData.damage + " skada");

                if (bloodEffect != null)
                {
                    // Spawn the blood at the exact hit point and orient it to match surface
                    GameObject blood = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal)); //Trigger the particle system with the correct rotations based on the raycast - zombie interaction
                    
                    Destroy(blood, 2f); // Auto-destroy after 2 seconds
                }
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
