using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponIDs : int
{
    SoulBlade = 0,
    SpiritSash,
    SurgeOrb,
    SorpalWing
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
            EquipWeapon((int)WeaponIDs.SoulBlade);
        }
    }

    private void InitialiseWeapons()
    {
        weaponRefs.Insert((int)WeaponIDs.SoulBlade, (GameObject)Resources.Load("weapons/SoulBlade", typeof(GameObject)));
        //weaponRefs.Insert((int)WeaponIDs.SpiritSash, (GameObject)Resources.Load("weapons/SpiritSash", typeof(GameObject)));
        //weaponRefs.Insert((int)WeaponIDs.SurgeOrb, (GameObject)Resources.Load("weapons/SurgeOrb", typeof(GameObject)));
        //weaponRefs.Insert((int)WeaponIDs.VorpalWing, (GameObject)Resources.Load("weapons/VorpalWing", typeof(GameObject)));
    }

    public void EquipWeapon(int _ID)
    {
        Destroy(equippedWeapon1);

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
