namespace Exodrifter.Rumor.Parser
{
	public static partial class Parse
	{
		/// <summary>
		/// Parses an optional sign
		/// </summary>
		private static Parser<string> Sign =>
			Char('-').Or(Char('+')).String().Or(Pure(""));

		/// <summary>
		/// Parses an integer number.
		/// </summary>
		private static Parser<string> Number
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						var digit = Digit(state);
						var rest = Digit.Or(Char('_')).Many().String()(state);

						transaction.CommitIndex();
						return digit + rest;
					}
				};
			}
		}

		/// <summary>
		/// Parses a number with a decimal component.
		/// </summary>
		private static Parser<string> Decimal
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						var l = Number(state);
						transaction.CommitIndex();

						try
						{
							var p = Char('.')(state);
							var r = Number(state);

							transaction.CommitIndex();
							return l + p + r;
						}
						catch (ParserException)
						{
							return l;
						}
					}
				};
			}
		}

		/// <summary>
		/// Parses a number with a decimal component and an optional sign.
		/// </summary>
		private static Parser<string> SignedDecimal
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						var sign = Sign(state);
						var number = Decimal(state);

						transaction.CommitIndex();
						return sign + number;
					}
				};
			}
		}

		/// <summary>
		/// Parses a double with an optional sign.
		/// </summary>
		public static Parser<double> Double
		{
			get
			{
				return state =>
				{
					using (var transaction = new Transaction(state))
					{
						var str = SignedDecimal(state);

						double result;
						if (!double.TryParse(str, out result))
						{
							throw new FormatException(state.Index, "double",
								"this might be a bug, please file a bug report"
							);
						}

						transaction.CommitIndex();
						return result;
					}
				};
			}
		}
	}
}
