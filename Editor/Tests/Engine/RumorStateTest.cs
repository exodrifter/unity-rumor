#if UNITY_EDITOR

using Exodrifter.Rumor.Engine;
using NUnit.Framework;

namespace Exodrifter.Rumor.Test.Engine
{
	/// <summary>
	/// Ensure RumorState operates as expected.
	/// </summary>
	public class RumorStateTest
	{
		/// <summary>
		/// Ensure an empty RumorState doesn't contain null references.
		/// </summary>
		[Test]
		public void EmptyState()
		{
			var state = new RumorState();
			Assert.AreEqual(0, state.Dialog.Keys.Count);
			Assert.AreEqual(0, state.Dialog.Values.Count);
			Assert.AreEqual(0, state.Choices.Count);
			Assert.AreEqual(0, state.Consequences.Count);
		}

		/// <summary>
		/// Ensure the SetDialog method operates as expected.
		/// </summary>
		[Test]
		public void SetDialog()
		{
			var state = new RumorState();

			var wrong = false;
			var count = 0;
			state.OnSetDialog += (obj, str) => count++;
			state.OnAddDialog += (obj, str) => wrong = true;
			state.OnAddChoice += (index, str) => wrong = true;
			state.OnRemoveChoice += (index, str) => wrong = true;
			state.OnClear += (type) => wrong = true;

			// First use
			state.SetDialog("Narrator", "Hello World.");
			Assert.AreEqual(1, state.Dialog.Keys.Count);
			Assert.AreEqual(1, state.Dialog.Values.Count);
			Assert.True(state.Dialog["Narrator"] == "Hello World.");

			// Second use
			state.SetDialog("Someone", "Goodbye World.");
			Assert.AreEqual(1, state.Dialog.Keys.Count);
			Assert.AreEqual(1, state.Dialog.Values.Count);
			Assert.True(state.Dialog["Someone"] == "Goodbye World.");

			// Check callback
			Assert.AreEqual(2, count);
			Assert.IsFalse(wrong);
		}

		/// <summary>
		/// Ensure the AddDialog method operates as expected.
		/// </summary>
		[Test]
		public void AddDialog()
		{
			var state = new RumorState();

			var wrong = false;
			var count = 0;
			state.OnSetDialog += (obj, str) => wrong = true;
			state.OnAddDialog += (obj, str) => count++;
			state.OnAddChoice += (index, str) => wrong = true;
			state.OnRemoveChoice += (index, str) => wrong = true;
			state.OnClear += (type) => wrong = true;

			// First use
			state.AddDialog("Narrator", "Hello World.");
			Assert.AreEqual(1, state.Dialog.Keys.Count);
			Assert.AreEqual(1, state.Dialog.Values.Count);
			Assert.True(state.Dialog["Narrator"] == "Hello World.");

			// Second use
			state.AddDialog("Someone", "Goodbye World.");
			Assert.AreEqual(2, state.Dialog.Keys.Count);
			Assert.AreEqual(2, state.Dialog.Values.Count);
			Assert.True(state.Dialog["Narrator"] == "Hello World.");
			Assert.True(state.Dialog["Someone"] == "Goodbye World.");

			// Check callback
			Assert.AreEqual(2, count);
			Assert.IsFalse(wrong);
		}

		/// <summary>
		/// Ensure the AddChoice method operates as expected.
		/// </summary>
		[Test]
		public void AddChoice()
		{
			var state = new RumorState();

			var wrong = false;
			var count = 0;
			state.OnSetDialog += (obj, str) => wrong = true;
			state.OnAddDialog += (obj, str) => wrong = true;
			state.OnAddChoice += (index, str) => count++;
			state.OnRemoveChoice += (index, str) => wrong = true;
			state.OnClear += (type) => wrong = true;

			// First use
			state.AddChoice("a", null);
			Assert.AreEqual(1, state.Choices.Count);
			Assert.True(state.Choices[0] == "a");
			Assert.AreEqual(1, state.Consequences.Count);
			Assert.True(state.Consequences[0].Count == 0);

			// Second use
			state.AddChoice("b", null);
			Assert.AreEqual(2, state.Choices.Count);
			Assert.True(state.Choices[0] == "a");
			Assert.True(state.Choices[1] == "b");
			Assert.AreEqual(2, state.Consequences.Count);
			Assert.True(state.Consequences[0].Count == 0);
			Assert.True(state.Consequences[1].Count == 0);

			// Check callback
			Assert.AreEqual(2, count);
			Assert.IsFalse(wrong);
		}

