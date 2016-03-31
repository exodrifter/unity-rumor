using Exodrifter.Rumor.Nodes;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	/// <summary>
	/// Interface for storing state about the game.
	/// </summary>
	public interface IRumorState : ISerializable
	{
		/// <summary>
		/// Returns the current dialog.
		/// </summary>
		string Dialog { get; }

		/// <summary>
		/// Returns a list of choices.
		/// </summary>
		List<string> Choices { get; }

		/// <summary>
		/// Returns a list of nodes for each choice
		/// </summary>
		List<IEnumerable<Node>> Consequences { get; }

		/// <summary>
		/// Sets the dialog for the state.
		/// </summary>
		/// <param name="dialog">The dialog to set.</param>
		void SetDialog(string dialog);

		/// <summary>
		/// Adds to the dialog for the state.
		/// </summary>
		/// <param name="dialog">The dialog to add.</param>
		void AddDialog(string dialog);

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
		int AddChoice(string choice, IEnumerable<Node> nodes);

		void ClearChoices();

		/// <summary>
		/// Resets the state.
		/// </summary>
		void Reset();
	}

	/// <summary>
	/// A default rumor state that may be polled.
	/// </summary>
	[Serializable]
	public class DefaultRumorState : IRumorState
	{
		public string Dialog { get; private set; }

		public List<string> Choices { get; private set; }
		public List<IEnumerable<Node>> Consequences { get; private set; }

		public DefaultRumorState()
		{
			Reset();
		}

		public void SetDialog(string dialog)
		{
			Dialog = dialog ?? "";
		}

		public void AddDialog(string dialog)
		{
			if (!Dialog.EndsWith(" ")) {
				Dialog += " ";
			}
			Dialog += dialog;
		}

		public int AddChoice(string choice, IEnumerable<Node> nodes)
		{
			Choices.Add(choice);
			Consequences.Add(nodes);
			return Choices.Count - 1;
		}

		public void ClearChoices()
		{
			Choices.Clear();
			Consequences.Clear();
		}

		public void Reset()
		{
			Dialog = "";
			Choices = new List<string>();
			Consequences = new List<IEnumerable<Node>>();
		}

		#region Serialization

		public DefaultRumorState(SerializationInfo info, StreamingContext context)
		{
			Dialog = (string)info.GetValue("dialog", typeof(string));
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("dialog", Dialog, typeof(string));
		}

		#endregion
	}
}
