using System;
using System.Collections.Generic;
using T = System.Object;

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
			Bind(name, action);
		}

		/// <summary>
		/// Bind a <see cref="Action{T}"/> to the Rumor metatable so it can be
		/// called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="action">The <see cref="Action{T}"/> to bind.</param>
		public static void Bind<T1>(string name, Action<T1> action)
		{
			Bind(name, action);
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
			Bind(name, action);
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
			Bind(name, action);
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
			Bind(name, action);
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
			Bind(name, func);
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
			Bind(name, func);
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
			Bind(name, func);
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
			Bind(name, func);
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
			Bind(name, func);
		}

		private static void Bind(string name, object binding)
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

			var binding = bindings[name];
			var type = binding.GetType();

			if (typeof(Action).IsAssignableFrom(type)) {
				if (p.Length != 0) {
					throw new ArgumentException(string.Format(
						"Expected 0 parameters for {0}, but got {1} instead",
						name, p.Length));
				}
				((Action)binding).Invoke();
				return null;
			}
			if (typeof(Action<T>).IsAssignableFrom(type)) {
				if (p.Length != 1) {
					throw new ArgumentException(string.Format(
						"Expected 1 parameter for {0}, but got {1} instead",
						name, p.Length));
				}
				((Action<T>)binding).Invoke(p[0]);
				return null;
			}
			if (typeof(Action<T, T>).IsAssignableFrom(type)) {
				if (p.Length != 2) {
					throw new ArgumentException(string.Format(
						"Expected 2 parameters for {0}, but got {1} instead",
						name, p.Length));
				}
				((Action<T, T>)binding).Invoke(p[0], p[1]);
				return null;
			}
			if (typeof(Action<T, T, T>).IsAssignableFrom(type)) {
				if (p.Length != 3) {
					throw new ArgumentException(string.Format(
						"Expected 3 parameters for {0}, but got {1} instead",
						name, p.Length));
				}
				((Action<T, T, T>)binding).Invoke(p[0], p[1], p[2]);
				return null;
			}
			if (typeof(Action<T, T, T, T>).IsAssignableFrom(type)) {
				if (p.Length != 4) {
					throw new ArgumentException(string.Format(
						"Expected 4 parameters for {0}, but got {1} instead",
						name, p.Length));
				}
				((Action<T, T, T, T>)binding).Invoke(p[0], p[1], p[2], p[3]);
				return null;
			}

			if (typeof(Func<T>).IsAssignableFrom(type)) {
				if (p.Length != 0) {
					throw new ArgumentException(string.Format(
						"Expected 0 parameters for {0}, but got {1} instead",
						name, p.Length));
				}
				((Func<T>)binding).Invoke();
				return null;
			}
			if (typeof(Func<T, T>).IsAssignableFrom(type)) {
				if (p.Length != 1) {
					throw new ArgumentException(string.Format(
						"Expected 1 parameter for {0}, but got {1} instead",
						name, p.Length));
				}
				return ((Func<T, T>)binding).Invoke(p[0]);
			}
			if (typeof(Func<T, T, T>).IsAssignableFrom(type)) {
				if (p.Length != 2) {
					throw new ArgumentException(string.Format(
						"Expected 2 parameters for {0}, but got {1} instead",
						name, p.Length));
				}
				return ((Func<T, T, T>)binding).Invoke(p[0], p[1]);
			}
			if (typeof(Func<T, T, T, T>).IsAssignableFrom(type)) {
				if (p.Length != 3) {
					throw new ArgumentException(string.Format(
						"Expected 3 parameters for {0}, but got {1} instead",
						name, p.Length));
				}
				return ((Func<T, T, T, T>)binding).Invoke(p[0], p[1], p[2]);
			}
			if (typeof(Func<T, T, T, T, T>).IsAssignableFrom(type)) {
				if (p.Length != 4) {
					throw new ArgumentException(string.Format(
						"Expected 4 parameters for {0}, but got {1} instead",
						name, p.Length));
				}
				return ((Func<T, T, T, T, T>)binding)
					.Invoke(p[0], p[1], p[2], p[3]);
			}

			throw new InvalidOperationException(string.Format(
				"A binding exists for {0}, but no legal cast was found!",
				name));
		}
	}
}
