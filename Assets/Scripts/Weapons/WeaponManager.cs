using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Statics")]
    [SerializeField] private List<WeaponSO> weaponIndex;
    [SerializeField] private Transform playerArm;

    private WeaponSO currentWeaponSO;
    private GameObject currentWeapon;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

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
        currentWeapon = Instantiate(newWeaponSO.weaponPrefab, playerArm);
        currentWeapon.name = newWeaponSO.weaponPrefab.name;
        //currentWeapon.transform.localPosition = Vector3.zero;
    }
}
