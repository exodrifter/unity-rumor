using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	[Serializable]
	public class NumberVariable : Expression, ISerializable
	{
		private readonly string name;

		public NumberVariable(string name)
		{
			this.name = name;
		}

		public override Value Evaluate(RumorScope scope)
		{
			var value = scope.Get(name);
			if (value == null)
			{
				throw new UndefinedVariableException(
					"Variable \"" + name+ "\" has not been defined yet!"
				);
			}
			if (!(value is NumberValue))
			{
				throw new VariableTypeException(
					"Variable is not a number!"
				);
			}

			return (NumberValue)value;
		}

		public override Expression Simplify()
		{
			return this;
		}

		#region Equality

		public override bool Equals(object obj)
		{
			return Equals(obj as NumberVariable);
		}

		public bool Equals(NumberVariable other)
		{
			if (other == null)
			{
				return false;
			}

			return Equals(name, other.name);
		}

		public override int GetHashCode()
		{
			return name.GetHashCode();
		}

		#endregion

		#region Serialization

		public NumberVariable
			(SerializationInfo info, StreamingContext context)
		{
			name = info.GetValue<string>("name");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<string>("name", name);
		}

		#endregion

		public override string ToString()
		{
			return name;
		}
	}
}
