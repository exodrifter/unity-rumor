#if UNITY_EDITOR

using NUnit.Framework;
using Exodrifter.Rumor.Language;

namespace Exodrifter.Rumor.Test.Lang
{
	internal sealed class ReaderTest
	{
		#region Clone

		/// <summary>
		/// Checks if the deep copy works as expected.
		/// <summary>
		[Test]
		public void Clone()
		{
			var a = new Reader("abcd");
			var b = new Reader(a);

			a.Read();
			Assert.AreEqual('b', a.Peek());
			Assert.AreEqual('a', b.Peek());
			b.Read();
			Assert.AreEqual('b', a.Peek());
			Assert.AreEqual('b', b.Peek());

			a.Read();
			b = new Reader(a);
			Assert.AreEqual('c', a.Peek());
			Assert.AreEqual('c', b.Peek());
			a.Read();
			Assert.AreEqual('d', a.Peek());
			Assert.AreEqual('c', b.Peek());
			b.Read();
			Assert.AreEqual('d', a.Peek());
			Assert.AreEqual('d', b.Peek());
		}

		#endregion

		#region Expect

		/// <summary>
		/// Checks if the reader handles a null script correctly while
		/// expecting.
		/// </summary>
		[Test]
		public void ExpectNull()
		{
			var reader = new Reader(null, 4);

			Assert.Throws<ReadException>(() => reader.Expect('\0'));
			Check(0, 1, 1, reader);
		}

		/// <summary>
		/// Checks if the reader handles a null script correctly while
		/// expecting.
		/// </summary>
		[Test]
		public void ExpectEmpty()
		{
			var reader = new Reader("", 4);

			Assert.Throws<ReadException>(() => reader.Expect('\0'));
			Check(0, 1, 1, reader);
		}

		/// <summary>
		/// Checks if the reader can expect characters properly.
		/// </summary>
		[Test]
		public void ExpectTest()
		{
			var reader = new Reader("abcd", 4);

			Assert.DoesNotThrow(() => reader.Expect('a'));
			Check(1, 1, 2, reader);

			Assert.DoesNotThrow(() => reader.Expect('b'));
			Check(2, 1, 3, reader);

			Assert.DoesNotThrow(() => reader.Expect('c'));
			Check(3, 1, 4, reader);

			Assert.DoesNotThrow(() => reader.Expect('d'));
			Check(4, 1, 5, reader);

			// Check whitespace
			reader = new Reader("\n\t ", 4);

			Assert.DoesNotThrow(() => reader.Expect('\n'));
			Check(1, 2, 1, reader);

			Assert.DoesNotThrow(() => reader.Expect('\t'));
			Check(2, 2, 5, reader);

			Assert.DoesNotThrow(() => reader.Expect(' '));
			Check(3, 2, 6, reader);
		}

		/// <summary>
		/// Checks if the reader works correctly if the wrong character is
		/// expected.
		/// </summary>
		[Test]
		public void ExpectWrongTest()
		{
			var reader = new Reader("abcd", 4);

			Assert.Throws<ReadException>(() => reader.Expect('b'));
			Check(0, 1, 1, reader);

			Assert.DoesNotThrow(() => reader.Expect('a'));
			Check(1, 1, 2, reader);
		}

		#endregion

		#region HasMatch

		/// <summary>
		/// Checks if the reader handles a null script correctly while checking
		/// for a match.
		/// </summary>
		[Test]
		public void HasMatchNull()
		{
			var reader = new Reader(null, 4);

			Assert.False(reader.HasMatch(""));
			Check(0, 1, 1, reader);

			Assert.False(reader.HasMatch(""));
			Check(0, 1, 1, reader);

			Assert.False(reader.HasMatch(null));
			Check(0, 1, 1, reader);
		}

		/// <summary>
		/// Checks if the reader handles an empty script correctly while
		/// checking for a match.
		/// </summary>
		[Test]
		public void HasMatchEmpty()
		{
			var reader = new Reader("", 4);

			Assert.False(reader.HasMatch(""));
			Check(0, 1, 1, reader);

			Assert.False(reader.HasMatch(""));
			Check(0, 1, 1, reader);
		}

