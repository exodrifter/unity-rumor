using System;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine
{
	/// <summary>
	/// Keeps track of bound functions.
	/// </summary>
	[Serializable]
	public class RumorBindings
	{
		/// <summary>
		/// Stores functions, methods, or constructors.
		/// </summary>
		private readonly Dictionary<string, Binding> bindings
			= new Dictionary<string, Binding>();

		/// <summary>
		/// Bind a <see cref="Action"/> to the Rumor metatable so it can be
		/// called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="action">The action to bind.</param>
		public void Bind(string name, Action action)
		{
			SetBinding(BindingType.Action, name, 0,
				new BindingAction(action)
			);
		}

		/// <summary>
		/// Bind a <see cref="Action{T}"/> to the Rumor metatable so it can be
		/// called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="action">The <see cref="Action{T}"/> to bind.</param>
		public void Bind<T1>(string name, Action<T1> action)
		{
			SetBinding(BindingType.Action, name, 1,
				new BindingAction<T1>(action)
			);
		}

		/// <summary>
		/// Bind a <see cref="Action{T1, T2}"/> to the Rumor metatable so it
		/// can be called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="action">
		/// The <see cref="Action{T1, T2}"/> to bind.
		/// </param>
		public void Bind<T1, T2>(string name, Action<T1, T2> action)
		{
			SetBinding(BindingType.Action, name, 2,
				new BindingAction<T1, T2>(action)
			);
		}

		/// <summary>
		/// Bind a <see cref="Action{T1, T2, T3}"/> to the Rumor metatable so
		/// it can be called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="action">
		/// The <see cref="Action{T1, T2, T3}"/> to bind.
		/// </param>
		public void Bind<T1, T2, T3>
			(string name, Action<T1, T2, T3> action)
		{
			SetBinding(BindingType.Action, name, 3,
				new BindingAction<T1, T2, T3>(action)
			);
		}

		/// <summary>
		/// Bind a <see cref="Action{T1, T2, T3, T4}"/> to the Rumor metatable
		/// so it can be called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="action">
		/// The <see cref="Action{T1, T2, T3, T4}"/> to bind.
		/// </param>
		public void Bind<T1, T2, T3, T4>
			(string name, Action<T1, T2, T3, T4> action)
		{
			SetBinding(BindingType.Action, name, 4,
				new BindingAction<T1, T2, T3, T4>(action)
			);
		}

		/// <summary>
		/// Bind a <see cref="Func{TResult}"/> to the Rumor metatable so it can
		/// be called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="func">
		/// The <see cref="Func{TResult}"/> to bind.
		/// </param>
		public void Bind<TResult>(string name, Func<TResult> func)
		{
			SetBinding(BindingType.Function, name, 0,
				new BindingFunc<TResult>(func)
			);
		}

		/// <summary>
		/// Bind a <see cref="Func{T1, TResult}"/> to the Rumor
		/// metatable so it can be called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="func">
		/// The <see cref="Func{T1, TResult}"/> to bind.
		/// </param>
		public void Bind<T1, TResult>(string name, Func<T1, TResult> func)
		{
			SetBinding(BindingType.Function, name, 1,
				new BindingFunc<T1, TResult>(func)
			);
		}

		/// <summary>
		/// Bind a <see cref="Func{T1, T2, TResult}"/> to the Rumor
		/// metatable so it can be called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="func">
		/// The <see cref="Func{T1, T2, TResult}"/> to bind.
		/// </param>
		public void Bind<T1, T2, TResult>
			(string name, Func<T1, T2, TResult> func)
		{
			SetBinding(BindingType.Function, name, 2,
				new BindingFunc<T1, T2, TResult>(func)
			);
		}

		/// <summary>
		/// Bind a <see cref="Func{T1, T2, T3, TResult}"/> to the Rumor
		/// metatable so it can be called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="func">
		/// The <see cref="Func{T1, T2, T3, TResult}"/> to bind.
		/// </param>
		public void Bind<T1, T2, T3, TResult>
			(string name, Func<T1, T2, T3, TResult> func)
		{
			SetBinding(BindingType.Function, name, 3,
				new BindingFunc<T1, T2, T3, TResult>(func)
			);
		}

		/// <summary>
		/// Bind a <see cref="Func{T1, T2, T3, T4, TResult}"/> to the Rumor
		/// metatable so it can be called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="func">
		/// The <see cref="Func{T1, T2, T3, T4, TResult}"/> to bind.
		/// </param>
		public void Bind<T1, T2, T3, T4, TResult>
			(string name, Func<T1, T2, T3, T4, TResult> func)
		{
			SetBinding(BindingType.Function, name, 4,
				new BindingFunc<T1, T2, T3, T4, TResult>(func)
			);
		}

		/// <summary>
		/// Add a binding.
		/// </summary>
		/// <param name="type">The type of the binding.</param>
		/// <param name="name">The name associate with the binding.</param>
		/// <param name="paramCount">
		/// The number of parameters in the binding.
		/// </param>
		/// <param name="binding">The binding to use.</param>
		private void SetBinding(BindingType type, string name, int paramCount, Binding binding)
		{
			if (binding == null)
			{
				throw new ArgumentNullException();
			}

			var mungedName = BindingUtil.MungeName(type, name, paramCount);
			bindings.Add(mungedName, binding);
		}

		/// <summary>
		/// Call a binding with the specified name and arguments
		/// </summary>
		/// <param name="type">The type of the binding to call.</param>
		/// <param name="name">The name of the binding to call.</param>
		/// <param name="p">The arguments to pass to the binding.</param>
		/// <returns>The result of calling the binding.</returns>
		public object CallBinding(BindingType type, string name, params object[] p)
		{
			var mungedName = BindingUtil.MungeName(type, name, p.Length);
			if (!bindings.ContainsKey(mungedName))
			{
				var paramStr = p.Length == 1 ? "parameter" : "parameters";

				throw new InvalidOperationException(string.Format(
					"No binding of the name \"{0}\" with {1} {2} exists!",
					name, p.Length, paramStr));
			}

			return bindings[mungedName].Invoke(p);
		}

		/// <summary>
		/// Remove a binding.
		/// </summary>
		/// <param name="name">The name of the binding to remove.</param>
		/// <param name="paramCount">
		/// The number of parameters of the binding to remove.
		/// </param>
		public bool RemoveBinding(BindingType type, string name, int paramCount)
		{
			var mungedName = BindingUtil.MungeName(type, name, paramCount);
			return bindings.Remove(mungedName);
		}

		/// <summary>
		/// Removes all of the bindings.
		/// </summary>
		public void ClearBindings()
		{
			bindings.Clear();
		}
	}
}
