#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
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
			rumor = new Rumor(@"
label start:
	say ""Hi!""
	say ""Is this working?""

	choice ""Yes!"":
		say ""Great!""
	choice ""No."":
		say ""Darn...""
		pause 0.5
		add ""Maybe next time.""
		jump end
	choose

$ apples = get_apples()
$ pears = get_pears()

if apples == pears:
	$ pears += 1

say ""I have "" + apples + "" apples.""
say ""You have {pears} pears.""
say ""Who has more fruits?""

choice ""I do."":
	if apples < pears:
		jump correct
	jump incorrect
choice ""You do."":
	if apples > pears:
		jump correct
	jump incorrect
choose

label correct:
	say ""That's right!""
	jump end

label incorrect:
	say ""That's wrong!""
	jump end

label end:
	say ""Well, thanks for stopping by!""
	say ""See you next time!""
");
			rumor.Scope.DefaultSpeaker = "Narrator";

			rumor.Bindings.Bind("get_apples", () => { return Random.Range(2, 6); });
			rumor.Bindings.Bind("get_pears", () => { return Random.Range(2, 6); });

			StartCoroutine(rumor.Start());
		}

		void Update()
		{
			if (rumor == null) {
				text.text = "";
				return;
			}

			if (rumor.Choosing) {
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

#endif
