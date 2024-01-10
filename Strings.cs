#pragma warning disable CA1050 //Declare types in namespaces
public static partial class SafeFile
{
#pragma warning disable CA1707 //Identifiers should not contain underscores

	#region Fallbacks
	/// <summary> Defaults to the 'My Documents' folder. (<see cref="Environment.SpecialFolder.MyDocuments"/>) </summary>
	public static string FALLBACK_PATH { get; set; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}";
	/// <summary> Defaults to the 'Fallback.txt' file in the 'My Documents' folder. (<see cref="Environment.SpecialFolder.MyDocuments"/>) </summary>
	public static string FALLBACK_FILE { get; set; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\Fallback.txt";
	public static IEnumerable<string> IENUMFALLBACK_STR { get; set; } = new List<string>()
	{
		"This is fallback text",
		"Don't touch it",
		"Or do"
	};
	#endregion

	#region Error Messages
	private const string SAFEPATH_BLANK = "The given 'safepath' parameter was blank but not null, defaulted to the path in FALLBACK_FILE.";
	private const string SAFEPATH_FAIL = "The given 'safepath' parameter was inaccessible, defaulted to the path in FALLBACK_FILE.";

	private const string SAFECONTENT_EMPTY = "An empty IEnumerable was given in the 'safecontents' parameter, defaulted to the contents of IENUMFALLBACK_STR.";
	#endregion

#pragma warning restore CA1707
}

#pragma warning restore