using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    public Weapon weapon;
    public TextMeshProUGUI ammoText;

    void Update()
    {
        if (weapon != null && ammoText != null)
        {
            ammoText.text = $"{weapon.GetCurrentAmmo()} / {weapon.GetTotalAmmo()}";
        }
    }
    public void UpdateAmmoText()
    {
        ammoText.text = weapon.GetCurrentAmmo() + " / " + weapon.GetTotalAmmo();
    }
}
