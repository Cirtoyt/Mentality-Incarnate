using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulBlade : Weapon
{
    private Player player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out DoubtEnemyAI enemy))
        {
            enemy.DealDamage(player.GetWillPower());
        }
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }
}
