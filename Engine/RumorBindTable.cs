using System;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine
{
	/// <summary>
	/// Stores references to binded methods and objects.
	/// </summary>
	public sealed partial class Rumor
	{
		/// <summary>
		/// Stores functions, methods, or constructors.
		/// </summary>
		private static Dictionary<string, object> bindings;

		/// <summary>
		/// Bind a <see cref="Action"/> to the Rumor metatable so it can be
		/// called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="action">The action to bind.</param>
		public static void Bind(string name, Action action)
		{
			AddBinding(name, action);
		}

		/// <summary>
		/// Bind a <see cref="Action{T}"/> to the Rumor metatable so it can be
		/// called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="action">The <see cref="Action{T}"/> to bind.</param>
		public static void Bind<T1>(string name, Action<T1> action)
		{
			AddBinding(name, action);
		}

		/// <summary>
		/// Bind a <see cref="Action{T1, T2}"/> to the Rumor metatable so it
		/// can be called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="action">
		/// The <see cref="Action{T1, T2}"/> to bind.
		/// </param>
		public static void Bind<T1, T2>(string name, Action<T1, T2> action)
		{
			AddBinding(name, action);
		}

		/// <summary>
		/// Bind a <see cref="Action{T1, T2, T3}"/> to the Rumor metatable so
		/// it can be called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="action">
		/// The <see cref="Action{T1, T2, T3}"/> to bind.
		/// </param>
		public static void Bind<T1, T2, T3>
			(string name, Action<T1, T2, T3> action)
		{
			AddBinding(name, action);
		}

		/// <summary>
		/// Bind a <see cref="Action{T1, T2, T3, T4}"/> to the Rumor metatable
		/// so it can be called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="action">
		/// The <see cref="Action{T1, T2, T3, T4}"/> to bind.
		/// </param>
		public static void Bind<T1, T2, T3, T4>
			(string name, Action<T1, T2, T3, T4> action)
		{
			AddBinding(name, action);
		}

		/// <summary>
		/// Bind a <see cref="Func{T1}"/> to the Rumor
		/// metatable so it can be called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="func">
		/// The <see cref="Func{T1}"/> to bind.
		/// </param>
		public static void Bind<T1>(string name, Func<T1> func)
		{
			AddBinding(name, func);
		}

		/// <summary>
		/// Bind a <see cref="Func{T1, TResult}"/> to the Rumor
		/// metatable so it can be called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="func">
		/// The <see cref="Func{T1, TResult}"/> to bind.
		/// </param>
		public static void Bind<T1, T2>(string name, Func<T1, T2> func)
		{
			AddBinding(name, func);
		}

		/// <summary>
		/// Bind a <see cref="Func{T1, T2, TResult}"/> to the Rumor
		/// metatable so it can be called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="func">
		/// The <see cref="Func{T1, T2, TResult}"/> to bind.
		/// </param>
		public static void Bind<T1, T2, T3>(string name, Func<T1, T2, T3> func)
		{
			AddBinding(name, func);
		}

		/// <summary>
		/// Bind a <see cref="Func{T1, T2, T3, TResult}"/> to the Rumor
		/// metatable so it can be called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="func">
		/// The <see cref="Func{T1, T2, T3, TResult}"/> to bind.
		/// </param>
		public static void Bind<T1, T2, T3, T4>
			(string name, Func<T1, T2, T3, T4> func)
		{
			AddBinding(name, func);
		}

		/// <summary>
		/// Bind a <see cref="Func{T1, T2, T3, T4, TResult}"/> to the Rumor
		/// metatable so it can be called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="func">
		/// The <see cref="Func{T1, T2, T3, T4, TResult}"/> to bind.
		/// </param>
		public static void Bind<T1, T2, T3, T4, T5>
			(string name, Func<T1, T2, T3, T4, T5> func)
		{
			AddBinding(name, func);
		}

		private static void AddBinding(string name, object binding)
		{
			if (binding == null) {
				throw new ArgumentNullException();
			}

			bindings = bindings ?? new Dictionary<string, object>();
			bindings.Add(name, binding);
		}

		public static object CallBinding(string name, params object[] p)
		{
			bindings = bindings ?? new Dictionary<string, object>();
			if (!bindings.ContainsKey(name)) {
				throw new InvalidOperationException(string.Format(
					"No binding of the name \"{0}\" exists!",
					name));
			}

			return ((Delegate)bindings[name]).DynamicInvoke(p);
		}
	}
}
