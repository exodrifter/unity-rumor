namespace Exodrifter.Rumor.Engine
{
	public abstract class Expression<T> where T : Value
	{
		public abstract T Evaluate();

		public abstract Expression<T> Simplify();

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static bool operator ==(Expression<T> l, Expression<T> r)
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

		public static bool operator !=(Expression<T> l, Expression<T> r)
		{
			return !(l == r);
		}
	}
}
