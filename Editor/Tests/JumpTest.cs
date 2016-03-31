using Exodrifter.Rumor.Nodes;
using Exodrifter.Rumor.Engine;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Test
{
	/// <summary>
	/// Makes sure that jump nodes operate as expected.
	/// </summary>
	public class JumpTest
	{
		/// <summary>
		/// Makes sure jumps to undefined labels throw an exception.
		/// </summary>
		[Test]
		public void JumpUndefined()
		{
			var rumor = new Engine.Rumor(new List<Node>() {
				new Jump("start"),
			});

			var yield = rumor.Run();
			Assert.Throws<InvalidOperationException>(() => yield.MoveNext());
		}

		/// <summary>
		/// Makes sure jumps to defined labels operate as expected.
		/// </summary>
		[Test]
		public void JumpDefined()
		{
			var rumor = new Engine.Rumor(new List<Node>() {
				new Jump("a"),
				new Label("b", null),
				new Say("b"),
				new Label("a", null),
				new Say("a"),
				new Jump("b"),
			});

			var yield = rumor.Run();
			yield.MoveNext();
			Assert.AreEqual("a", (rumor.Current as Say).text);
			
			rumor.Advance();
			yield.MoveNext();
			Assert.AreEqual("b", (rumor.Current as Say).text);
		}

		/// <summary>
		/// Makes sure jumps go to the first defined label when the same
		/// label is defined multiple times in the same scope.
		/// </summary>
		[Test]
		public void JumpMultipleDefinedSameScope()
		{
			var rumor = new Engine.Rumor(new List<Node>() {
				new Jump ("start"),
				new Label("a", new List<Node>() {
					new Say("aa"),
				}),
				new Label("a", new List<Node>() {
					new Say("ab"),
				}),
				new Label("a", new List<Node>() {
					new Say("ac"),
				}),
				new Label("start", null),
				new Jump("a"),
			});

			var yield = rumor.Run();
			yield.MoveNext();
			Assert.AreEqual("aa", (rumor.Current as Say).text);
		}

		/// <summary>
		/// Makes sure jumps go to the closest defined label when the same
		/// label is defined multiple times in different scopes.
		/// </summary>
		[Test]
		public void JumpMultipleDefinedDifferentScope()
		{
			var rumor = new Engine.Rumor(new List<Node>() {
				new Label("a", new List<Node>() {
					new Say("a"),

					new Label("a", new List<Node>() {
						new Say("aa"),
						new Jump("b"),
					}),
					new Label("b", new List<Node>() {
						new Say("ab"),
						new Jump("a"),
					}),
				}),
			});

			var yield = rumor.Run();
			yield.MoveNext();
			Assert.AreEqual("a", (rumor.Current as Say).text);

			rumor.Advance();
			yield.MoveNext();
			Assert.AreEqual("aa", (rumor.Current as Say).text);

			rumor.Advance();
			yield.MoveNext();
			Assert.AreEqual("ab", (rumor.Current as Say).text);

			rumor.Advance();
			yield.MoveNext();
			Assert.AreEqual("aa", (rumor.Current as Say).text);
		}
	}
}
