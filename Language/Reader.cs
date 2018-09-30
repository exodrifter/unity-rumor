using System;
using System.Text;

namespace Exodrifter.Rumor.Language
{
	/// <summary>
	/// Utility class used for reading Rumor scripts.
	/// </summary>
	public class Reader : ITextPosition
	{
		/// <summary>
		/// A string containing all of the valid characters that can be used to
		/// name a variable.
		/// </summary>
		public const string VALID_VAR_CHARS =
			"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_1234567890";

		#region Properties

		/// <summary>
		/// The contents of the script being read.
		/// </summary>
		public string Script
		{
			get { return script; }
		}
		private readonly string script;

		/// <summary>
		/// The size of each tab.
		/// </summary>
		public int TabSize
		{
			get { return tabSize; }
		}
		private readonly int tabSize;

		/// <summary>
		/// The line number the reader is currently at.
		/// </summary>
		public int Line
		{
			get { return line; }
		}
		private int line = 1;

		/// <summary>
		/// The column number the reader is currently at.
		/// </summary>
		public int Column
		{
			get { return col; }
		}
		private int col = 1;

		/// <summary>
		/// The index the reader is currently at.
		/// </summary>
		public int Index
		{
			get { return index; }
		}
		private int index = 0;

		/// <summary>
		/// True if there is no next character in the reader.
		/// </summary>
		public bool EOF
		{
			get { return index >= script.Length; }
		}

		public const int DEFAULT_TAB_SIZE = 4;

		#endregion

		#region Constructor

		/// <summary>
		/// Creates a new Reader to read the specified string.
		/// </summary>
		/// <param name="script">
		/// The string to read.
		/// </param>
		/// <param name="tabSize">
		/// The number of spaces to treat each tab as.
		/// </param>
		public Reader(string script, int tabSize = DEFAULT_TAB_SIZE)
		{
			if (tabSize < 0)
			{
				throw new ArgumentOutOfRangeException(
					"tabSize",
					"Tab size cannot be negative!"
				);
			}

			this.script = script ?? "";
			this.tabSize = tabSize;
		}

