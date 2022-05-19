using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Statics")]
    [SerializeField] private List<WeaponSO> weaponIndex;

    public WeaponController currentWeaponController { get; private set; }

    private WeaponSO currentWeaponSO;
    private GameObject currentWeapon;

    private void OnTestAction1()
    {
        EquipWeapon(weaponIndex[0]);
    }

    private void EquipWeapon(WeaponSO newWeaponSO)
    {
        // Remove old weapon if there is one
        if (currentWeaponSO != null)
        {
            Destroy(currentWeapon);
        }

        // Add new weapon
        Player player = transform.GetComponent<Player>();
        currentWeaponController = Instantiate(newWeaponSO.weaponController, transform).GetComponent<WeaponController>();
        currentWeaponController.name = newWeaponSO.weaponController.name;
        currentWeapon = Instantiate(newWeaponSO.weaponPrefab, transform.position, Quaternion.identity);
        currentWeapon.name = newWeaponSO.weaponPrefab.name;
        currentWeaponController.SetupController(player, currentWeapon.GetComponent<Weapon>());
    }
}
