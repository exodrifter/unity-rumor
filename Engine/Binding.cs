using System;

namespace Exodrifter.Rumor.Engine
{
	public abstract class Binding
	{
		public abstract object Invoke(object[] args);
	}

	public class BindingAction : Binding
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

	public class BindingAction<T1> : Binding
	{
		private Action<T1> action;

		public BindingAction(Action<T1> action)
		{
			this.action = action;
		}

		public override object Invoke(object[] args)
		{
			Bind()(args[0]);
			return null;
		}

		private Action<object> Bind()
		{
			return new Action<object>(
				(a) => { action(
					(T1)Convert.ChangeType(a, typeof(T1))
				);}
			);
		}
	}

	public class BindingAction<T1, T2> : Binding
	{
		private Action<T1, T2> action;

		public BindingAction(Action<T1, T2> action)
		{
			this.action = action;
		}

		public override object Invoke(object[] args)
		{
			Bind()(args[0], args[1]);
			return null;
		}

		private Action<object, object> Bind()
		{
			return new Action<object, object>(
				(a, b) => { action(
					(T1)Convert.ChangeType(a, typeof(T1)),
					(T2)Convert.ChangeType(b, typeof(T2))
				);}
			);
		}
	}

	public class BindingAction<T1, T2, T3> : Binding
	{
		private Action<T1, T2, T3> action;

		public BindingAction(Action<T1, T2, T3> action)
		{
			this.action = action;
		}

		public override object Invoke(object[] args)
		{
			Bind()(args[0], args[1], args[2]);
			return null;
		}

		private Action<object, object, object> Bind()
		{
			return new Action<object, object, object>(
				(a, b, c) => { action(
					(T1)Convert.ChangeType(a, typeof(T1)),
					(T2)Convert.ChangeType(b, typeof(T2)),
					(T3)Convert.ChangeType(c, typeof(T3))
				);}
			);
		}
	}

	public class BindingAction<T1, T2, T3, T4> : Binding
	{
		private Action<T1, T2, T3, T4> action;

		public BindingAction(Action<T1, T2, T3, T4> action)
		{
			this.action = action;
		}

		public override object Invoke(object[] args)
		{
			Bind()(args[0], args[1], args[2], args[3]);
			return null;
		}

		private Action<object, object, object, object> Bind()
		{
			return new Action<object, object, object, object>(
				(a, b, c, d) => { action(
					(T1)Convert.ChangeType(a, typeof(T1)),
					(T2)Convert.ChangeType(b, typeof(T2)),
					(T3)Convert.ChangeType(c, typeof(T3)),
					(T4)Convert.ChangeType(d, typeof(T4))
				);}
			);
		}
	}

	public class BindingFunc<TResult> : Binding
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

	public class BindingFunc<T1, TResult> : Binding
	{
		private Func<T1, TResult> func;

		public BindingFunc(Func<T1, TResult> func)
		{
			this.func = func;
		}

		public override object Invoke(object[] args)
		{
			return Bind()(args[0]);
		}

		private Func<object, object> Bind()
		{
			return new Func<object, object>(
				(a) => { return func(
					(T1)Convert.ChangeType(a, typeof(T1))
				);}
			);
		}
	}

	public class BindingFunc<T1, T2, TResult> : Binding
	{
		private Func<T1, T2, TResult> func;

		public BindingFunc(Func<T1, T2, TResult> func)
		{
			this.func = func;
		}

		public override object Invoke(object[] args)
		{
			return Bind()(args[0], args[1]);
		}

		private Func<object, object, object> Bind()
		{
			return new Func<object, object, object>(
				(a, b) => { return func(
					(T1)Convert.ChangeType(a, typeof(T1)),
					(T2)Convert.ChangeType(b, typeof(T2))
				);}
			);
		}
	}

	public class BindingFunc<T1, T2, T3, TResult> : Binding
	{
		private Func<T1, T2, T3, TResult> func;

		public BindingFunc(Func<T1, T2, T3, TResult> func)
		{
			this.func = func;
		}

		public override object Invoke(object[] args)
		{
			return Bind()(args[0], args[1], args[2]);
		}

		private Func<object, object, object, object> Bind()
		{
			return new Func<object, object, object, object>(
				(a, b, c) => { return func(
					(T1)Convert.ChangeType(a, typeof(T1)),
					(T2)Convert.ChangeType(b, typeof(T2)),
					(T3)Convert.ChangeType(c, typeof(T3))
				);}
			);
		}
	}

	public class BindingFunc<T1, T2, T3, T4, TResult> : Binding
	{
		private Func<T1, T2, T3, T4, TResult> func;

		public BindingFunc(Func<T1, T2, T3, T4, TResult> func)
		{
			this.func = func;
		}

		public override object Invoke(object[] args)
		{
			return Bind()(args[0], args[1], args[2], args[3]);
		}

		private Func<object, object, object, object, object> Bind()
		{
			return new Func<object, object, object, object, object>(
				(a, b, c, d) => { return func(
					(T1)Convert.ChangeType(a, typeof(T1)),
					(T2)Convert.ChangeType(b, typeof(T2)),
					(T3)Convert.ChangeType(c, typeof(T3)),
					(T4)Convert.ChangeType(d, typeof(T4))
				);}
			);
		}
	}
}
