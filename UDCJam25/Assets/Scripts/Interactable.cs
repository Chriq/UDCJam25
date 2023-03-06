using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum InteractableEffects
{
    Load_Scene = 1 << 0,
}
enum InteractableEffectsValue
{
    Scene_SafeHouse     = 0,
    Scene_1     = 2,
    Scene_2     = 3,
    Scene_3     = 4,
    Scene_4     = 5,
    Scene_5     = 6,
    Scene_6     = 7,
}

public class Interactable : MonoBehaviour
{
    [SerializeField] [EnumMask(typeof(InteractableEffects))] int int_effects;
    [SerializeField] [EnumMask(typeof(InteractableEffectsValue))] int int_effects_value;

    public void Interact()
    {
        if ((int_effects & (int)InteractableEffects.Load_Scene) != 0)
        {
            SceneManager.LoadScene(int_effects_value);
        }
    }
}
