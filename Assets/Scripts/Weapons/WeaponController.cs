using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class WeaponController : MonoBehaviour
{
    abstract public void SetupController(Transform player, Weapon weapon);

    abstract public void ResetController();
}
