<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>JsonFormatting = Newtonsoft.Json.Formatting</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

//
// This script adds a new shader effect to all projects.
//

void Main()
{
	// Configurable options
	var effectName = "foo";

	// Environment constants
	var platforms = new[] { "Android", "iOS" };

	// Logic
	CreateFileTemplate(effectName);
	AddToSolution(effectName);
	foreach (var platform in platforms)
		AddToContentProject(effectName, platform);

	// Success
	Console.WriteLine("Effect '{0}' added successfully.", effectName);
}

#region Logic

private void CreateFileTemplate(string effectName)
{
	var srcPath = GetPath("utils", "EffectTemplate.fx");
	var destPath = GetPath("src", "Resources", "Effects", effectName + ".fx");

	File.Copy(srcPath, destPath);
}

private void AddToSolution(string effectName)
{
	var solutionPath = GetPath("Corund.sln");
	var content = File.ReadAllLines(solutionPath)
                      .Select((x, idx) => new { Line = x, Index = idx })
                      .ToList();

	var folderStart = content.First(x => x.Line.Contains("48933D54-7995-4B9E-AD06-E824680CD90E")).Index;
	var folderEnd = content.First(x => x.Line == "EndProject" && x.Index > folderStart).Index;

	var line = string.Format(
		"\t\t{0} = {0}",
		$@"src\Resources\Effects\{effectName}.fx"
	);

	content.Insert(folderEnd - 1, new { Line = line, Index = 0 });

	File.WriteAllLines(solutionPath, content.Select(x => x.Line));
}

private void AddToContentProject(string effectName, string platform)
{
	var projectPath = GetPath("src", "Corund.Platform." + platform, "Content", "Content.mgcb");
	var content = File.ReadAllLines(projectPath);

	var newContent = new[]
	{
		$"#begin ../../Resources/Effects/{effectName}.fx",
		$"/importer:EffectImporter",
		$"/processor:EffectProcessor",
		$"/processorParam:DebugMode=Auto",
		$"/build:../../Resources/Effects/{effectName}.fx",
		$""
	};
	
	File.WriteAllLines(projectPath, content.Concat(newContent));
}

#endregion

#region Utilities

private string GetPath(params string[] parts)
{
	var basePath = Path.GetDirectoryName(Util.CurrentQueryPath);
	var allParts = new[] { basePath, ".." }.Concat(parts);
	return Path.GetFullPath(Path.Combine(allParts.ToArray()));
}

#endregion