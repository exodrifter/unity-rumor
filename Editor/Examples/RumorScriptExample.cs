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
		public TextAsset asset;

		void Awake()
		{
			rumor = new Rumor(asset.text);
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
