using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Effects Enumerator
enum ItemEffects
{
    Damage_Target = 1 << 0,
    Damage_Self = 1 << 1,
    Heal_Target = 1 << 2,
    Heal_Self = 1 << 3,
    Stun_Target = 1 << 4,
    Stun_Self = 1 << 5,
    Shield_Target = 1 << 6,
    Shield_Self = 1 << 7,
}

/* https://pavcreations.com/equipment-system-for-an-rpg-unity-game/#The-concepts-of-the-equipment-system */
[CreateAssetMenu(menuName = "ScriptableObjects/GameItem")]
public class ItemScriptable : ScriptableObject
{
    // Display
    public string item_name;
    public Sprite item_icon;

    // Combat Effect
    [EnumMask(typeof(ItemEffects))] public int item_effects;
    public float value_target;
    public float value_self;

    // Usability
    public int cooldown_period;
    public int reload_period;
    public int ammunition;
    public int use_range;
    public int use_cost;

    public float success_probability;
}
