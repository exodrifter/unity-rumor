#if UNITY_EDITOR

using Exodrifter.Rumor.Nodes;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Test.Nodes
{
	/// <summary>
	/// Ensure Call nodes operate as expected.
	/// </summary>
	public class CallTest
	{
		/// <summary>
		/// Ensure calls to undefined labels throw an exception.
		/// </summary>
		[Test]
		public void CallUndefined()
		{
			var rumor = new Rumor.Engine.Rumor(new List<Node>() {
				new Call("start"),
			});

			var yield = rumor.Start();
			Assert.Throws<InvalidOperationException>(() => yield.MoveNext());
		}

		/// <summary>
		/// Ensure calls to defined labels operate as expected.
		/// </summary>
		[Test]
		public void CallDefined()
		{
			var rumor = new Rumor.Engine.Rumor(new List<Node>() {
				new Call("a"),
				new Label("b", new List<Node>() {
					new Say("b"),
					new Return(),
				}),
				new Label("a", new List<Node>() {
					new Say("a"),
				}),
				new Call("b"),
			});

			var yield = rumor.Start();
			yield.MoveNext();
			Assert.AreEqual("a", (rumor.Current as Say).EvaluateText(rumor));

			rumor.Advance();
			yield.MoveNext();
			Assert.AreEqual("b", (rumor.Current as Say).EvaluateText(rumor));

			rumor.Advance();
			yield.MoveNext();
			Assert.AreEqual("a", (rumor.Current as Say).EvaluateText(rumor));

			rumor.Advance();
			yield.MoveNext();
			Assert.AreEqual("b", (rumor.Current as Say).EvaluateText(rumor));

			rumor.Advance();
			yield.MoveNext();
			Assert.True(rumor.Started);
			Assert.True(rumor.Finished);
		}

		/// <summary>
		/// Ensure calls go to the first defined label when the same label is
		/// defined multiple times in the same scope.
		/// </summary>
		[Test]
		public void CallMultipleDefinedSameScope()
		{
			var rumor = new Rumor.Engine.Rumor(new List<Node>() {
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
				new Call("a"),
			});

			var yield = rumor.Start();
			yield.MoveNext();
			Assert.AreEqual("aa", (rumor.Current as Say).EvaluateText(rumor));
		}

		/// <summary>
		/// Ensure calls go to the closest defined label when the same label
		/// is defined multiple times in different scopes.
		/// </summary>
		[Test]
		public void CallMultipleDefinedDifferentScope()
		{
			var rumor = new Rumor.Engine.Rumor(new List<Node>() {
				new Label("a", new List<Node>() {
					new Say("a"),

					new Label("a", new List<Node>() {
						new Say("aa"),
					}),
					new Label("b", new List<Node>() {
						new Say("ab"),
						new Call("a"),
					}),
				}),
			});

			var yield = rumor.Start();
			yield.MoveNext();
			Assert.AreEqual("a", (rumor.Current as Say).EvaluateText(rumor));

			rumor.Advance();
			yield.MoveNext();
			Assert.AreEqual("aa", (rumor.Current as Say).EvaluateText(rumor));

			rumor.Advance();
			yield.MoveNext();
			Assert.AreEqual("ab", (rumor.Current as Say).EvaluateText(rumor));

			rumor.Advance();
			yield.MoveNext();
			Assert.AreEqual("aa", (rumor.Current as Say).EvaluateText(rumor));
		}
	}
}

#endif
