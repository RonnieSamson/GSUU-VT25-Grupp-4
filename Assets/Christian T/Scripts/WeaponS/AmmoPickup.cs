using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 1; 

    private void OnTriggerEnter(Collider other)
    {
        Weapon weapon = other.GetComponentInChildren<Weapon>();
        if (weapon != null)
        {
            weapon.AddMagazine(ammoAmount);
            Debug.Log("Plockade upp ammo!");
            Destroy(gameObject); 
        }
    }
}
