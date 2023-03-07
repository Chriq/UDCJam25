using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum InteractableEffects
{
    Load_Scene          = 1,
}
enum InteractableEffectsValue
{
    Scene_SafeHouse     = 0,
    Scene_1             = 1,
    Scene_2             = 2,
    Scene_3             = 3,
    Scene_4             = 4,
    Scene_5             = 5,
    Scene_6             = 6,
}

public class Interactable : MonoBehaviour
{
    [SerializeField] InteractableEffects int_effect;
    [SerializeField] InteractableEffectsValue int_effects_value;

    public void Interact()
    {
        if (int_effect == InteractableEffects.Load_Scene)
        {
            SceneManager.LoadScene((int)int_effects_value);
        }
    }
}
