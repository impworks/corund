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
	var effectName = "";

	// Environment constants
	var platforms = new[] { "Android", "IOS", "WP8" };

	// Logic
	Validate(effectName);
	AddToSolution(effectName);
	foreach (var platform in platforms)
	{
		AddToContentProject(effectName, platform);
		AddToPlatformProject(effectName, platform);
	}

	// Success
	Console.WriteLine("Effect '{0}' added successfully.", effectName);
}

#region Logic

private void Validate(string effectName)
{
	var fullPath = GetPath("Corund.Effects", effectName + ".fx");

	if(!File.Exists(fullPath))
		throw new ArgumentException($"Path does not exist:\n'{fullPath}'");
}

private void AddToSolution(string effectName)
{
	var solutionPath = GetPath("Corund.sln");
	var content = File.ReadAllLines(solutionPath)
                      .Select((x, idx) => new { Line = x, Index = idx })
                      .ToList();

	var folderStart = content.First(x => x.Line.Contains("CD6098D5-0F6C-4DDD-A9A6-EDF3B6F065E8")).Index;
	var folderEnd = content.First(x => x.Line.Contains("EndProject") && x.Index > folderStart).Index;

	var line = string.Format(
		"{0}{1} = {1}",
		new string(' ', 8), // indent,
		$@"Corund.Effects\{effectName}.fx"
	);

	content.Insert(folderEnd - 2, new { Line = line, Index = 0 });

	File.WriteAllLines(solutionPath, content.Select(x => x.Line));
}

private void AddToContentProject(string effectName, string platform)
{

}

private void AddToPlatformProject(string effectName, string platform)
{

}

#endregion

#region Utilities

private string GetPath(params string[] parts)
{
	var basePath = Path.GetDirectoryName(Util.CurrentQueryPath);
	var allParts = new[] { basePath, ".." }.Concat(parts);
	return Path.Combine(allParts.ToArray());
}

#endregion