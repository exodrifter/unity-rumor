using System;

namespace Exodrifter.Rumor.Engine
{
	public abstract class BindingHint { }

	public class BindingActionHint : BindingHint
	{
		public BindingActionHint() { }
	}

	public class BindingActionHint1 : BindingHint
	{
		public ValueType t1;

		public BindingActionHint1(ValueType t1)
		{
			this.t1 = t1;
		}
	}

	public class BindingActionHint2 : BindingHint
	{
		public ValueType t1;
		public ValueType t2;

		public BindingActionHint2(ValueType t1, ValueType t2)
		{
			this.t1 = t1;
			this.t2 = t2;
		}
	}

	public class BindingActionHint3 : BindingHint
	{
		public ValueType t1;
		public ValueType t2;
		public ValueType t3;

		public BindingActionHint3(ValueType t1, ValueType t2, ValueType t3)
		{
			this.t1 = t1;
			this.t2 = t2;
			this.t3 = t3;
		}
	}

	public class BindingActionHint4 : BindingHint
	{
		public ValueType t1;
		public ValueType t2;
		public ValueType t3;
		public ValueType t4;

		public BindingActionHint4
			(ValueType t1, ValueType t2, ValueType t3, ValueType t4)
		{
			this.t1 = t1;
			this.t2 = t2;
			this.t3 = t3;
			this.t4 = t4;
		}
	}

	public class BindingFunctionHint : BindingHint
	{
		public ValueType result;

		public BindingFunctionHint(ValueType result)
		{
			this.result = result;
		}
	}

	public class BindingFunctionHint1 : BindingHint
	{
		public ValueType t1;
		public ValueType result;

		public BindingFunctionHint1(ValueType t1, ValueType result)
		{
			this.t1 = t1;
			this.result = result;
		}
	}

	public class BindingFunctionHint2 : BindingHint
	{
		public ValueType t1;
		public ValueType t2;
		public ValueType result;

		public BindingFunctionHint2(ValueType t1, ValueType t2, ValueType result)
		{
			this.t1 = t1;
			this.t2 = t2;
			this.result = result;
		}
	}

	public class BindingFunctionHint3 : BindingHint
	{
		public ValueType t1;
		public ValueType t2;
		public ValueType t3;
		public ValueType result;

		public BindingFunctionHint3
			(ValueType t1, ValueType t2, ValueType t3, ValueType result)
		{
			this.t1 = t1;
			this.t2 = t2;
			this.t3 = t3;
			this.result = result;
		}
	}

	public class BindingFunctionHint4 : BindingHint
	{
		public ValueType t1;
		public ValueType t2;
		public ValueType t3;
		public ValueType t4;
		public ValueType result;

		public BindingFunctionHint4
			(ValueType t1, ValueType t2, ValueType t3, ValueType t4,
			ValueType result)
		{
			this.t1 = t1;
			this.t2 = t2;
			this.t3 = t3;
			this.t4 = t4;
			this.result = result;
		}
	}
}
