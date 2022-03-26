using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponIDs
{
    soulBlade = 0,
    spiritSash,
    umbraOrb,
    wraithWhip,
    vorpalWing
}

public class InventoryManager : MonoBehaviour
{
    public GameObject equippedWeapon1;

    private GameObject player;
    private PlayerIncarnation playerIncScript;

    private List<GameObject> weaponRefs = new List<GameObject>();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerIncScript = player.GetComponent<PlayerIncarnation>();

        InitialiseWeapons();
    }
    
    void Update()
    {
        // Test button
        if (Input.GetKeyDown("g"))
        {
            EquipWeapon((int)WeaponIDs.soulBlade);
        }
    }

    private void InitialiseWeapons()
    {
        weaponRefs.Insert((int)WeaponIDs.soulBlade, (GameObject)Resources.Load("weapons/SoulBlade", typeof(GameObject)));
        //weaponRefs.Insert((int)WeaponIDs.spiritSash, (GameObject)Resources.Load("weapons/SpiritSash", typeof(GameObject)));
        //weaponRefs.Insert((int)WeaponIDs.umbraOrb, (GameObject)Resources.Load("weapons/UmbraOrb", typeof(GameObject)));
        //weaponRefs.Insert((int)WeaponIDs.wraithWhip, (GameObject)Resources.Load("weapons/WraithWhip", typeof(GameObject)));
        //weaponRefs.Insert((int)WeaponIDs.vorpalWing, (GameObject)Resources.Load("weapons/VorpalWing", typeof(GameObject)));
    }

    public void EquipWeapon(int _ID)
    {
        GameObject.Destroy(equippedWeapon1);

        equippedWeapon1 = Instantiate(weaponRefs[_ID]);
        equippedWeapon1.name = "SoulBlade1";
        equippedWeapon1.tag = "Incarnation1";
        equippedWeapon1.transform.parent = player.transform.Find("Player Arm 1");
        equippedWeapon1.transform.localPosition = Vector3.zero;

        playerIncScript.UpdateIncarnationRefs();
    }

    // Store references to all possible items
    
    // Store other currently equipped items
    // Store the inventory

    // Function to add items to inventory
    // Function to remove items from inventory
}
