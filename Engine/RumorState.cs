using Exodrifter.Rumor.Nodes;
using Exodrifter.Rumor.Util;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	/// <summary>
	/// Interface for storing state about the game.
	/// </summary>
	[Serializable]
	public class RumorState : ISerializable
	{
		public const string NARRATOR = "_narrator";

		/// <summary>
		/// Returns the current dialog.
		/// </summary>
		public Dictionary<object, string> Dialog { get; private set; }

		/// <summary>
		/// Returns a list of choices.
		/// </summary>
		public List<string> Choices { get; private set; }

		/// <summary>
		/// Returns a list of nodes for each choice
		/// </summary>
		public List<List<Node>> Consequences { get; private set; }

		/// <summary>
		/// Creates a new Rumor state.
		/// </summary>
		public RumorState()
		{
			Reset();
		}

		/// <summary>
		/// Resets the state.
		/// </summary>
		public void Reset()
		{
			Dialog = new Dictionary<object, string>();
			Choices = new List<string>();
			Consequences = new List<List<Node>>();
		}

		/// <summary>
		/// Sets the dialog for the state. If the speaker is null, then the
		/// <see cref="RumorState.NARRATOR"/> is used instead.
		/// </summary>
		/// <param name="speaker">The speaker of the dialog.</param>
		/// <param name="dialog">The dialog to set.</param>
		public void SetDialog(object speaker, string dialog)
		{
			Dialog.Clear();
			Dialog[speaker ?? NARRATOR] = dialog;
		}

		/// <summary>
		/// Adds to the dialog for the state.
		/// </summary>
		/// <param name="dialog">The dialog to add.</param>
		public void AddDialog(object character, string dialog)
		{
			if (Dialog.ContainsKey(character)) {
				Dialog[character ?? NARRATOR] += dialog;
			}
			else {
				Dialog[character ?? NARRATOR] = dialog;
			}
		}

		/// <summary>
		/// Adds a choice for the state.
		/// </summary>
		/// <param name="choice">
		/// The text for the choice.
		/// </param>
		/// <param name="nodes">
		/// The nodes to use if the choice is selected.
		/// </param>
		/// <returns>
		/// The index of the choice.
		/// </returns>
		public int AddChoice(string choice, IEnumerable<Node> nodes)
		{
			Choices.Add(choice);
			Consequences.Add(new List<Node>(nodes));
			return Choices.Count - 1;
		}

		/// <summary>
		/// Removes all current choices.
		/// </summary>
		public void ClearChoices()
		{
			Choices.Clear();
			Consequences.Clear();
		}

		#region Serialization

		public RumorState(SerializationInfo info, StreamingContext context)
		{
			Dialog = info.GetValue<Dictionary<object,string>>("dialog");
			Choices = info.GetValue<List<string>>("choices");
			Consequences = info.GetValue<List<List<Node>>>("consequences");
		}

		public void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<Dictionary<object,string>>("dialog", Dialog);
			info.AddValue<List<string>>("choices", Choices);
			info.AddValue<List<List<Node>>>("consequences", Consequences);
		}

		#endregion
	}
}
