using System;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine
{
	/// <summary>
	/// Keeps track of declared variables and the default speaker.
	/// </summary>
	[Serializable]
	public class Bindings
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
			AddBinding(name, 0, new BindingAction(action));
		}

		/// <summary>
		/// Bind a <see cref="Action{T}"/> to the Rumor metatable so it can be
		/// called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="action">The <see cref="Action{T}"/> to bind.</param>
		public void Bind<T1>(string name, Action<T1> action)
		{
			AddBinding(name, 1, new BindingAction<T1>(action));
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
			AddBinding(name, 2, new BindingAction<T1, T2>(action));
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
			AddBinding(name, 3, new BindingAction<T1, T2, T3>(action));
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
			AddBinding(name, 4, new BindingAction<T1, T2, T3, T4>(action));
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
			AddBinding(name, 0, new BindingFunc<TResult>(func));
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
			AddBinding(name, 1, new BindingFunc<T1, TResult>(func));
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
			AddBinding(name, 2, new BindingFunc<T1, T2, TResult>(func));
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
			AddBinding(name, 3, new BindingFunc<T1, T2, T3, TResult>(func));
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
			AddBinding(name, 4, new BindingFunc<T1, T2, T3, T4, TResult>(func));
		}

		/// <summary>
		/// Add a binding.
		/// </summary>
		/// <param name="name">The name associate with the binding.</param>
		/// <param name="paramCount">
		/// The number of parameters in the binding.
		/// </param>
		/// <param name="binding">The binding to use.</param>
		private void AddBinding(string name, int paramCount, Binding binding)
		{
			if (binding == null)
			{
				throw new ArgumentNullException();
			}

			var mungedName = MungeName(name, paramCount);
			if (bindings.ContainsKey(mungedName))
			{
				var paramStr = paramCount == 1 ? "parameter" : "parameters";

				throw new InvalidOperationException(string.Format(
					"A binding \"{0}\" with {1} {2} is already in use!",
					name, paramCount, paramStr));
			}

			bindings.Add(mungedName, binding);
		}

		/// <summary>
		/// Call a binding with the specified name and arguments
		/// </summary>
		/// <param name="name">The name of the binding to call.</param>
		/// <param name="p">The arguments to pass to the binding.</param>
		/// <returns>The result of calling the binding.</returns>
		public object CallBinding(string name, params object[] p)
		{
			var mungedName = MungeName(name, p.Length);
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
		public bool RemoveBinding(string name, int paramCount)
		{
			var mungedName = MungeName(name, paramCount);
			return bindings.Remove(mungedName);
		}

		/// <summary>
		/// Removes all of the bindings.
		/// </summary>
		public void ClearBindings()
		{
			bindings.Clear();
		}

		#region Util

		/// <summary>
		/// Munges a binding name.
		/// </summary>
		/// <param name="name">The binding name to munge.</param>
		/// <param name="paramCount">
		/// The number of parameters in the binding.
		/// </param>
		/// <returns>The munged name</returns>
		private string MungeName(string name, int paramCount)
		{
			return string.Format("{0}@{1}", name, paramCount);
		}

		#endregion
	}
}