		/// <summary>
		/// Checks if the reader checks for matches properly.
		/// </summary>
		[Test]
		public void HasMatchTest()
		{
			var reader = new Reader("abcd", 4);

			Assert.True(reader.HasMatch("abcd"));
			Check(0, 1, 1, reader);

			Assert.True(reader.HasMatch("abc"));
			Check(0, 1, 1, reader);

			Assert.True(reader.HasMatch("a"));
			Check(0, 1, 1, reader);

			Assert.True(reader.HasMatch(""));
			Check(0, 1, 1, reader);

			Assert.False(reader.HasMatch("bcd"));
			Check(0, 1, 1, reader);

			Assert.False(reader.HasMatch("bc"));
			Check(0, 1, 1, reader);

			Assert.False(reader.HasMatch("b"));
			Check(0, 1, 1, reader);

			Assert.False(reader.HasMatch(null));
			Check(0, 1, 1, reader);

			reader.Read();

			Assert.False(reader.HasMatch("abcd"));
			Check(1, 1, 2, reader);

			Assert.False(reader.HasMatch("abc"));
			Check(1, 1, 2, reader);

			Assert.False(reader.HasMatch("ab"));
			Check(1, 1, 2, reader);

			Assert.False(reader.HasMatch("a"));
			Check(1, 1, 2, reader);

			Assert.True(reader.HasMatch("bcd"));
			Check(1, 1, 2, reader);

			Assert.True(reader.HasMatch("bc"));
			Check(1, 1, 2, reader);

			Assert.True(reader.HasMatch("b"));
			Check(1, 1, 2, reader);

			Assert.False(reader.HasMatch(null));
			Check(1, 1, 2, reader);
		}

		#endregion

		#region HasToken

		/// <summary>
		/// Checks if the reader handles a null script correctly while checking
		/// for a token.
		/// </summary>
		[Test]
		public void HasTokenNull()
		{
			var reader = new Reader(null, 4);

			Assert.False(reader.HasMatch(""));
			Check(0, 1, 1, reader);

			Assert.False(reader.HasMatch(""));
			Check(0, 1, 1, reader);

			Assert.False(reader.HasMatch(null));
			Check(0, 1, 1, reader);
		}

		/// <summary>
		/// Checks if the reader handles an empty script correctly while
		/// checking for a token.
		/// </summary>
		[Test]
		public void HasTokenEmpty()
		{
			var reader = new Reader("", 4);

			Assert.False(reader.HasMatch(""));
			Check(0, 1, 1, reader);

			Assert.False(reader.HasMatch(""));
			Check(0, 1, 1, reader);
		}

		/// <summary>
		/// Checks if the reader checks for tokensproperly.
		/// </summary>
		[Test]
		public void HasTokenTest()
		{
			var reader = new Reader("abcd", 4);

			Assert.True(reader.HasToken("abcd"));
			Check(0, 1, 1, reader);

			Assert.False(reader.HasToken("abc"));
			Check(0, 1, 1, reader);

			reader = new Reader("abcd:", 4);

			Assert.True(reader.HasToken("abcd"));
			Check(0, 1, 1, reader);

			reader = new Reader("abcd)", 4);

			Assert.True(reader.HasToken("abcd"));
			Check(0, 1, 1, reader);

			reader = new Reader("abcd ", 4);

			Assert.True(reader.HasToken("abcd"));
			Check(0, 1, 1, reader);
		}

		#endregion

		#region Peek

		/// <summary>
		/// Checks if the reader handles a null script correctly while peeking.
		/// </summary>
		[Test]
		public void PeekNull()
		{
			var reader = new Reader(null, 4);

			Assert.Throws<ReadException>(() => reader.Peek());
			Check(0, 1, 1, reader);
		}

		/// <summary>
		/// Checks if the reader handles an empty script correctly while
		/// peeking.
		/// </summary>
		[Test]
		public void PeekEmpty()
		{
			var reader = new Reader("", 4);

			Assert.Throws<ReadException>(() => reader.Peek());
			Check(0, 1, 1, reader);
		}

