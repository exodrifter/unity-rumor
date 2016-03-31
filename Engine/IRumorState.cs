using System;
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
		/// Sets the dialog for the state.
		/// </summary>
		/// <param name="dialog">The dialog to set.</param>
		void SetDialog(string dialog);
	}

	/// <summary>
	/// A default rumor state that may be polled.
	/// </summary>
	[Serializable]
	public class DefaultRumorState : IRumorState
	{
		public string Dialog { get; private set; }

		public DefaultRumorState() { }

		public void SetDialog(string dialog)
		{
			Dialog = dialog;
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
