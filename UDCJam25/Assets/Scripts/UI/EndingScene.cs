using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingScene : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI content;

	private void Update() {
		if(GameData.Instance.carnegieKilled && GameData.Instance.rockefellerKilled && GameData.Instance.jpMorganKilled) {
			title.text = "Communist Revolution Ending";
			content.text = "The utter destruction and execution of the anti-magic bourgeoisie has inspired the magic-wielding proletariet to rise up and seize the mechanical means of production. Magic users once again reign supreme, though conflict now flourishes throughout the land and tension runs high on both sides. Perhaps you may someday lead everyone to a brighter, more equal future.";
		}

		else if(!GameData.Instance.carnegieKilled && GameData.Instance.rockefellerKilled && GameData.Instance.jpMorganKilled) {
			title.text = "Social Reform Ending";
			content.text = "The corrupt have been washed away, but you have the compassion to spare the misguided leadership, inspiring new social reform. Andrew Carnegie, Nikola Tesla, and other generally good-natured captains of industry cooperate with those they employ to provide for better working conditions, better pay, and most importantly, better education. This movement stems the flow of hate against magic users, and over time, the magical and non-magical work together to build a brighter future";
		}
		else if(GameData.Instance.carnegieKilled && !GameData.Instance.rockefellerKilled && !GameData.Instance.jpMorganKilled) {
			title.text = "Fascist Backlash Ending";
			content.text = "Your intentions were good all along, but unfortunately your judgement led you to eliminate the wrong targets. In the wake of the destruction of their industrial centers, the captains of industry unleash a reign of terror against the working magic population, laying down the law in a strict and harsh matter. These are dark times you have brought upon the world, let's just hope someday you can mend your mistakes.";
		}
		else {
			title.text = "Status Quo Ending";
			content.text = "While news of your action swept the world, they ultimately were in vain. Very little tangible changes were made in the wake of your revolution, and everything seems to be as it was. What else must be done in order to achieve equality of magic users across the land?";
		}
	}

	public void Quit() {
		Application.Quit();
	}
}