		/// <summary>
		/// Checks if the reader peeks characters properly.
		/// </summary>
		[Test]
		public void PeekTest()
		{
			var reader = new Reader("abcd", 4);

			var ch = reader.Peek();
			Assert.AreEqual('a', ch);
			Check(0, 1, 1, reader);
			reader.Read(); // a
			Check(1, 1, 2, reader);

			ch = reader.Peek();
			Assert.AreEqual('b', ch);
			Check(1, 1, 2, reader);
			reader.Read(); // b
			Check(2, 1, 3, reader);

			ch = reader.Peek();
			Assert.AreEqual('c', ch);
			Check(2, 1, 3, reader);
			reader.Read(); // c
			Check(3, 1, 4, reader);

			ch = reader.Peek();
			Assert.AreEqual('d', ch);
			Check(3, 1, 4, reader);
			reader.Read(); // d
			Check(4, 1, 5, reader);

			// Check whitespace
			reader = new Reader("\n\t ", 4);

			ch = reader.Peek();
			Assert.AreEqual('\n', ch);
			Check(0, 1, 1, reader);
			reader.Read(); // '\n'
			Check(1, 2, 1, reader);
			
			ch = reader.Peek();
			Assert.AreEqual('\t', ch);
			Check(1, 2, 1, reader);
			reader.Read(); // '\t'
			Check(2, 2, 5, reader);
			
			ch = reader.Peek();
			Assert.AreEqual(' ', ch);
			Check(2, 2, 5, reader);
			reader.Read(); // ' '
			Check(3, 2, 6, reader);
		}

		/// <summary>
		/// Checks if the multiple peek calls works correctly.
		/// </summary>
		public void PeekSame()
		{
			var reader = new Reader("abcd", 4);

			var ch = reader.Peek();
			Assert.AreEqual('a', ch);
			Check(1, 1, 1, reader);

			ch = reader.Peek();
			Assert.AreEqual('a', ch);
			Check(1, 1, 1, reader);

			reader = new Reader("\n\t ", 4);

			ch = reader.Peek();
			Assert.AreEqual('\n', ch);
			Check(1, 1, 1, reader);

			ch = reader.Peek();
			Assert.AreEqual('\n', ch);
			Check(1, 1, 1, reader);
		}

		#endregion

		#region Read

		/// <summary>
		/// Checks if the reader handles a null script correctly while reading.
		/// </summary>
		[Test]
		public void ReadNull()
		{
			var reader = new Reader(null, 4);

			Assert.Throws<ReadException>(() => reader.Peek());
			Check(0, 1, 1, reader);
		}

		/// <summary>
		/// Checks if the reader handles an empty script correctly while
		/// reading.
		/// </summary>
		[Test]
		public void ReadEmpty()
		{
			var reader = new Reader("", 4);

			Assert.Throws<ReadException>(() => reader.Peek());
			Check(0, 1, 1, reader);
		}

		/// <summary>
		/// Checks if the reader reads characters properly.
		/// </summary>
		[Test]
		public void ReadTest()
		{
			var reader = new Reader("abcd", 4);

			var ch = reader.Read();
			Assert.AreEqual('a', ch);
			Check(1, 1, 2, reader);

			ch = reader.Read();
			Assert.AreEqual('b', ch);
			Check(2, 1, 3, reader);

			ch = reader.Read();
			Assert.AreEqual('c', ch);
			Check(3, 1, 4, reader);

			ch = reader.Read();
			Assert.AreEqual('d', ch);
			Check(4, 1, 5, reader);

			// Check whitespace
			reader = new Reader("\n\t ", 4);

			ch = reader.Read();
			Assert.AreEqual('\n', ch);
			Check(1, 2, 1, reader);
			
			ch = reader.Read();
			Assert.AreEqual('\t', ch);
			Check(2, 2, 5, reader);
			
			ch = reader.Read();
			Assert.AreEqual(' ', ch);
			Check(3, 2, 6, reader);

		}

