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