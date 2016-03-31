using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Nodes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Example.Exodrifter
{
	/// <summary>
	/// This is an example of how a Rumor may be intialized using just code.
	/// </summary>
	public class RumorCodeExample : MonoBehaviour
	{
		private Rumor rumor;

		public Text text;

		void Awake()
		{
			rumor = new Rumor(new List<Node>() {
				new Label("start", new List<Node>() {
					new Say("Hi!"),
					new Say("How are you?"),
					new Say("I am doing fine."),
					new Say("Did you see the show last night?"),
					new Say("It was very good."),
					new Say("I shouldn't have watched it though..."),
					new Pause(3),
					new Say("I didn't finish my assignment!"),
					new Jump("start"),
				}),
			});

			StartCoroutine(rumor.Run());
		}

		void Update()
		{
			if (rumor == null) {
				text.text = "";
				return;
			}

			text.text = rumor.State.Dialog;

			rumor.Update(Time.deltaTime);

			if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) {
				rumor.Advance();
			}
		}
	}
}