		/// <summary>
		/// Checks if the reader reads strings containing null terminators
		/// properly.
		/// </summary>
		[Test]
		public void ReadNullTerminator()
		{
			var reader = new Reader("a\0b\0", 4);

			var ch = reader.Read();
			Assert.AreEqual('a', ch);
			Check(1, 1, 2, reader);

			ch = reader.Read();
			Assert.AreEqual('\0', ch);
			Check(2, 1, 3, reader);

			ch = reader.Read();
			Assert.AreEqual('b', ch);
			Check(3, 1, 4, reader);

			ch = reader.Read();
			Assert.AreEqual('\0', ch);
			Check(4, 1, 5, reader);
		}

		/// <summary>
		/// Checks if the reader reads spaces properly.
		/// </summary>
		[Test]
		public void ReadSpaces()
		{
			var reader = new Reader("    ", 4);

			var ch = reader.Read();
			Assert.AreEqual(' ', ch);
			Check(1, 1, 2, reader);

			ch = reader.Read();
			Assert.AreEqual(' ', ch);
			Check(2, 1, 3, reader);

			ch = reader.Read();
			Assert.AreEqual(' ', ch);
			Check(3, 1, 4, reader);

			ch = reader.Read();
			Assert.AreEqual(' ', ch);
			Check(4, 1, 5, reader);
		}

		/// <summary>
		/// Checks if the reader reads tabs properly.
		/// </summary>
		[Test]
		public void ReadTabs()
		{
			var reader = new Reader("\t\t\t\t", 4);

			var ch = reader.Read();
			Assert.AreEqual('\t', ch);
			Check(1, 1, 5, reader);

			ch = reader.Read();
			Assert.AreEqual('\t', ch);
			Check(2, 1, 9, reader);

			ch = reader.Read();
			Assert.AreEqual('\t', ch);
			Check(3, 1, 13, reader);

			ch = reader.Read();
			Assert.AreEqual('\t', ch);
			Check(4, 1, 17, reader);
		}

		/// <summary>
		/// Checks if the reader reads spaces and tabs properly.
		/// </summary>
		[Test]
		public void ReadSpacesAndTabs()
		{
			var reader = new Reader("\t \t ", 4);

			var ch = reader.Read();
			Assert.AreEqual('\t', ch);
			Check(1, 1, 5, reader);

			ch = reader.Read();
			Assert.AreEqual(' ', ch);
			Check(2, 1, 6, reader);

			ch = reader.Read();
			Assert.AreEqual('\t', ch);
			Check(3, 1, 9, reader);

			ch = reader.Read();
			Assert.AreEqual(' ', ch);
			Check(4, 1, 10, reader);
		}

		/// <summary>
		/// Checks if the reader reads new lines properly.
		/// </summary>
		[Test]
		public void ReadNewLines()
		{
			var reader = new Reader("\n \n\t", 4);

			var ch = reader.Read();
			Assert.AreEqual('\n', ch);
			Check(1, 2, 1, reader);

			ch = reader.Read();
			Assert.AreEqual(' ', ch);
			Check(2, 2, 2, reader);

			ch = reader.Read();
			Assert.AreEqual('\n', ch);
			Check(3, 3, 1, reader);

			ch = reader.Read();
			Assert.AreEqual('\t', ch);
			Check(4, 3, 5, reader);
		}

		/// <summary>
		/// Checks if the reader reads carriage returns properly.
		/// </summary>
		[Test]
		public void ReadCarriageReturns()
		{
			var reader = new Reader("\r\n \r", 4);

			var ch = reader.Read();
			Assert.AreEqual('\r', ch);
			Check(1, 1, 2, reader);

			ch = reader.Read();
			Assert.AreEqual('\n', ch);
			Check(2, 2, 1, reader);

			ch = reader.Read();
			Assert.AreEqual(' ', ch);
			Check(3, 2, 2, reader);

			ch = reader.Read();
			Assert.AreEqual('\r', ch);
			Check(4, 2, 3, reader);
		}

		#endregion

		#region Read Until

