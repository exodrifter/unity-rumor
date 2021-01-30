namespace Exodrifter.Rumor.Engine
{
	public abstract class Expression
	{
		public abstract Value Evaluate(RumorScope scope);

		public abstract Expression Simplify();

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static bool operator ==(Expression l, Expression r)
		{
			if (ReferenceEquals(l, r))
			{
				return true;
			}
			if (l as object == null || r as object == null)
			{
				return false;
			}
			return l.Equals(r);
		}

		public static bool operator !=(Expression l, Expression r)
		{
			return !(l == r);
		}
	}
}
