using System;
using System.Collections.Generic;
using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Parser;

namespace Exodrifter.Rumor.Compiler
{
	public class RumorCompiler
	{
		private RumorParserState userState;

		public RumorCompiler()
		{
			userState = new RumorParserState();
		}

		public Dictionary<string, List<Node>> Compile(string script)
		{
			var state = new ParserState(script, tabSize, userState);
			return Compiler.Script(state);
		}

		#region Settings

		private int tabSize = 4;

		public RumorCompiler SetTabSize(int tabSize)
		{
			this.tabSize = tabSize;
			return this;
		}

		#endregion

		#region Bindings

		public RumorCompiler LinkAction(string name)
		{
			userState.LinkAction(name);
			return this;
		}

		public RumorCompiler LinkAction(string name, Engine.ValueType p1)
		{
			userState.LinkAction(name, p1);
			return this;
		}

		public RumorCompiler LinkAction
			(string name, Engine.ValueType p1, Engine.ValueType p2)
		{
			userState.LinkAction(name, p1, p2);
			return this;
		}

		public RumorCompiler LinkAction
			(string name, Engine.ValueType p1, Engine.ValueType p2,
			Engine.ValueType p3)
		{
			userState.LinkAction(name, p1, p2, p3);
			return this;
		}

		public RumorCompiler LinkAction
			(string name, Engine.ValueType p1, Engine.ValueType p2,
			Engine.ValueType p3, Engine.ValueType p4)
		{
			userState.LinkAction(name, p1, p2, p3, p4);
			return this;
		}

		public RumorCompiler LinkFunction(string name, Engine.ValueType result)
		{
			userState.LinkFunction(name, result);
			return this;
		}

		public RumorCompiler LinkFunction
			(string name, Engine.ValueType p1, Engine.ValueType result)
		{
			userState.LinkFunction(name, p1, result);
			return this;
		}

		public RumorCompiler LinkFunction
			(string name, Engine.ValueType p1, Engine.ValueType p2,
			Engine.ValueType result)
		{
			userState.LinkFunction(name, p1, p2, result);
			return this;
		}

		public RumorCompiler LinkFunction
			(string name, Engine.ValueType p1, Engine.ValueType p2,
			Engine.ValueType p3, Engine.ValueType result)
		{
			userState.LinkFunction(name, p1, p2, p3, result);
			return this;
		}

		public RumorCompiler LinkFunction
			(string name, Engine.ValueType p1, Engine.ValueType p2,
			Engine.ValueType p3, Engine.ValueType p4, Engine.ValueType result)
		{
			userState.LinkFunction(name, p1, p2, p3, p4, result);
			return this;
		}

		#endregion
	}
}