		/// <summary>
		/// Ensure the AddChoice method operates as expected.
		/// </summary>
		[Test]
		public void RemoveChoice()
		{
			var state = new RumorState();

			var wrong = false;
			var addCount = 0;
			var removeCount = 0;
			state.OnSetDialog += (obj, str) => wrong = true;
			state.OnAddDialog += (obj, str) => wrong = true;
			state.OnAddChoice += (index, str) => addCount++;
			state.OnRemoveChoice += (index, str) => removeCount++;
			state.OnClear += (type) => wrong = true;

			// Add choices
			state.AddChoice("a", null);
			state.AddChoice("b", null);
			state.AddChoice("c", null);
			Assert.AreEqual(3, state.Choices.Count);
			Assert.True(state.Choices[0] == "a");
			Assert.True(state.Choices[1] == "b");
			Assert.True(state.Choices[2] == "c");
			Assert.AreEqual(3, state.Consequences.Count);
			Assert.True(state.Consequences[0].Count == 0);
			Assert.True(state.Consequences[1].Count == 0);
			Assert.True(state.Consequences[2].Count == 0);

			state.RemoveChoice(1);
			Assert.AreEqual(2, state.Choices.Count);
			Assert.True(state.Choices[0] == "a");
			Assert.True(state.Choices[1] == "c");
			Assert.AreEqual(2, state.Consequences.Count);
			Assert.True(state.Consequences[0].Count == 0);
			Assert.True(state.Consequences[1].Count == 0);

			state.RemoveChoice(0);
			Assert.AreEqual(1, state.Choices.Count);
			Assert.True(state.Choices[0] == "c");
			Assert.AreEqual(1, state.Consequences.Count);
			Assert.True(state.Consequences[0].Count == 0);

			state.RemoveChoice(0);
			Assert.AreEqual(0, state.Choices.Count);
			Assert.AreEqual(0, state.Consequences.Count);

			// Check callback
			Assert.AreEqual(3, addCount);
			Assert.AreEqual(3, removeCount);
			Assert.IsFalse(wrong);
		}

		[Test]
		public void ClearDialog()
		{
			var state = new RumorState();

			var wrong = false;
			ClearType? type = null;
			state.OnAddChoice += (index, str) => wrong = true;
			state.OnRemoveChoice += (index, str) => wrong = true;
			state.OnClear += (t) => type = t;

			// Test Clear All
			state.AddDialog("Narrator", "Hello World.");
			state.AddDialog("Someone", "Goodbye World.");
			state.Clear();
			Assert.AreEqual(0, state.Dialog.Keys.Count);
			Assert.AreEqual(0, state.Dialog.Values.Count);
			Assert.AreEqual(ClearType.ALL, type);

			// Test Clear Dialog
			state.AddDialog("Narrator", "Hello World.");
			state.AddDialog("Someone", "Goodbye World.");
			state.ClearDialog();
			Assert.AreEqual(0, state.Dialog.Keys.Count);
			Assert.AreEqual(0, state.Dialog.Values.Count);
			Assert.AreEqual(ClearType.DIALOG, type);

			// Test Clear Choices
			state.AddDialog("Narrator", "Hello World.");
			state.AddDialog("Someone", "Goodbye World.");
			state.ClearChoices();
			Assert.AreEqual(2, state.Dialog.Keys.Count);
			Assert.AreEqual(2, state.Dialog.Values.Count);
			Assert.True(state.Dialog["Narrator"] == "Hello World.");
			Assert.True(state.Dialog["Someone"] == "Goodbye World.");
			Assert.AreEqual(ClearType.CHOICES, type);

			// Check callback
			Assert.IsFalse(wrong);
		}

		[Test]
		public void ClearChoices()
		{
			var state = new RumorState();

			var wrong = false;
			var count = 0;
			ClearType? type = null;
			state.OnRemoveChoice += (index, str) => wrong = true;
			state.OnClear += (t) => type = t;

			// Test Clear All
			state.AddChoice("a", null);
			state.AddChoice("b", null);
			state.Clear();
			Assert.AreEqual(0, state.Choices.Count);
			Assert.AreEqual(0, state.Consequences.Count);
			Assert.AreEqual(ClearType.ALL, type);

			// Test Clear Dialog
			state.AddChoice("a", null);
			state.AddChoice("b", null);
			state.ClearDialog();
			Assert.AreEqual(2, state.Choices.Count);
			Assert.True(state.Choices[0] == "a");
			Assert.True(state.Choices[1] == "b");
			Assert.AreEqual(2, state.Consequences.Count);
			Assert.True(state.Consequences[0].Count == 0);
			Assert.True(state.Consequences[1].Count == 0);
			Assert.AreEqual(ClearType.DIALOG, type);

			// Test Clear Choices
			state.AddChoice("a", null);
			state.AddChoice("b", null);
			state.ClearChoices();
			Assert.AreEqual(0, state.Choices.Count);
			Assert.AreEqual(0, state.Consequences.Count);
			Assert.AreEqual(ClearType.CHOICES, type);

			// Check callback
			Assert.AreEqual(0, count);
			Assert.IsFalse(wrong);
		}
	}
}

#endif
