using System;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	/// <summary>
	/// This type may be returned from a node to indicate some condition that
	/// should happen before execution may continue.
	/// </summary>
	[Serializable]
	public abstract class Yield : ISerializable
	{
		public bool Finished { get; protected set; }
		public double Elapsed { get; protected set; }

		public Yield()
		{
			Finished = false;
			Elapsed = 0;
		}

		public virtual void Advance() { }

		public virtual void Update(Rumor rumor, double delta)
		{
			Elapsed += delta;
		}

		public virtual void Choose() { }

		#region Serialization

		public Yield(SerializationInfo info, StreamingContext context)
		{
			Finished = info.GetValue<bool>("finished");
			Elapsed = info.GetValue<double>("elapsed");
		}

		public virtual void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			info.AddValue<bool>("finished", Finished);
			info.AddValue<double>("elapsed", Elapsed);
		}

		#endregion
	}

	/// <summary>
	/// Yield execution until Rumor is advanced.
	/// </summary>
	[Serializable]
	public class ForAdvance : Yield, ISerializable
	{
		public ForAdvance() { }

		public override void Advance()
		{
			Finished = true;
		}

		#region Serialization

		public ForAdvance(SerializationInfo info, StreamingContext context)
			: base(info, context) { }

		#endregion
	}

	/// <summary>
	/// Yield execution until Rumor is advanced.
	/// </summary>
	[Serializable]
	public class ForSeconds : Yield, ISerializable
	{
		private readonly double time;

		public ForSeconds(double seconds)
		{
			time = seconds;
		}

		public override void Update(Rumor rumor, double delta)
		{
			base.Update(rumor, delta);
			Finished = Elapsed >= time;
		}

		#region Serialization

		public ForSeconds(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			time = info.GetValue<double>("time");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue<double>("time", time);
		}

		#endregion
	}

	/// <summary>
	/// Yield execution until Rumor is advanced.
	/// </summary>
	[Serializable]
	public class ForChoose : Yield, ISerializable
	{
		public double? Timeout { get; private set; }
		private readonly string label;

		public ForChoose(double? timeout, string label)
		{
			Timeout = timeout;
			this.label = label;
		}

		public override void Update(Rumor rumor, double delta)
		{
			base.Update(rumor, delta);

			if (Timeout.HasValue)
			{
				Finished = Elapsed >= Timeout.Value;
				if (Finished)
				{
					rumor.Jump(label);
					rumor.State.ClearChoices();
				}
			}
		}

		public override void Choose()
		{
			Finished = true;
		}

		#region Serialization

		public ForChoose(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			Timeout = info.GetValue<double?>("timeout");
			label = info.GetValue<string>("label");
		}

		public override void GetObjectData
			(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue<double?>("timeout", Timeout);
			info.AddValue<string>("label", label);
		}

		#endregion
	}
}
