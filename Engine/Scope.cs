using Exodrifter.Rumor.Expressions;
using Exodrifter.Rumor.Util;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	/// <summary>
	/// A scope which keeps track of declared variables and functions. While
	/// variables are serialized, bindings are NOT serialized.
	/// </summary>
	[Serializable]
	public class Scope : ISerializable
	{
		/// <summary>
		/// Stores functions, methods, or constructors.
		/// </summary>
		private Dictionary<string, object> bindings;

		/// <summary>
		/// The variables in this scope.
		/// </summary>
		private readonly Dictionary<string, Value> vars;

		/// <summary>
		/// The default narrator for nodes to use.
		/// </summary>
		public object DefaultSpeaker { get; set; }

		/// <summary>
		/// An event that is called when a variable is set.
		/// </summary>
		public event Action OnVarSet;

		/// <summary>
		/// Creates a new scope.
		/// </summary>
		/// <param name="parent">
		/// The parent scope or null if there is none.
		/// </param>
		public Scope()
		{
			vars = new Dictionary<string, Value>();
		}

		#region Variables

		/// <summary>
		/// Sets the value of one variable to a bool.
		/// </summary>
		/// <param name="name">The name of the variable to set.</param>
		/// <param name="value">The value of the variable to use.</param>
		public void SetVar(string name, bool @bool)
		{
			vars[name] = new BoolValue(@bool);

			if (OnVarSet != null) {
				OnVarSet();
			}
		}

		/// <summary>
		/// Sets the value of one variable to an int.
		/// </summary>
		/// <param name="name">The name of the variable to set.</param>
		/// <param name="value">The value of the variable to use.</param>
		public void SetVar(string name, int @int)
		{
			vars[name] = new IntValue(@int);

			if (OnVarSet != null) {
				OnVarSet();
			}
		}

		/// <summary>
		/// Sets the value of one variable to a float.
		/// </summary>
		/// <param name="name">The name of the variable to set.</param>
		/// <param name="value">The value of the variable to use.</param>
		public void SetVar(string name, float @float)
		{
			vars[name] = new FloatValue(@float);

			if (OnVarSet != null) {
				OnVarSet();
			}
		}

		/// <summary>
		/// Sets the value of one variable to a string.
		/// </summary>
		/// <param name="name">The name of the variable to set.</param>
		/// <param name="value">The value of the variable to use.</param>
		public void SetVar(string name, string @string)
		{
			vars[name] = new StringValue(@string);

			if (OnVarSet != null) {
				OnVarSet();
			}
		}

		/// <summary>
		/// Sets the value of one variable to another.
		/// </summary>
		/// <param name="name">The name of the variable to set.</param>
		/// <param name="value">The value of the variable to use.</param>
		public void SetVar(string name, object @object)
		{
			vars[name] = new ObjectValue(@object);

			if (OnVarSet != null) {
				OnVarSet();
			}
		}

		/// <summary>
		/// Sets the value of one variable to another.
		/// </summary>
		/// <param name="name">The name of the variable to set.</param>
		/// <param name="value">The value of the variable to use.</param>
		public void SetVar(string name, Value value)
		{
			vars[name] = value;

			if (OnVarSet != null) {
				OnVarSet();
			}
		}

		/// <summary>
		/// Returns the value of the specified variable.
		/// </summary>
		/// <returns>The value of the variable.</returns>
		/// <param name="name">The name of the variable to get.</param>
		public Value GetVar(string name)
		{
			if (vars.ContainsKey(name)) {
				return vars[name];
			}
			return null;
		}

		/// <summary>
		/// Returns true if this scope or a parent scope has the specified
		/// variable.
		/// </summary>
		/// <param name="name">The name of the variable to find.</param>
		/// <returns>True if the variable is declared.</returns>
		public bool HasVar(string name)
		{
			return vars.ContainsKey(name);
		}

		/// <summary>
		/// Removes all of the variables from this scope.
		/// </summary>
		public void ClearVars()
		{
			vars.Clear();
		}

		#endregion

		#region Bindings

		/// <summary>
		/// Bind a <see cref="Action"/> to the Rumor metatable so it can be
		/// called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="action">The action to bind.</param>
		public void Bind(string name, Action action)
		{
			AddBinding(name, action);
		}

		/// <summary>
		/// Bind a <see cref="Action{T}"/> to the Rumor metatable so it can be
		/// called by scripts.
		/// </summary>
		/// <param name="name">The name to use for the binding.</param>
		/// <param name="action">The <see cref="Action{T}"/> to bind.</param>
		public void Bind<T1>(string name, Action<T1> action)
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
		public void Bind<T1, T2>(string name, Action<T1, T2> action)
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
		public void Bind<T1, T2, T3>
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
		public void Bind<T1, T2, T3, T4>
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
		public void Bind<T1>(string name, Func<T1> func)
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
		public void Bind<T1, T2>(string name, Func<T1, T2> func)
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
		public void Bind<T1, T2, T3>(string name, Func<T1, T2, T3> func)
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
		public void Bind<T1, T2, T3, T4>
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
		public void Bind<T1, T2, T3, T4, T5>
			(string name, Func<T1, T2, T3, T4, T5> func)
		{
			AddBinding(name, func);
		}

		/// <summary>
		/// Add a binding.
		/// </summary>
		/// <param name="name">The name associate with the binding.</param>
		/// <param name="binding">The binding to use.</param>
		private void AddBinding(string name, object binding)
		{
			if (binding == null) {
				throw new ArgumentNullException();
			}

			bindings = bindings ?? new Dictionary<string, object>();
			bindings.Add(name, binding);
		}

		/// <summary>
		/// Call a binding with the specified name and arguments
		/// </summary>
		/// <param name="name">The name of the binding to call.</param>
		/// <param name="p">The arguments to pass to the binding.</param>
		/// <returns>The result of calling the binding.</returns>
		public object CallBinding(string name, params object[] p)
		{
			bindings = bindings ?? new Dictionary<string, object>();
			if (!bindings.ContainsKey(name)) {
				throw new InvalidOperationException(string.Format(
					"No binding of the name \"{0}\" exists!",
					name));
			}

			return ((Delegate)bindings[name]).DynamicInvoke(p);
		}

		/// <summary>
		/// Remove a binding.
		/// </summary>
		/// <param name="name">The name of the binding to remove.</param>
		public void RemoveBinding(string name)
		{
			bindings.Remove(name);
		}

		/// <summary>
		/// Removes all of the bindings from this scope.
		/// </summary>
		public void ClearBindings()
		{
			bindings.Clear();
		}

		#endregion

		#region Serialization

		public Scope(SerializationInfo info, StreamingContext context)
		{
			var keys = info.GetValue<List<string>>("keys");
			var values = info.GetValue<List<Value>>("values");

			vars = new Dictionary<string, Value>();
			for (int i = 0; i < keys.Count; ++i) {
				vars.Add(keys[i], values[i]);
			}

			DefaultSpeaker = info.GetValue<object>("defaultSpeaker");
		}

		void ISerializable.GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			var keys = new List<string>(vars.Count);
			var values = new List<Value>(vars.Count);

			foreach (var kvp in vars) {
				keys.Add(kvp.Key);
				values.Add(kvp.Value);
			}

			info.AddValue<List<string>>("keys", keys);
			info.AddValue<List<Value>>("values", values);

			info.AddValue<object>("defaultSpeaker", DefaultSpeaker);
		}

		#endregion
	}
}