		/// <summary>
		/// Creates a deep copy of another reader.
		/// </summary>
		/// <param name="reader">The reader to copy.</param>
		public Reader(Reader reader)
		{
			this.script = reader.script;
			this.tabSize = reader.tabSize;
			this.index = reader.index;
			this.line = reader.line;
			this.col = reader.col;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Tries to find one of the requested characters at the current
		/// position. If no match is found, a ReadException is thrown.
		/// </summary>
		/// <param name="match">The requested characters to find.</param>
		/// <returns>The character found.</returns>
		public char Expect(params char[] match)
		{
			if (EOF)
			{
				throw new ReadException(this, match);
			}

			var ch = Peek();
			if (match.Contains(ch))
			{
				Read();
				return ch;
			}

			throw new ReadException(this, match);
		}

		/// <summary>
		/// Returns true if the specified string is found in the script starting
		/// at the current position. Always returns false if the reader is at
		/// the end of the file.
		/// </summary>
		/// <param name="match">
		/// The requested string to find.
		/// </param>
		/// <param name="caseSensitive">
		/// True for a case-sensitive search.
		/// </param>
		public bool HasMatch(string match, bool caseSensitive = false)
		{
			if (EOF || match == null)
			{
				return false;
			}

			// Check if the match length is possible
			if (index + match.Length > script.Length)
			{
				return false;
			}

			// Check each character one by one
			for (int i = 0; i < match.Length; ++i)
			{
				var a = match[i];
				var b = script[index + i];

				// Case sensitivity
				if (!caseSensitive)
				{
					a = char.ToLowerInvariant(a);
					b = char.ToLowerInvariant(b);
				}

				if (a != b)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Returns true if the specified token is found in the script starting
		/// at the current position. Always returns false if the reader is at
		/// the end of the file.
		/// </summary>
		/// <param name="match">
		/// The requested token to find.
		/// </param>
		/// <param name="caseSensitive">
		/// True for a case-sensitive search.
		/// </param>
		public bool HasToken(string match, bool caseSensitive = false)
		{
			var isMatch = HasMatch(match, caseSensitive);
			if (isMatch == false)
			{
				return false;
			}

			// A match is a token if it cannot also be the prefix of a variable
			var pos = index + match.Length;
			if (pos < script.Length)
			{
				if (VALID_VAR_CHARS.Contains(script[pos]))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Returns the rest of the line as a string and updates the reader's
		/// position to the beginning of the next line.
		/// </summary>
		/// <returns>The rest of the line as a string.</returns>
		public string NextLine()
		{
			var ret = ReadUntil('\n');
			if (!EOF)
			{
				Expect('\n');
			}

			return ret;
		}

		/// <summary>
		/// Returns the next character in the script. Throws a read exception if
		/// there is no next character.
		/// </summary>
		/// <returns>The next character in the script.</returns>
		public char Peek()
		{
			if (EOF)
			{
				throw new ReadException(this);
			}

			return script[index];
		}

		/// <summary>
		/// Returns the next character in the script and updates the reader's
		/// position. Throws a read exception if there is no next character.
		/// </summary>
		/// <returns>The next character in the script.</returns>
		public char Read()
		{
			if (EOF)
			{
				throw new ReadException(this);
			}

			return ReadInternal();
		}

		/// <summary>
		/// Returns the next characters in the script as a string and updates
		/// the reader's position. Throws a read exception if the remainder of
		/// the script is not long enough for the proper read.
		/// </summary>
		/// <returns>The next string in the script.</returns>
		public string Read(int length)
		{
			if (index + length > script.Length)
			{
				throw new ReadException(this);
			}

			var ret = script.Substring(index, length);

			for (int i = 0; i < length; ++i)
			{
				ReadInternal();
			}

			return ret;
		}

		/// <summary>
		/// Returns the string between the current reader position and either
		/// one of the specified characters or the end of the script and updates
		/// the reader's position.
		/// </summary>
		/// <param name="arr">
		/// The characters to stop reading at.
		/// </param>
		/// <returns>The string found.</returns>
		public string ReadUntil(params char[] arr)
		{
			var start = index;
			var length = 0;

			while (!EOF)
			{
				// Check if reading should stop
				if (arr.Contains(script[index]))
				{
					break;
				}

				ReadInternal();
				length++;
			}

			return script.Substring(start, length);
		}

		private char ReadInternal()
		{
			var ch = script[index++];

			// Update state
			switch (ch)
			{
				case '\n':
					line++;
					col = 1;
					break;

				case '\t':
					col += GetTabDelta();
					break;

				default:
					col++;
					break;
			}

			return ch;
		}

		/// <summary>
		/// Skips spaces and tabs.
		/// </summary>
		/// <returns>The number of columns skipped.</returns>
		public int Skip()
		{
			int skipped = 0;

			while (!EOF)
			{
				var ch = script[index];

				switch (ch)
				{
					case ' ':
						index++;
						col++;
						skipped++;
						continue;

					case '\t':
						index++;
						var delta = GetTabDelta();
						col += delta;
						skipped += delta;
						continue;

					case '\r':
						index++;
						continue;
				}

				break;
			}

			return skipped;
		}

		#endregion

		#region Util

		/// <summary>
		/// Overwrites this reader's state with another. Does not check if the
		/// readers are reading from the same source with the same
		/// configuration.
		/// </summary>
		internal void Update(Reader other)
		{
			this.line = other.line;
			this.col = other.col;
			this.index = other.index;
		}

		/// <summary>
		/// Returns the number of spaces the column position should move in
		/// order to insert a tab.
		/// </summary>
		/// <returns>
		/// The number of spaces the column position should move.
		/// </returns>
		private int GetTabDelta()
		{
			return tabSize - (col - 1) % tabSize;
		}

		#endregion
	}
}
