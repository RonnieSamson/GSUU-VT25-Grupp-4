using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapon Data", order = 1)]
public class WeaponData : ScriptableObject
{
    public float damage = 20f;
    public float range = 100f;
    public float fireRate = 2f;
    public float reloadTime = 1.5f;

    public int maxAmmo = 10;
    public bool isMelee = false;
}
