using Exodrifter.Rumor.Expressions;
using Exodrifter.Rumor.Util;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	/// <summary>
	/// Keeps track of declared variables and the default speaker.
	/// </summary>
	[Serializable]
	public class Scope : ISerializable
	{
		#region Properties

		/// <summary>
		/// The declared variables.
		/// </summary>
		private readonly Dictionary<string, Value> vars;

		/// <summary>
		/// The default speaker.
		/// </summary>
		public object DefaultSpeaker { get; set; }

		#endregion

		#region Events

		/// <summary>
		/// An event that is called when all declared variables are removed.
		/// </summary>
		public event Action OnClear;

		/// <summary>
		/// An event that is called when a variable is set.
		/// </summary>
		public event Action<string, Value> OnSet;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of this class.
		/// </summary>
		public Scope()
		{
			vars = new Dictionary<string, Value>();
		}

		#endregion

		#region Variables

		/// <summary>
		/// Removes all declared variables.
		/// </summary>
		public void ClearVars()
		{
			vars.Clear();

			if (OnClear != null)
			{
				OnClear();
			}
		}

		/// <summary>
		/// Returns the value of the specified variable.
		/// </summary>
		/// <returns>The value of the variable.</returns>
		/// <param name="name">The name of the variable to get.</param>
		public Value GetVar(string name)
		{
			if (vars.ContainsKey(name))
			{
				return vars[name];
			}
			return null;
		}

		/// <summary>
		/// Returns true if the specified variable is declared.
		/// </summary>
		/// <param name="name">The name of the variable to find.</param>
		/// <returns>True if the variable is declared.</returns>
		public bool HasVar(string name)
		{
			return vars.ContainsKey(name);
		}

		/// <summary>
		/// Sets the value of one variable to a bool.
		/// </summary>
		/// <param name="name">The name of the variable to set.</param>
		/// <param name="value">The value of the variable to use.</param>
		public void SetVar(string name, bool @bool)
		{
			vars[name] = new BoolValue(@bool);

			if (OnSet != null)
			{
				OnSet(name, vars[name]);
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

			if (OnSet != null)
			{
				OnSet(name, vars[name]);
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

			if (OnSet != null)
			{
				OnSet(name, vars[name]);
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

			if (OnSet != null)
			{
				OnSet(name, vars[name]);
			}
		}

		/// <summary>
		/// Sets the value of one variable.
		/// </summary>
		/// <param name="name">The name of the variable to set.</param>
		/// <param name="value">The value of the variable to use.</param>
		public void SetVar(string name, object @object)
		{
			if (@object is bool)
			{
				vars[name] = new BoolValue((bool)@object);
			}
			else if (@object is int)
			{
				vars[name] = new IntValue((int)@object);
			}
			else if (@object is float)
			{
				vars[name] = new FloatValue((float)@object);
			}
			else if (@object is string)
			{
				vars[name] = new StringValue((string)@object);
			}
			else
			{
				vars[name] = new ObjectValue(@object);
			}

			if (OnSet != null)
			{
				OnSet(name, vars[name]);
			}
		}

		/// <summary>
		/// Sets the value of one variable.
		/// </summary>
		/// <param name="name">The name of the variable to set.</param>
		/// <param name="value">The value of the variable to use.</param>
		public void SetVar(string name, Value value)
		{
			vars[name] = value;

			if (OnSet != null)
			{
				OnSet(name, vars[name]);
			}
		}

		#endregion

		#region Serialization

		private List<string> tempKeys;
		private List<Value> tempValues;

		public Scope(SerializationInfo info, StreamingContext context)
		{
			vars = new Dictionary<string, Value>();
			tempKeys = info.GetValue<List<string>>("keys");
			tempValues = info.GetValue<List<Value>>("values");

			DefaultSpeaker = info.GetValue<object>("defaultSpeaker");
		}

		[OnDeserialized]
		void OnDeserialized()
		{
			for (int i = 0; i < tempKeys.Count; ++i)
			{
				vars.Add(tempKeys[i], tempValues[i]);
			}
			tempKeys = null;
			tempValues = null;
		}

		void ISerializable.GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			var keys = new List<string>(vars.Count);
			var values = new List<Value>(vars.Count);

			foreach (var kvp in vars)
			{
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