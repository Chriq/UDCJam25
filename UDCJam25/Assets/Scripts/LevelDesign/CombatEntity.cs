using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatEntity : MonoBehaviour
{
    // Entity Charcteristics
    [SerializeField] float base_health;
    [SerializeField] float strength;
    [SerializeField] float weight;
    [SerializeField] float wit;

    // Combat Status
    public float health;
    bool stunned;
    float armor;

    // Equipment
    [SerializeField] List<Item> items;

    public OnKilledEvent onKilledEvent;

    void Awake()
    {
        health = base_health;
        if(onKilledEvent == null) {
            onKilledEvent = new OnKilledEvent();
        }
    }

    // AI
    public float CombatPotential(List<GameObject> enemies)
    {
        float potential = 0;
        int dist_closest_enemy = 100;
        
        // Calculate closest enemy
        foreach (GameObject enemy in enemies)
        {
            int dist = Mathf.RoundToInt(
                Mathf.Abs(enemy.transform.position.x - this.gameObject.transform.position.x) +
                Mathf.Abs(enemy.transform.position.y - this.gameObject.transform.position.y)
                );

            if (dist_closest_enemy > dist)
                dist_closest_enemy = dist;
        }

        // Find arbitrary potential of all available items
        //      Assumes offensive weapon use
        foreach (Item item in items)
        {
            if (item.cooldown_timer > 0)
            {
                continue;
            }
            float pot = Mathf.Max(item.value_target, item.value_self) / item.use_cost;
            if (item.use_range <= dist_closest_enemy)
                pot *= 2;

            potential += pot;
        }

        // Add potential of moving closer
        potential += dist_closest_enemy;

        return potential;
    }
    public int AutoPlay(List<GameObject> enemies, int max_actions_available)
    {
        Debug.Log("AUTOPLAY");
        int actions_used = 0;
        int dist_closest_enemy = 100;
        GameObject closest_enemy = enemies[0];

        // Calculate closest enemy
        foreach (GameObject enemy in enemies)
        {
            int dist = Mathf.RoundToInt(
                Mathf.Abs(enemy.transform.position.x - this.gameObject.transform.position.x) +
                Mathf.Abs(enemy.transform.position.y - this.gameObject.transform.position.y)
                );

            if (dist_closest_enemy > dist)
            {
                closest_enemy = enemy;
                dist_closest_enemy = dist;
            }
        }
        Debug.Log(closest_enemy);

        // Scan Items for availability
        List<Item> available_items = new List<Item>();
        foreach (Item item in items)
        {
            Debug.LogFormat("Actions {0} - CD {1} - Use Cost {2} - Use Range {3} - Dist {4}",
                max_actions_available,
                item.cooldown_timer,
                item.use_cost,
                item.use_range,
                dist_closest_enemy
                );
            if (item.cooldown_timer == 0                                                            // Off Cooldown
                && item.use_cost <= max_actions_available                                           // Within actions remaining
                && item.use_range <= dist_closest_enemy + max_actions_available - item.use_cost     // Within Range
                )
            {
                Debug.Log("Append");
                available_items.Add(item);
            }
        }
        Debug.Log(available_items.Count);

        if (available_items.Count > 0)
        {
            Item item = available_items[0];
            Debug.Log(item);

            if (item.use_range > dist_closest_enemy)
            {
                //TODO : Move To Enemy
                //actions_used += ;
            }

            item.Use(this.gameObject.GetComponent<CombatEntity>(), closest_enemy.GetComponent<CombatEntity>());
            actions_used += item.use_cost;
            return actions_used;
        }

        //TODO : Move Closer if necessary

        return actions_used;
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
            onKilledEvent.Invoke(this.gameObject);
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
    public int UseItem(int item, CombatEntity target)
    {
        if (item >= items.Count)
        {
            Debug.LogError("ERROR: item index, " + item.ToString() + ", >= number of items," + items.Count.ToString());
            return 0;
        }
        return items[item].Use(this, target);
    }
}

public class OnKilledEvent: UnityEvent<GameObject> {}