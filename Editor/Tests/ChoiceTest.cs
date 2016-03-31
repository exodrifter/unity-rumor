using Exodrifter.Rumor.Nodes;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Test
{
	/// <summary>
	/// Makes sure that choice nodes operate as expected.
	/// </summary>
	public class ChoiceTest
	{
		/// <summary>
		/// Ensure adding choices works.
		/// </summary>
		[Test]
		public void AddChoice()
		{
			var rumor = new Engine.Rumor(new List<Node>());
			new Choice("1", new List<Node>()).Run(rumor).MoveNext();
			new Choice("2", new List<Node>()).Run(rumor).MoveNext();
			new Choice("3", new List<Node>()).Run(rumor).MoveNext();

			Assert.AreEqual(3, rumor.State.Choices.Count);
		}

		/// <summary>
		/// Ensure choices auto add each other until there are no more choices.
		/// </summary>
		[Test]
		public void AutoAddChoice()
		{
			var rumor = new Engine.Rumor(new List<Node>() {
				new Choice("1", new List<Node>()),
				new Choice("2", new List<Node>()),
				new Choice("3", new List<Node>()),
				new Say("say"),
				new Choice("4", new List<Node>()),
			});

			var yield = rumor.Run();
			yield.MoveNext();
			Assert.AreEqual(3, rumor.State.Choices.Count);
		}
	}
}
