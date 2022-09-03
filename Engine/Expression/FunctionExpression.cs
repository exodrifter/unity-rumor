using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public abstract class FunctionExpression : Expression, ISerializable
	{
		public FunctionExpression() {}
	}
}
