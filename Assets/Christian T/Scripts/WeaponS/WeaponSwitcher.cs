using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public GameObject[] weapons;
    private int currentWeaponIndex = 0;

    void Start()
    {
        SelectWeapon(currentWeaponIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SelectWeapon(0); // Kniv

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SelectWeapon(1); // Pistol
    }

    void SelectWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length)
            return;

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == index);
        }

        currentWeaponIndex = index;
    }
}
