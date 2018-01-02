#if UNITY_EDITOR

using Exodrifter.Rumor.Nodes;
using NUnit.Framework;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Test.Nodes
{
	/// <summary>
	/// Ensure Add nodes operate as expected.
	/// </summary>
	public class AddTest
	{
		/// <summary>
		/// Ensure Add nodes work if there is no existing dialog.
		/// </summary>
		[Test]
		public void AddNoneExisting()
		{
			var rumor = new Rumor.Engine.Rumor(new List<Node>());
			new Add("add").Run(rumor).MoveNext();
			Assert.AreEqual("add", rumor.State.Dialog[null]);
		}

		/// <summary>
		/// Ensure Add nodes work if there is existing dialog.
		/// </summary>
		[Test]
		public void AddExisting()
		{
			// Auto-add a space if dialog doesn't end with whitespace
			var rumor = new Rumor.Engine.Rumor(new List<Node>());
			new Say("thing").Run(rumor).MoveNext();
			new Add("add").Run(rumor).MoveNext();
			Assert.AreEqual("thing add", rumor.State.Dialog[null]);
			
			// Don't add a space if dialog ends with a space
			rumor = new Rumor.Engine.Rumor(new List<Node>());
			new Say("thing ").Run(rumor).MoveNext();
			new Add("add").Run(rumor).MoveNext();
			Assert.AreEqual("thing add", rumor.State.Dialog[null]);

			// Don't add a space if dialog ends with a tab
			rumor = new Rumor.Engine.Rumor(new List<Node>());
			new Say("thing\t").Run(rumor).MoveNext();
			new Add("add").Run(rumor).MoveNext();
			Assert.AreEqual("thing\tadd", rumor.State.Dialog[null]);

			// Don't add a space if dialog ends with a newline
			rumor = new Rumor.Engine.Rumor(new List<Node>());
			new Say("thing\n").Run(rumor).MoveNext();
			new Add("add").Run(rumor).MoveNext();
			Assert.AreEqual("thing\nadd", rumor.State.Dialog[null]);
		}

		/// <summary>
		/// Ensure empty Add nodes work properly with existing dialog.
		/// </summary>
		[Test]
		public void AddEmpty()
		{
			var rumor = new Rumor.Engine.Rumor(new List<Node>());

			new Say("thing").Run(rumor).MoveNext();
			new Add("").Run(rumor).MoveNext();

			// Adding nothing to a say line should do nothing
			Assert.AreEqual("thing", rumor.State.Dialog[null]);
		}
	}
}

#endif
