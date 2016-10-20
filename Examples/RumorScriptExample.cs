using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Lang;
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
$ n = ""Narrator""
label start:
	say n ""Hi!""
	say n ""Is this working?""

	choice ""Yes!"":
		say n ""Great!""
	choice ""No."":
		say n ""Darn...""
		pause 0.5
		add n ""Maybe next time.""

say n ""Well, thanks for stopping by!""
say n ""See you next time!""
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
