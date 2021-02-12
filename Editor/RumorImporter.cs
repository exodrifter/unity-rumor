using UnityEngine;
using UnityEditor.AssetImporters;
using System.IO;

/// <summary>
/// Allows the use of files with ".rumor" extensions as a UnityAsset.
/// </summary>
namespace Exodrifter.Rumor.Editor
{
	[ScriptedImporter(1, "rumor")]
	public class RumorImporter : ScriptedImporter
	{
		private RumorImporter() { }

		public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext ctx)
		{
			var str = File.ReadAllText(ctx.assetPath);

			var textAsset = new TextAsset(str);
			ctx.AddObjectToAsset("text", textAsset);
			ctx.SetMainObject(textAsset);
		}
	}
}
