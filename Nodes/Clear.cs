using Exodrifter.Rumor.Engine;
using Exodrifter.Rumor.Util;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Nodes
{
	/// <summary>
	/// Exits the current block.
	/// </summary>
	[Serializable]
	public sealed class Clear : Node
	{
		private readonly ClearType type;
		public ClearType ClearType
		{
			get { return type; }
		}

		/// <summary>
		/// Creates a new Return node.
		/// </summary>
		public Clear(ClearType type)
		{
			this.type = type;
		}

		public override IEnumerator<RumorYield> Run(Engine.Rumor rumor)
		{
			switch(type)
			{
			case ClearType.ALL:
				rumor.State.Clear();
				break;
			case ClearType.CHOICES:
				rumor.State.ClearChoices();
				break;
			case ClearType.DIALOG:
				rumor.State.ClearDialog();
				break;
			}
			yield return null;
		}

		#region Serialization

		public Clear(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			type = info.GetValue<ClearType>("type");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue<ClearType>("type", type);
		}

		#endregion
	}
}