		/// <summary>
		/// Checks if the reader handles a null script correctly while
		/// performing a read until.
		/// </summary>
		[Test]
		public void ReadUntilNull()
		{
			var reader = new Reader(null, 4);

			var str = reader.ReadUntil();
			Assert.AreEqual("", str);
			Check(0, 1, 1, reader);
		}

		/// <summary>
		/// Checks if the reader handles an empty script correctly while
		/// performing a read until.
		/// </summary>
		[Test]
		public void ReadUntilEmpty()
		{
			var reader = new Reader("", 4);

			var str = reader.ReadUntil();
			Assert.AreEqual("", str);
			Check(0, 1, 1, reader);
		}

		/// <summary>
		/// Checks if the reader handles a read until operation with no stops.
		/// </summary>
		[Test]
		public void ReadUntilAllTest()
		{
			var reader = new Reader("abcd", 4);
			var str = reader.ReadUntil();
			Assert.AreEqual("abcd", str);
			Check(4, 1, 5, reader);

			reader = new Reader("ab\ncd", 4);
			str = reader.ReadUntil();
			Assert.AreEqual("ab\ncd", str);
			Check(5, 2, 3, reader);

			// Check whitespace
			reader = new Reader("  \n\t\t", 4);
			str = reader.ReadUntil();
			Assert.AreEqual("  \n\t\t", str);
			Check(5, 2, 9, reader);
		}

		/// <summary>
		/// Checks if the reader handles a read until operation with multiple
		/// stops.
		/// </summary>
		[Test]
		public void ReadUntilMultipleTest()
		{
			var reader = new Reader("abcdxyz", 4);
			var str = reader.ReadUntil('x', 'y', 'z');
			Assert.AreEqual("abcd", str);
			Check(4, 1, 5, reader);

			reader = new Reader("abcdyzx", 4);
			str = reader.ReadUntil('x', 'y', 'z');
			Assert.AreEqual("abcd", str);
			Check(4, 1, 5, reader);

			reader = new Reader("abcdzxy", 4);
			str = reader.ReadUntil('x', 'y', 'z');
			Assert.AreEqual("abcd", str);
			Check(4, 1, 5, reader);
		}

		/// <summary>
		/// Checks if the reader handles a read until operation with a requested
		/// stop at a space.
		/// </summary>
		[Test]
		public void ReadUntilSpace()
		{
			var reader = new Reader(" ", 4);
			var str = reader.ReadUntil(' ');
			Assert.AreEqual("", str);
			Check(0, 1, 1, reader);

			reader = new Reader("ab cd", 4);
			str = reader.ReadUntil(' ');
			Assert.AreEqual("ab", str);
			Check(2, 1, 3, reader);
		}

		/// <summary>
		/// Checks if the reader handles a read until operation with a requested
		/// stop at a tab.
		/// </summary>
		[Test]
		public void ReadUntilTab()
		{
			var reader = new Reader("\t", 4);
			var str = reader.ReadUntil('\t');
			Assert.AreEqual("", str);
			Check(0, 1, 1, reader);

			reader = new Reader("ab\tcd", 4);
			str = reader.ReadUntil('\t');
			Assert.AreEqual("ab", str);
			Check(2, 1, 3, reader);
		}

		/// <summary>
		/// Checks if the reader handles a read until operation with a requested
		/// stop at a space or tab.
		/// </summary>
		[Test]
		public void ReadUntilSpaceOrTab()
		{
			var reader = new Reader(" \t", 4);
			var str = reader.ReadUntil(' ', '\t');
			Assert.AreEqual("", str);
			Check(0, 1, 1, reader);

			reader = new Reader("\t ", 4);
			str = reader.ReadUntil(' ', '\t');
			Assert.AreEqual("", str);
			Check(0, 1, 1, reader);

			reader = new Reader("ab \tcd", 4);
			str = reader.ReadUntil(' ', '\t');
			Assert.AreEqual("ab", str);
			Check(2, 1, 3, reader);

			reader = new Reader("ab\t cd", 4);
			str = reader.ReadUntil(' ', '\t');
			Assert.AreEqual("ab", str);
			Check(2, 1, 3, reader);
		}

