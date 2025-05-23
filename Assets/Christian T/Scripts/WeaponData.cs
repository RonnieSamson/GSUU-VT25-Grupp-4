using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapon/Create New Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public float damage;
    public float range;
    public float fireRate;
    public bool isMelee;
}
