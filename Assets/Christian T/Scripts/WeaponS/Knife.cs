using UnityEngine;

public class Knife : MonoBehaviour
{
    public WeaponData weaponData;
    public Animator animator;

    public AudioClip slashSound;      // Spelas vid miss
    public AudioClip hitSound;        // Spelas vid träff
    private AudioSource audioSource;

    private float nextAttackTime = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

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

                if (audioSource != null && hitSound != null)
                    audioSource.PlayOneShot(hitSound);
            }
            else
            {
                if (audioSource != null && slashSound != null)
                    audioSource.PlayOneShot(slashSound);
            }
        }
        else
        {
            if (audioSource != null && slashSound != null)
                audioSource.PlayOneShot(slashSound);
        }
    }
}
