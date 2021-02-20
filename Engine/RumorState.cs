using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class RumorState : ISerializable
	{
		private const string DefaultSpeaker = "_narrator";

		private Dictionary<string, string> Dialog { get; }
		private Dictionary<string, string> Choices { get; }

		/// <summary>
		/// Creates a new, empty state with no dialog or choices.
		/// </summary>
		internal RumorState()
		{
			Dialog = new Dictionary<string, string>();
			Choices = new Dictionary<string, string>();
		}

		#region Choices

		/// <summary>
		/// An event you can subscribe to in order to get notified of new
		/// choices that are added to the state. The first parameter is the
		/// label of the choice and the second parameter is the text for the
		/// choice.
		/// </summary>
		public event Action<string, string> OnChoiceAdded;

		/// <summary>
		/// Adds a new choice to the state. If the choice already exists, then
		/// it will be overwritten with the new choice. <paramref name="text"/>
		/// will have leading and trailing whitespace removed.
		///
		/// If you don't want any input sanitization, use
		/// <see cref="AddRawChoice(string, string)"/> instead.
		/// </summary>
		/// <param name="label">
		/// The label for the choice to add, which will be used to identify this
		/// choice as well as determine which label to jump to if this choice
		/// is picked.
		/// </param>
		/// <param name="text">The text for the choice.</param>
		/// <exception cref="ArgumentNullException">
		/// Thrown if either <paramref name="label"/> or
		/// <paramref name="text"/> is null.
		/// </exception>
		public void AddChoice(string label, string text)
		{
			if (label == null)
			{
				throw new ArgumentNullException(nameof(label));
			}
			if (text == null)
			{
				throw new ArgumentNullException(nameof(text));
			}

			text = text.Trim();
			Choices.Add(label, text);
			OnChoiceAdded?.Invoke(label, text);
		}

		/// <summary>
		/// Adds a new choice to the state without any input sanitization. If
		/// the choice already exists, then it will be overwritten with the new
		/// choice.
		/// </summary>
		/// <param name="label">
		/// The label for the choice to add, which will be used to identify this
		/// choice as well as determine which label to jump to if this choice
		/// is picked.
		/// </param>
		/// <param name="text">The text for the choice.</param>
		/// <exception cref="ArgumentNullException">
		/// Thrown if either <paramref name="label"/> or
		/// <paramref name="text"/> is null.
		/// </exception>
		public void AddRawChoice(string label, string text)
		{
			if (label == null)
			{
				throw new ArgumentNullException(nameof(label));
			}
			if (text == null)
			{
				throw new ArgumentNullException(nameof(text));
			}

			Choices.Add(label, text);
			OnChoiceAdded?.Invoke(label, text);
		}

		/// <summary>
		/// Returns a copy of all the choices that are currently available.
		/// </summary>
		/// <returns>A copy of all the choices.</returns>
		public Dictionary<string, string> GetChoices()
		{
			return new Dictionary<string, string>(Choices);
		}

		#endregion

		#region Clear

		/// <summary>
		/// An event you can subscribe to in order to get notified when dialog
		/// and/or choices are removed from the state.
		/// </summary>
		public event Action<ClearType> OnClear;

		/// <summary>
		/// An event you can subscribe to in order to get notified when all of
		/// the dialog and choices are removed from the state.
		/// </summary>
		public event Action OnClearAll;

		/// <summary>
		/// An event you can subscribe to in order to get notified when all of
		/// the dialog is removed from the state.
		/// </summary>
		public event Action OnClearDialog;

		/// <summary>
		/// An event you can subscribe to in order to get notified when all of
		/// the choices are removed from the state.
		/// </summary>
		public event Action OnClearChoices;

		/// <summary>
		/// Removes all of the dialog and choices from the state.
		/// </summary>
		public void ClearAll()
		{
			Dialog.Clear();
			Choices.Clear();

			OnClear?.Invoke(ClearType.All);
			OnClearAll?.Invoke();
			OnClearDialog?.Invoke();
			OnClearChoices?.Invoke();
		}

		/// <summary>
		/// Removes all of the dialog from the state.
		/// </summary>
		public void ClearDialog()
		{
			Dialog.Clear();

			OnClear?.Invoke(ClearType.Dialog);
			OnClearDialog?.Invoke();
		}

		/// <summary>
		/// Removes all of the choices from the state.
		/// </summary>
		public void ClearChoices()
		{
			Choices.Clear();

			OnClear?.Invoke(ClearType.Choices);
			OnClearChoices?.Invoke();
		}

		/// <summary>
		/// Removes some subset of the state.
		/// </summary>
		/// <param name="type">The type of data to remove.</param>
		public void Clear(ClearType type)
		{
			switch (type)
			{
				case ClearType.All:
					ClearAll();
					break;

				case ClearType.Dialog:
					ClearDialog();
					break;

				case ClearType.Choices:
					ClearChoices();
					break;

				default:
					throw new InvalidOperationException(
						"Unknown clear type! Please file a bug report."
					);
			}
		}

		#endregion

		#region Dialog

		/// <summary>
		/// An event you can subscribe to in order to get notified when dialog
		/// is appended to the state
		/// </summary>
		public event Action<string, string> OnAppendDialog;

		/// <summary>
		/// An event you can subscribe to in order to get notified when dialog
		/// is set in the state
		/// </summary>
		public event Action<string, string> OnSetDialog;

		/// <summary>
		/// Appends additional dialog for the specified speaker to the state.
		/// <paramref name="dialog"/> will have leading and trailing whitespace
		/// removed and a space will be prepended to it if the existing dialog
		/// for the specified speaker doesn't end with a whitespace.
		///
		/// If you don't want any input sanitization, use
		/// <see cref="AppendRawDialog(string, string)"/> instead.
		/// </summary>
		/// <param name="speaker">The speaker to add dialog for.</param>
		/// <param name="dialog">The dialog to add.</param>
		/// <exception cref="ArgumentNullException">
		/// Thrown if either <paramref name="speaker"/> or
		/// <paramref name="dialog"/> is null.
		/// </exception>
		public void AppendDialog(string speaker, string dialog)
		{
			speaker = speaker ?? DefaultSpeaker;
			if (dialog == null)
			{
				throw new ArgumentNullException(nameof(dialog));
			}

			if (Dialog.ContainsKey(speaker) && Dialog[speaker].Length > 0)
			{
				var ch = Dialog[speaker][Dialog[speaker].Length - 1];
				if (char.IsWhiteSpace(ch))
				{
					Dialog[speaker] += dialog;
					OnAppendDialog?.Invoke(speaker, dialog);
				}
				else
				{
					Dialog[speaker] += " " + dialog;
					OnAppendDialog?.Invoke(speaker, " " + dialog);
				}
			}
			else
			{
				Dialog[speaker] = dialog;
				OnAppendDialog?.Invoke(speaker, dialog);
			}
		}

		/// <summary>
		/// Adds dialog to the state without any input sanitization.
		/// </summary>
		/// <param name="speaker">The speaker to add dialog for.</param>
		/// <param name="dialog">The dialog to add.</param>
		/// <exception cref="ArgumentNullException">
		/// Thrown if either <paramref name="speaker"/> or
		/// <paramref name="dialog"/> is null.
		/// </exception>
		public void AppendRawDialog(string speaker, string dialog)
		{
			speaker = speaker ?? DefaultSpeaker;
			if (dialog == null)
			{
				throw new ArgumentNullException(nameof(dialog));
			}

			if (Dialog.ContainsKey(speaker))
			{
				Dialog[speaker] += dialog;
				OnAppendDialog?.Invoke(speaker, dialog);
			}
			else
			{
				Dialog.Add(speaker, dialog);
				OnAppendDialog?.Invoke(speaker, dialog);
			}
		}

		/// <summary>
		/// Clears the state and sets the dialog for the specified speaker to
		/// the state. <paramref name="dialog"/> will have leading and trailing
		/// whitespace removed.
		///
		/// If you don't want any input sanitization, use
		/// <see cref="SetRawDialog(string, string)"/> instead.
		/// </summary>
		/// <param name="speaker">The speaker to add dialog for.</param>
		/// <param name="dialog">The dialog to add.</param>
		/// <exception cref="ArgumentNullException">
		/// Thrown if either <paramref name="speaker"/> or
		/// <paramref name="dialog"/> is null.
		/// </exception>
		public void SetDialog(string speaker, string dialog)
		{
			speaker = speaker ?? DefaultSpeaker;
			if (dialog == null)
			{
				throw new ArgumentNullException(nameof(dialog));
			}

			dialog = dialog.Trim();

			Dialog.Clear();
			Dialog[speaker] = dialog;
			OnSetDialog?.Invoke(speaker, dialog);
		}

		/// <summary>
		/// Clears the state and sets the dialog for the specified speaker to
		/// the state. <paramref name="dialog"/> will have leading and trailing
		/// whitespace removed.
		///
		/// If you don't want any input sanitization, use
		/// <see cref="SetRawDialog(string, string)"/> instead.
		/// </summary>
		/// <param name="speaker">The speaker to add dialog for.</param>
		/// <param name="dialog">The dialog to add.</param>
		/// <exception cref="ArgumentNullException">
		/// Thrown if either <paramref name="speaker"/> or
		/// <paramref name="dialog"/> is null.
		/// </exception>
		public void SetRawDialog(string speaker, string dialog)
		{
			speaker = speaker ?? DefaultSpeaker;
			if (dialog == null)
			{
				throw new ArgumentNullException(nameof(dialog));
			}

			Dialog.Clear();
			Dialog[speaker] = dialog;
			OnSetDialog?.Invoke(speaker, dialog);
		}

		/// <summary>
		/// Returns a copy of all the dialog that is currently available.
		/// </summary>
		/// <returns>A copy of all the dialog.</returns>
		public Dictionary<string, string> GetDialog()
		{
			return new Dictionary<string, string>(Dialog);
		}

		#endregion

		#region Serialization

		public RumorState(SerializationInfo info, StreamingContext context)
		{
			Dialog = info.GetValue<Dictionary<string, string>>("dialog");
			Choices = info.GetValue<Dictionary<string, string>>("choices");
		}

		public void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<Dictionary<string, string>>("dialog", Dialog);
			info.AddValue<Dictionary<string, string>>("choices", Choices);
		}

		#endregion
	}
}
