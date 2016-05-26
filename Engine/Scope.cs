using System;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine
{
	/// <summary>
	/// A scope which keeps track of declared variables and functions.
	/// </summary>
	public class Scope
	{
		/// <summary>
		/// The parent scope.
		/// </summary>
		private Scope parent;

		/// <summary>
		/// The variables in this scope.
		/// </summary>
		private Dictionary<string, object> vars;

		/// <summary>
		/// Creates a new scope.
		/// </summary>
		/// <param name="parent">
		/// The parent scope or null if there is none.
		/// </param>
		public Scope(Scope parent = null)
		{
			this.parent = parent;
			vars = new Dictionary<string, object>();
		}

		/// <summary>
		/// Sets the value of one variable to another.
		/// </summary>
		/// <param name="name">The name of the variable to set.</param>
		/// <param name="value">The value of the variable to use.</param>
		public void SetVar(string name, object value)
		{
			if (vars.ContainsKey(name)) {
				vars[name] = value;
				return;
			}
			if (parent != null && parent.HasVar(name)) {
				parent.SetVar(name, value);
				return;
			}
			vars[name] = value;
		}

		/// <summary>
		/// Returns the value of the specified variable.
		/// </summary>
		/// <returns>The value of the variable.</returns>
		/// <param name="name">The name of the variable to get.</param>
		public object GetVar(string name)
		{
			if (vars.ContainsKey(name)) {
				return vars[name];
			}
			if (parent != null) {
				return parent.GetVar(name);
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
			if (vars.ContainsKey(name)) {
				return true;
			}
			if (parent != null) {
				return parent.HasVar(name);
			}
			return false;
		}

		/// <summary>
		/// Removes all of the variables from this scope.
		/// </summary>
		/// <param name="recursive">If true, clear parent scopes too.</param>
		public void Clear(bool recursive)
		{
			vars.Clear();
			if (recursive && parent != null) {
				parent.Clear(recursive);
			}
		}
	}
}