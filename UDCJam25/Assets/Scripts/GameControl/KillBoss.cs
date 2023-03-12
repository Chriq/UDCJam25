using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBoss : MonoBehaviour
{
    public void KillBossCharacter(string name) {
        switch(name) {
            case "Carnegie":
                Destroy(GameObject.Find("Andrew Carnegie"));
                GameData.Instance.carnegieKilled = true;
                break;
            case "Rockefeller":
				Destroy(GameObject.Find("Rockefeller"));
				GameData.Instance.rockefellerKilled = true;
				break;
            case "JP Morgan":
				Destroy(GameObject.Find("JP Morgan"));
				GameData.Instance.jpMorganKilled = true;
				break;
            default: break;
        }
    }
}
