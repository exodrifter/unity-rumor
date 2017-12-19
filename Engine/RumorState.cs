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
		/// <summary>
		/// Returns the current dialog.
		/// </summary>
		public LazyDictionary<object, string> Dialog { get; private set; }

		/// <summary>
		/// Returns a list of choices.
		/// </summary>
		public List<string> Choices { get; private set; }

		/// <summary>
		/// Returns a list of nodes for each choice
		/// </summary>
		public List<List<Node>> Consequences { get; private set; }

		/// <summary>
		/// An event for when dialog is added to the state.
		/// </summary>
		public event Action<object, string> OnAddDialog;

		/// <summary>
		/// An event for when dialog is set in the state.
		/// </summary>
		public event Action<object, string> OnSetDialog;

		/// <summary>
		/// An event for when a choice is added to the state.
		/// </summary>
		public event Action<int, string> OnAddChoice;
		
		/// <summary>
		/// An event for when a choice is removed from the state.
		/// </summary>
		public event Action<int, string> OnRemoveChoice;

		/// <summary>
		/// An event for when the state is cleared.
		/// </summary>
		public event Action<ClearType> OnClear;

		/// <summary>
		/// Creates a new Rumor state.
		/// </summary>
		public RumorState()
		{
			Clear();
		}

		/// <summary>
		/// Sets the dialog for the state.
		/// </summary>
		/// <param name="speaker">The speaker of the dialog.</param>
		/// <param name="dialog">The dialog to set.</param>
		public void SetDialog(object speaker, string dialog)
		{
			Dialog.Clear();
			Dialog[speaker] = dialog;

			if (OnSetDialog != null) {
				OnSetDialog(speaker, dialog);
			}
		}

		/// <summary>
		/// Adds to the dialog for the state.
		/// </summary>
		/// <param name="speaker">The speaker of the dialog.</param>
		/// <param name="dialog">The dialog to add.</param>
		public void AddDialog(object speaker, string dialog)
		{
			Dialog[speaker] += dialog;

			if (OnAddDialog != null) {
				OnAddDialog(speaker, dialog);
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
			if (nodes == null)
			{
				Consequences.Add(new List<Node>());
			}
			else
			{
				Consequences.Add(new List<Node>(nodes));
			}

			var index = Choices.Count - 1;
			if (OnAddChoice != null) {
				OnAddChoice(index, choice);
			}
			return index;
		}

		/// <summary>
		/// Removes the choice at the specified index.
		/// </summary>
		/// <param name="index">The index of the choice to remove.</param>
		public void RemoveChoice(int index)
		{
			var choice = Choices[index];
			Choices.RemoveAt(index);
			Consequences.RemoveAt(index);

			if (OnRemoveChoice != null) {
				OnRemoveChoice(index, choice);
			}
		}

		/// <summary>
		/// Clears the state.
		/// </summary>
		public void Clear()
		{
			Dialog = new LazyDictionary<object, string>();
			Choices = new List<string>();
			Consequences = new List<List<Node>>();

			if (OnClear != null) {
				OnClear(ClearType.ALL);
			}
		}

		/// <summary>
		/// Clears the choices.
		/// </summary>
		public void ClearChoices()
		{
			Choices = new List<string>();
			Consequences = new List<List<Node>>();

			if (OnClear != null) {
				OnClear(ClearType.CHOICES);
			}
		}

		/// <summary>
		/// Clears the dialog.
		/// </summary>
		public void ClearDialog()
		{
			Dialog = new LazyDictionary<object, string>();

			if (OnClear != null) {
				OnClear(ClearType.DIALOG);
			}
		}

		#region Serialization

		public RumorState(SerializationInfo info, StreamingContext context)
		{
			Dialog = info.GetValue<LazyDictionary<object,string>>("dialog");
			Choices = info.GetValue<List<string>>("choices");
			Consequences = info.GetValue<List<List<Node>>>("consequences");
		}

		public void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<LazyDictionary<object,string>>("dialog", Dialog);
			info.AddValue<List<string>>("choices", Choices);
			info.AddValue<List<List<Node>>>("consequences", Consequences);
		}

		#endregion
	}
}
