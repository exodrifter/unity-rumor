using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Lang;
using Exodrifter.Rumor.Nodes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Example.Exodrifter
{
	/// <summary>
	/// This is an example of how a Rumor may be intialized using a script.
	/// </summary>
	[AddComponentMenu("")]
	public class RumorScriptExample : MonoBehaviour
	{
		private Rumor rumor;

		public Text text;

		void Awake()
		{
			rumor = new Rumor(new RumorCompiler().Compile(@"
label start:
	say ""Hi!""
	say ""Is this working?""

	choice ""Yes!"":
		say ""Great!""
	choice ""No."":
		say ""Darn...""
		pause 0.5
		add ""Maybe next time.""

say ""Well, thanks for stopping by!""
say ""See you next time!""
"));

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
				text.text = rumor.State.Dialog;
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
