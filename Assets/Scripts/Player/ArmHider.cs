using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmHider : MonoBehaviour
{
    public void DoneEndCasting()
    {
        transform.GetComponent<SpriteRenderer>().enabled = false;
    }
}
