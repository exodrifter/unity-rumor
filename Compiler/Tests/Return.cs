﻿using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;
using NUnit.Framework;

namespace Exodrifter.Rumor.Compiler.Tests
{
	public static class Return
	{
		[Test]
		public static void ReturnSuccess()
		{
			var state = new State("return", 4, 0);

			var node = Compiler.Return(state);
			Assert.AreEqual(new ReturnNode(), node);
		}
	}
}