		#endregion

		#region Skip

		/// <summary>
		/// Checks if the reader handles a null script correctly while skipping.
		/// </summary>
		[Test]
		public void SkipNull()
		{
			var reader = new Reader(null, 4);

			var count = reader.Skip();
			Assert.AreEqual(0, count);
			Check(0, 1, 1, reader);
		}

		/// <summary>
		/// Checks if the reader handles an empty script correctly while
		/// skipping.
		/// </summary>
		[Test]
		public void SkipEmpty()
		{
			var reader = new Reader("", 4);

			var count = reader.Skip();
			Assert.AreEqual(0, count);
			Check(0, 1, 1, reader);
		}

		/// <summary>
		/// Checks if the reader skips characters it isn't supposed to.
		/// </summary>
		[Test]
		public void SkipTest()
		{
			var reader = new Reader("a b\tc", 4);

			var count = reader.Skip();
			Assert.AreEqual(0, count);
			Check(0, 1, 1, reader);

			reader.Read(); // a

			count = reader.Skip();
			Assert.AreEqual(1, count);
			Check(2, 1, 3, reader);

			reader.Read(); // b

			count = reader.Skip();
			Assert.AreEqual(1, count);
			Check(4, 1, 5, reader);

			count = reader.Skip();
			Assert.AreEqual(0, count);
			Check(4, 1, 5, reader);
		}

		/// <summary>
		/// Checks if the reader handles different tab sizes correctly.
		/// </summary>
		[Test]
		public void SkipTabSizeTest()
		{
			var reader = new Reader("a b\tab\ta\t", 3);

			reader.Read(); // 'a'
			reader.Read(); // ' '
			reader.Read(); // 'b'
			var ch = reader.Read();
			Assert.AreEqual('\t', ch);
			Check(4, 1, 7, reader);

			reader.Read(); // 'a'
			reader.Read(); // 'b'
			ch = reader.Read();
			Assert.AreEqual('\t', ch);
			Check(7, 1, 10, reader);

			reader.Read(); // 'a'
			ch = reader.Read();
			Assert.AreEqual('\t', ch);
			Check(9, 1, 13, reader);

			reader = new Reader("12345678\ta\t", 8);

			reader.Read(); // '1'
			reader.Read(); // '2'
			reader.Read(); // '3'
			reader.Read(); // '4'
			reader.Read(); // '5'
			reader.Read(); // '6'
			reader.Read(); // '7'
			reader.Read(); // '8'
			ch = reader.Read();
			Assert.AreEqual('\t', ch);
			Check(9, 1, 17, reader);

			reader.Read(); // 'a'
			ch = reader.Read();
			Assert.AreEqual('\t', ch);
			Check(11, 1, 25, reader);
		}

		/// <summary>
		/// Checks if the reader handles spaces correctly.
		/// </summary>
		[Test]
		public void SkipSpaces()
		{
			var reader = new Reader("    ", 4);

			var count = reader.Skip();
			Assert.AreEqual(4, count);
			Check(4, 1, 5, reader);
		}

		/// <summary>
		/// Checks if the reader handles tabs correctly.
		/// </summary>
		[Test]
		public void SkipTabs()
		{
			var reader = new Reader("\t\t\t\t", 4);

			var count = reader.Skip();
			Assert.AreEqual(16, count);
			Check(4, 1, 17, reader);
		}

		/// <summary>
		/// Checks if the reader handles mixed spaces and tabs correctly.
		/// </summary>
		[Test]
		public void SkipSpacesAndTabs()
		{
			var reader = new Reader("\t \t ", 4);

			var count = reader.Skip();
			Assert.AreEqual(9, count);
			Check(4, 1, 10, reader);
		}

		#endregion

		#region Util

		/// <summary>
		/// Asserts that the specified properties are set to the correct value.
		/// </summary>
		private void Check(int pos, int line, int col, Reader reader)
		{
			Assert.AreEqual(pos, reader.Index);
			Assert.AreEqual(line, reader.Line);
			Assert.AreEqual(col, reader.Column);
		}

		#endregion
	}
}

#endif
