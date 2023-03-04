using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Offensive
 * 
 *      Long Sword      Slash/Stab  
 *      Katana          Slash/Stab
 *      Spear           Stab        5
 *      Musket          Shoot       20
 *      FlintLock       Shoot       10
 *      Kukri           Throw       5
 */

/*
 * Defensive
 * 
 *      Shield          Shield 10
 *      Buckler         Shield 2
 *      Tazer           Stun
 */
public class Shield : Item
{
    public Shield()
    {
        cooldown_period = 3;
        ammo = 5;
    }
    public override void Use(CombatEntity self, CombatEntity target)
    {
        if (cooldown_timer > 0)
        {
            Debug.LogError("ERROR: Item used on cooldown," + cooldown_timer);
            return;
        }
        self.Shield(10);
        cooldown_timer = cooldown_period;
    }
}
public class Buckler : Item
{
    public Buckler()
    {
        cooldown_period = 2;
        ammo = 6;
    }
    public override void Use(CombatEntity self, CombatEntity target)
    {
        if (cooldown_timer > 0)
        {
            Debug.LogError("ERROR: Item used on cooldown," + cooldown_timer);
            return;
        }
        self.Shield(2);
        cooldown_timer = cooldown_period;
    }
}
public class Tazer : Item
{
    public Tazer()
    {
        cooldown_period = 5;
        ammo = 3;
    }
    public override void Use(CombatEntity self, CombatEntity target)
    {
        if (cooldown_timer > 0)
        {
            Debug.LogError("ERROR: Item used on cooldown," + cooldown_timer);
            return;
        }
        target.Stun();
        cooldown_timer = cooldown_period;
    }
}

/*
 * Magic
 * 
 *      Divinity
 *      
 *      BloodBoil
 */
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
 */
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
public class Cornucopia : Item
{
    public Cornucopia()
    {
        cooldown_period = 1;
        ammo = 10;
    }
     
    public override void Use(CombatEntity self, CombatEntity target)
    {
        self.Heal(3);
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