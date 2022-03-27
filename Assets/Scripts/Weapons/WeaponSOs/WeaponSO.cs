using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public string weaponName;
    public GameObject weaponPrefab;
    public GameObject weaponController;
}
