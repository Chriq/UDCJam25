using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool IsStunned()
    {
        return stunned;
    }
    public void Shield(float protection)
    {
        armor = protection;
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
