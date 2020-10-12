namespace Exodrifter.Rumor.Engine
{
	public class SayNode
	{
		public string Speaker { get; }
		public string Dialog { get; }

		public SayNode(string speaker, string dialog)
		{
			Speaker = speaker;
			Dialog = dialog;
		}
	}
}
