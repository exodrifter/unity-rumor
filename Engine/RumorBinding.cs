using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Exodrifter.Rumor.Engine
{
	public abstract class RumorBinding
	{
		public abstract object Invoke(object[] args);
	}

	public class BindingAction : RumorBinding
	{
		private Action action;

		public BindingAction(Action action)
		{
			this.action = action;
		}

		public override object Invoke(object[] args)
		{
			action();
			return null;
		}
	}

	public class BindingAction<T1> : RumorBinding
	{
		private Action<T1> action;

		public BindingAction(Action<T1> action)
		{
			this.action = action;
		}

		public override object Invoke(object[] args)
		{
			Convert()(args[0]);
			return null;
		}

		private Action<object> Convert()
		{
			return new Action<object>(
				(a) => { action((T1)a); }
			);
		}
	}

	public class BindingAction<T1, T2> : RumorBinding
	{
		private Action<T1, T2> action;

		public BindingAction(Action<T1, T2> action)
		{
			this.action = action;
		}

		public override object Invoke(object[] args)
		{
			Convert()(args[0], args[1]);
			return null;
		}

		private Action<object, object> Convert()
		{
			return new Action<object, object>(
				(a, b) => { action((T1)a, (T2)b); }
			);
		}
	}

	public class BindingAction<T1, T2, T3> : RumorBinding
	{
		private Action<T1, T2, T3> action;

		public BindingAction(Action<T1, T2, T3> action)
		{
			this.action = action;
		}

		public override object Invoke(object[] args)
		{
			Convert()(args[0], args[1], args[2]);
			return null;
		}

		private Action<object, object, object> Convert()
		{
			return new Action<object, object, object>(
				(a, b, c) => { action((T1)a, (T2)b, (T3)c); }
			);
		}
	}

	public class BindingAction<T1, T2, T3, T4> : RumorBinding
	{
		private Action<T1, T2, T3, T4> action;

		public BindingAction(Action<T1, T2, T3, T4> action)
		{
			this.action = action;
		}

		public override object Invoke(object[] args)
		{
			Convert()(args[0], args[1], args[2], args[3]);
			return null;
		}

		private Action<object, object, object, object> Convert()
		{
			return new Action<object, object, object, object>(
				(a, b, c, d) => { action((T1)a, (T2)b, (T3)c, (T4)d); }
			);
		}
	}

	public class BindingFunc<TResult> : RumorBinding
	{
		private Func<TResult> func;

		public BindingFunc(Func<TResult> func)
		{
			this.func = func;
		}

		public override object Invoke(object[] args)
		{
			return func();
		}
	}

	public class BindingFunc<T1, TResult> : RumorBinding
	{
		private Func<T1, TResult> func;

		public BindingFunc(Func<T1, TResult> func)
		{
			this.func = func;
		}

		public override object Invoke(object[] args)
		{
			return Convert()(args[0]);
		}

		private Func<object, object> Convert()
		{
			return new Func<object, object>(
				(a) => { return func((T1)a); }
			);
		}
	}

	public class BindingFunc<T1, T2, TResult> : RumorBinding
	{
		private Func<T1, T2, TResult> func;

		public BindingFunc(Func<T1, T2, TResult> func)
		{
			this.func = func;
		}

		public override object Invoke(object[] args)
		{
			return Convert()(args[0], args[1]);
		}

		private Func<object, object, object> Convert()
		{
			return new Func<object, object, object>(
				(a, b) => { return func((T1)a, (T2)b); }
			);
		}
	}

	public class BindingFunc<T1, T2, T3, TResult> : RumorBinding
	{
		private Func<T1, T2, T3, TResult> func;

		public BindingFunc(Func<T1, T2, T3, TResult> func)
		{
			this.func = func;
		}

		public override object Invoke(object[] args)
		{
			return Convert()(args[0], args[1], args[2]);
		}

		private Func<object, object, object, object> Convert()
		{
			return new Func<object, object, object, object>(
				(a, b, c) => { return func((T1)a, (T2)b, (T3)c); }
			);
		}
	}

	public class BindingFunc<T1, T2, T3, T4, TResult> : RumorBinding
	{
		private Func<T1, T2, T3, T4, TResult> func;

		public BindingFunc(Func<T1, T2, T3, T4, TResult> func)
		{
			this.func = func;
		}

		public override object Invoke(object[] args)
		{
			return Convert()(args[0], args[1], args[2], args[3]);
		}

		private Func<object, object, object, object, object> Convert()
		{
			return new Func<object, object, object, object, object>(
				(a, b, c, d) => { return func((T1)a, (T2)b, (T3)c, (T4)d); }
			);
		}
	}
}
