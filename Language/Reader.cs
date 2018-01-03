using System;
using System.Text;

namespace Exodrifter.Rumor.Language
{
	/// <summary>
	/// Utility class used for reading Rumor scripts.
	/// </summary>
	public class Reader : ITextPosition
	{
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

			// Read from current position
			string str = "";
			for (int i = 0; i < match.Length; ++i)
			{
				if (index + i < script.Length)
				{
					str += script[index + i];
				}
			}

			// Case sensitivity
			if (!caseSensitive)
			{
				match = match.ToLower();
				str = str.ToLower();
			}

			// Check match
			if (match == str)
			{
				return true;
			}
			return false;
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

			char ret = script[index];

			// Update state
			if (ret == '\t')
			{
				col += GetTabDelta();
			}
			else if (ret != '\n')
			{
				col++;
			}
			else
			{
				line++;
				col = 1;
			}

			index++;
			return ret;
		}

		/// <summary>
		/// Returns the next characters in the script as a string and updates
		/// the reader's position. Throws a read exception if the remainder of
		/// the script is not long enough for the proper read.
		/// </summary>
		/// <returns>The next string in the script.</returns>
		public string Read(int length)
		{
			var builder = new StringBuilder();

			for (int i = 0; i < length; ++i)
			{
				builder.Append(Read());
			}

			return builder.ToString();
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
			var builder = new StringBuilder();

			while (!EOF)
			{
				// Check if reading should stop
				var ch = script[index];
				if (arr.Contains(ch))
				{
					break;
				}

				// Read
				builder.Append(ch);

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

				index++;
			}

			return builder.ToString();
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
				}

				break;
			}

			return skipped;
		}

		#endregion

		#region Util

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
