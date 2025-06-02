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
    public AmmoUI ammoUI;

    private int totalAmmo;
    private int totalMagazines;
    private bool isReloading = false;
    public AudioClip shootSound;
    private AudioSource audioSource;

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
        audioSource = GetComponent<AudioSource>();
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

        if (audioSource != null && shootSound != null)
        audioSource.PlayOneShot(shootSound);

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        Debug.DrawRay(ray.origin, ray.direction * weaponData.range, Color.red, 1f); //Debug for seeing where the raycast hits

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, weaponData.range))
        {
            Debug.Log("Ray hit: " + hit.transform.name);
            IDamageable target = hit.transform.GetComponentInParent<IDamageable>();

            if (target != null)
            {
                target.TakeDamage(weaponData.damage);
                Debug.Log("Sköt fiende: " + weaponData.damage + " skada");

                if (bloodEffect != null)
                {
                    GameObject blood = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(blood, 2f);
                }

                DismemberPart(hit.transform);
            }
        }

        if (animator != null)
        {
            animator.SetBool("Shoot", true);
            StartCoroutine(ResetBool("Shoot", 1f / weaponData.fireRate));
        }
    }
    public void AddMagazine(int amount)
    {
        int ammoToAdd = amount * weaponData.maxAmmo;
        totalAmmo += ammoToAdd;

        Debug.Log($"Plockade upp ammo! Lade till {ammoToAdd} skott. Nuvarande ammo: {currentAmmo}/{totalAmmo}");

        if (ammoUI != null)
            ammoUI.UpdateAmmoText();
    }



    void DismemberPart(Transform hitTransform)
    {
        // List of allowed bones for dismemberment
        string[] allowedLimbs = { "upperarm_l", "lowerarm_l", "upperarm_r", "lowerarm_r", "Head" };

        bool canDismember = false;
        foreach (var limb in allowedLimbs)
        {
            if (hitTransform.name.ToLower().Contains(limb.ToLower()))
            {
                canDismember = true;
                break;
            }
        }

        if (!canDismember)
        {
            Debug.Log("Hit part is not dismemberable: " + hitTransform.name);
            return;
        }

        // Recursive function to remove all visible mesh parts in subtree
        void RemoveMeshParts(Transform current)
        {
            // If it’s a visible mesh part, destroy it
            if (current.GetComponent<MeshRenderer>() != null || current.GetComponent<SkinnedMeshRenderer>() != null)
            {
                if (bloodEffect != null)
                {
                    GameObject blood = Instantiate(bloodEffect, current.position, Quaternion.identity);
                    Destroy(blood, 2f);
                }

                Destroy(current.gameObject);
                Debug.Log("Removed mesh part: " + current.name);
                return; // Stop here, don't go deeper if this mesh is removed
            }

            // Otherwise, go deeper
            foreach (Transform child in current)
            {
                RemoveMeshParts(child);
            }
        }

        RemoveMeshParts(hitTransform);
    }



    IEnumerator ResetBool(string param, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (animator != null)
            animator.SetBool(param, false);
    }
    
}
