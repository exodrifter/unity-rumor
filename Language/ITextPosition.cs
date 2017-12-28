using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Exodrifter.Rumor.Language
{
	/// <summary>
	/// Describes a position in a plain-text document or string.
	/// </summary>
	public interface ITextPosition
	{
		/// <summary>
		/// The line number in the plain-text document.
		/// </summary>
		int Line { get; }

		/// <summary>
		/// The column number in the plain-text document.
		/// </summary>
		int Column { get; }

		/// <summary>
		/// The index in the plain-text document.
		/// </summary>
		int Index { get; }
	}
}
