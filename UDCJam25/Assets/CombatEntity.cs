using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    // Item Characteristics
    public string item_name;
    public Sprite item_icon;

    // Combat Status
    public float cooldown_timer;
    public float cooldown_period;
    public float success_probability;
    public int ammo;

    public virtual void Use(CombatEntity self, CombatEntity target)
    {
        // override
        Debug.Log("Using Item " + item_name + " that does not override Use()!");
    }
    public void Update()
    {
        switch(ammo)
        {
            case 0:
                ammo++;
                cooldown_timer = 10;
                break;
            default:
                cooldown_timer--;
                break;
        }
    }
}

public class CombatEntity : MonoBehaviour
{
    // Entity Charcteristics
    [SerializeField] float base_health;
    [SerializeField] float strength;
    [SerializeField] float weight;
    [SerializeField] float wit;

    // Combat Status
    float health;
    bool stunned;
    float armor;

    // Equipment
    [SerializeField] int n_items;
    [SerializeField] Item[] items;

    void Awake()
    {
        health = base_health;
    }

    // Combat Modifications
    public void Damage(float damage)
    {
        if (armor > 0)
        {
            armor -= damage;
            return;
        }

        health -= damage;
        if (health <= 0)
        {
            // TODO: Check scene for end of combat situations
            Destroy(this.gameObject);
        }
    }
    public void Heal(float damage)
    {
        health = Mathf.Clamp(health + damage, 0, base_health);
    }
    public void Stun()
    {
        stunned = true;
    }
    public void Shield(float protection)
    {
        armor += protection;
    }

    // Control Functions
    public void UseItem(int item, CombatEntity target)
    {
        if (item >= n_items)
        {
            Debug.LogError("ERROR: item index, " + item.ToString() + ", >= number of items," + n_items.ToString());
            return;
        }
        items[item].Use(this, target);
    }
}
