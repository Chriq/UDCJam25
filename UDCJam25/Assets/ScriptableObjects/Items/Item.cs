using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    // Item Characteristics
    [SerializeField] public ItemScriptable item_stats;

    // Combat Status
    public int cooldown_timer;
    public int ammo;

    // Call at start of scene/combat to set cd and ammo
    public void Init()
    {
        if (item_stats == null)
            Debug.LogError("Item has not been given an ItemScriptable reference");

        ammo = item_stats.ammunition;
        cooldown_timer = 0;
    }

    // Use the item on the given target
    public int Use(CombatEntity self, CombatEntity target)
    {
        ammo--;
        cooldown_timer = item_stats.cooldown_period;

        if (Random.Range(0, 1) > item_stats.success_probability)
        {
            return item_stats.use_cost;
        }

        if ((item_stats.item_effects & (int)ItemEffects.Damage_Target) != 0)
        {
            target.Damage(item_stats.value_target);
        }
        if ((item_stats.item_effects & (int)ItemEffects.Damage_Self) != 0)
        {
            self.Damage(item_stats.value_self);
        }
        if ((item_stats.item_effects & (int)ItemEffects.Heal_Target) != 0)
        {
            target.Heal(item_stats.value_target);
        }
        if ((item_stats.item_effects & (int)ItemEffects.Heal_Self) != 0)
        {
            self.Heal(item_stats.value_self);
        }
        if ((item_stats.item_effects & (int)ItemEffects.Stun_Target) != 0)
        {
            target.Stun();
        }
        if ((item_stats.item_effects & (int)ItemEffects.Stun_Self) != 0)
        {
            self.Stun();
        }
        if ((item_stats.item_effects & (int)ItemEffects.Shield_Target) != 0)
        {
            target.Shield(item_stats.value_target);
        }
        if ((item_stats.item_effects & (int)ItemEffects.Shield_Self) != 0)
        {
            self.Shield(item_stats.value_self);
        }

        return item_stats.use_cost;
    }

    // Update cooldown - to be called at the start of every turn
    public void Cycle()
    {
        if (cooldown_timer <= 0)
            return;

        switch (ammo)
        {
            case 0:
                ammo++;
                cooldown_timer = item_stats.reload_period;
                break;
            default:
                cooldown_timer--;
                break;
        }
    }
}
