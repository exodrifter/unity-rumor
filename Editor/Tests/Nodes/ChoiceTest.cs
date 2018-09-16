#if UNITY_EDITOR

using Exodrifter.Rumor.Nodes;
using NUnit.Framework;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Test.Nodes
{
	/// <summary>
	/// Ensure Choice nodes operate as expected.
	/// </summary>
	public class ChoiceTest
	{
		/// <summary>
		/// Ensure adding choices works.
		/// </summary>
		[Test]
		public void AddChoice()
		{
			var rumor = new Rumor.Engine.Rumor(new List<Node>());
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
			var rumor = new Rumor.Engine.Rumor(new List<Node>() {
				new Choice("1", new List<Node>()),
				new Choice("2", new List<Node>()),
				new Choice("3", new List<Node>()),
				new Say("say"),
				new Choice("4", new List<Node>()),
			});

			var yield = rumor.Start();
			yield.MoveNext();
			Assert.AreEqual(3, rumor.State.Choices.Count);
		}

		/// <summary>
		/// Ensure choices jump immediately without waiting for the current
		/// pause to complete.
		/// </summary>
		[Test]
		public void ChoiceTerminatesPause()
		{
			var rumor = new Rumor.Engine.Rumor(new List<Node>() {
				new Choice("1", new List<Node>() { new Return() } ),
				new Pause(60, false),
			});

			var yield = rumor.Start();
			yield.MoveNext(); // Start the pause
			rumor.Choose(0);
			yield.MoveNext(); // Update the state
			Assert.AreEqual(true, rumor.Finished);
		}

		/// <summary>
		/// Ensure choices cannot skip an unskippable pause.
		/// </summary>
		[Test]
		public void ChoiceCannotSkipUnskippablePause()
		{
			var rumor = new Rumor.Engine.Rumor(new List<Node>() {
				new Choice("1", new List<Node>() { new Return() } ),
				new Pause(60, true),
			});

			var yield = rumor.Start();
			yield.MoveNext(); // Start the pause
			rumor.Choose(0);
			yield.MoveNext(); // Update the state
			Assert.AreEqual(false, rumor.Finished);
		}
	}
}

#endif
