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
	[AddComponentMenu("")]
	public class RumorCodeExample : MonoBehaviour
	{
		private Rumor rumor;

		public Text text;

		void Awake()
		{
			var n = "Narrator";
			rumor = new Rumor(new List<Node>() {
				new Label("start", new List<Node>() {
					new Say(n, "Hi!"),
					new Say(n, "Is this working?"),
					new Choice("Yes!", new List<Node>() {
						new Say(n, "Great!"),
					}),
					new Choice("No.", new List<Node>() {
						new Say(n, "Darn..."),
						new Pause(0.5f),
						new Add(n, "Maybe next time."),
					}),
				}),
				new Say("Well, thanks for stopping by!"),
				new Say("See you next time!"),
			});

			StartCoroutine(rumor.Run());
		}

		void Update()
		{
			if (rumor == null) {
				text.text = "";
				return;
			}

			if (rumor.State.Choices.Count > 0) {
				int num = 1;
				text.text = "";

				foreach (var choice in rumor.State.Choices) {
					text.text += num + ") " + choice + "\n";
					num++;
				}
			}
			else {
				text.text = rumor.State.Dialog["Narrator"];
			}

			rumor.Update(Time.deltaTime);

			if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) {
				rumor.Advance();
			}

			if (Input.GetKeyDown(KeyCode.Alpha1)) {
				rumor.Choose(0);
			}

			if (Input.GetKeyDown(KeyCode.Alpha2)) {
				rumor.Choose(1);
			}
		}
	}
}
