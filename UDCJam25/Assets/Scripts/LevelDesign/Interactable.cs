using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum InteractableEffects
{
    Load_Scene          = 1,
    Change_Elevation    = 2,
    Dialogue            = 3,
    Trigger_Objective   = 4,
}
enum InteractableEffectsValue
{
    Scene_SafeHouse     = 1,
    Scene_1             = 2,
    Scene_2             = 3,
    Scene_3             = 4,
    Scene_4             = 5,

	Elevation_Left      = 7,
	Elevation_Right     = 8,
	Elevation_Bottom    = 9,

    Carnegie_Obj        = 10,
    Rock_Obj            = 11,
    JP_Obj              = 12,
}

public class Interactable : MonoBehaviour
{
    [SerializeField] InteractableEffects int_effect;
    [SerializeField] InteractableEffectsValue int_effects_value;

    [SerializeField] string dialog_speaker;
	[SerializeField] string dialog_text;

	public void Interact()
    {
        if (int_effect == InteractableEffects.Load_Scene)
        {
            GameManager.Instance.ClearDialog();
			if(GameData.Instance.carnegieObjectiveComplete && GameData.Instance.rockefellerObjectiveComplete && GameData.Instance.jpMorganObjectiveComplete) {
				SceneManager.LoadScene("End");
			} else {
				SceneManager.LoadScene((int)int_effects_value);
			}
        } 
        else if((int_effect == InteractableEffects.Change_Elevation)) {
			GameManager.Instance.ChangeCharacterElevation((int)int_effects_value, gameObject.transform.position);
		}
        else if(int_effect == InteractableEffects.Dialogue) {
            GameManager.Instance.ToggleDialog(dialog_speaker, dialog_text);
		} else if(int_effect == InteractableEffects.Trigger_Objective) {
			switch((int) int_effects_value) {
                case 10:
                    GameData.Instance.carnegieObjectiveComplete = true;
                    Destroy(gameObject);
                    break;
				case 11:
					GameData.Instance.carnegieKilled = true;
					Destroy(gameObject);
					break;
				case 12:
					GameData.Instance.rockefellerObjectiveComplete = true;
					Destroy(gameObject);
					break;
				case 13:
					GameData.Instance.rockefellerKilled = true;
					Destroy(gameObject);
					break;
				case 14:
					GameData.Instance.jpMorganObjectiveComplete = true;
					Destroy(gameObject);
					break;
				case 15:
					GameData.Instance.jpMorganKilled = true;
					Destroy(gameObject);
					break;
			}
		}
	}
}
