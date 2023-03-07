using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Effects Enumerator
enum ItemEffects
{
    Damage_Target   = 1 << 0,
    Damage_Self     = 1 << 1,
    Heal_Target     = 1 << 2,
    Heal_Self       = 1 << 3,
    Stun_Target     = 1 << 4,
    Stun_Self       = 1 << 5,
    Shield_Target   = 1 << 6,
    Shield_Self     = 1 << 7,
}

/* https://pavcreations.com/equipment-system-for-an-rpg-unity-game/#The-concepts-of-the-equipment-system */
[CreateAssetMenu(menuName = "ScriptableObjects/GameItem")]
[System.Serializable]
public class Item : ScriptableObject
{
    // Item Characteristics
    public string item_name;
    public Sprite item_icon;

    [SerializeField] [EnumMask(typeof(ItemEffects))] int item_effects;
    [SerializeField] public int cooldown_period;
    [SerializeField] int reload_period;
    [SerializeField] public int ammunition;
    [SerializeField] public float value_target;
    [SerializeField] public float value_self;
    [SerializeField] float success_probability;
    [SerializeField] public int use_range;
    [SerializeField] public int use_cost;

    // Combat Status
    public int cooldown_timer { get; private set; }
    int ammo;

    public int Use(CombatEntity self, CombatEntity target)
    {
        Debug.Log("USE");

        if (cooldown_timer > 0)
            Debug.LogError("Error - using item " + this.name + "/" + item_name + " when on cooldown!");

        ammo--;
        cooldown_timer = cooldown_period;

        if (Random.Range(0, 1) > success_probability)
        {
            return use_cost;
        }

        if ((item_effects & (int)ItemEffects.Damage_Target) != 0)
        {
            target.Damage(value_target);
        }
        if ((item_effects & (int)ItemEffects.Damage_Self) != 0)
        {
            self.Damage(value_self);
        }
        if ((item_effects & (int)ItemEffects.Heal_Target) != 0)
        {
            target.Heal(value_target);
        }
        if ((item_effects & (int)ItemEffects.Heal_Self) != 0)
        {
            self.Heal(value_self);
        }
        if ((item_effects & (int)ItemEffects.Stun_Target) != 0)
        {
            target.Stun();
        }
        if ((item_effects & (int)ItemEffects.Stun_Self) != 0)
        {
            self.Stun();
        }
        if ((item_effects & (int)ItemEffects.Shield_Target) != 0)
        {
            target.Shield(value_target);
        }
        if ((item_effects & (int)ItemEffects.Shield_Self) != 0)
        {
            self.Shield(value_self);
        }

        return use_cost;
    }

    public void Update()
    {
        switch (ammo)
        {
            case 0:
                ammo++;
                cooldown_timer = reload_period;
                break;
            default:
                cooldown_timer--;
                break;
        }
    }
}

/*
 * Magic
 * 
 *      Divinity
 *      
 *      BloodBoil
 
public class Divinity : Item
{
    public Divinity()
    {
        cooldown_period = 8;
        ammo = 2;
    }
    public override void Use(CombatEntity self, CombatEntity target)
    {
        target.Heal(10);
        self.Heal(5);
        cooldown_timer = cooldown_period;
        ammo--;
    }
}
public class BloodBoil : Item
{
    public BloodBoil()
    {
        cooldown_period = 3;
        ammo = 10;
    }

    public override void Use(CombatEntity self, CombatEntity target)
    {
        target.Damage(10);
        self.Damage(5);
        cooldown_timer = cooldown_period;
        ammo--;
    }
}

/*
 * Charms
 * 
 *  PressedDuck
 *  Cornucopia
 *  CursedCrown
 *  Rat
 
public class PressedDuck : Item
{
    public PressedDuck()
    {
        cooldown_period = 1;
        ammo = 2;
    }

    public override void Use(CombatEntity self, CombatEntity target)
    {
        self.Heal(10);
        cooldown_timer = cooldown_period;
        ammo--;
    }
}
public class CursedCrown : Item
{
    public CursedCrown()
    {
        cooldown_period = 1;
        ammo = 20;
    }

    public override void Use(CombatEntity self, CombatEntity target)
    {
        target.Damage(2);
        self.Heal(2);
        cooldown_timer = cooldown_period;
        ammo--;
    }
}
public class Rat : Item
{
    public Rat()
    {
        cooldown_period = 1;
        ammo = 1;
    }

    public override void Use(CombatEntity self, CombatEntity target)
    {
        target.Damage(10);
        target.Stun();
        cooldown_timer = cooldown_period;
        ammo--;
    }
}
*/
