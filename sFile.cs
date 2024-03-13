#pragma warning disable IDE0079 //Remove unnessecary supression
#pragma warning disable CA1050 //Declare types in namespaces
using System.Text;

public static partial class SafeFile
{
	/// <summary> Appends lines to a file, and then closes the file. If the specified file foes not exist, this method creates a file, writes the specified lines to the file, then closes the file. </summary>
	/// <param name="path"> The directory of the file to modify or create. If invalid, will be set to <see cref="FALLBACK_FILE"/>, or <paramref name="safepath"/> if given. </param>
	/// <param name="contents"> The contents to insert into the file. If invalid, will be set to <see cref="IENUMFALLBACK_STR"/>, or <paramref name="safecontents"/> if given. </param>
	/// <param name="safepath"> The path to fall back to if <paramref name="path"/> fails. Resets to <see cref="FALLBACK_FILE"/> if invalid. </param>
	/// <param name="safecontents"> The contents to use if <paramref name="contents"/> fails. Resets to <see cref="IENUMFALLBACK_STR"/> if invalid. </param>
	/// <returns> True if all operations proceeded without the use of fallbacks, otherwise returns false. </returns>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>")]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
	public static bool AppendAllLines(string path, IEnumerable<string> contents, string? safepath = null, IEnumerable<string>? safecontents = null)
	{
		if (Console.Out == null) Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
		bool safetyfail = false;

		// safepath override check
		safepath ??= FALLBACK_FILE;
		if (string.IsNullOrWhiteSpace(safepath))
		{
			safetyfail = true;
			try { Console.Error.WriteLine(SAFEPATH_BLANK); }
			catch (Exception) { Console.SetOut(new StreamWriter(Console.OpenStandardOutput())); Console.Error.WriteLine(SAFEPATH_BLANK); }
			safepath = FALLBACK_FILE;
		}

		// safepath safety check
		#pragma warning disable CA1031 //Do not catch general exception types
		try { File.Open(safepath, FileMode.OpenOrCreate).Dispose(); File.Delete(safepath); }
		catch (Exception)
		{
			safetyfail = true;
			try { Console.Error.WriteLine(SAFEPATH_FAIL); }
			catch (Exception) { Console.SetOut(new StreamWriter(Console.OpenStandardOutput())); Console.Error.WriteLine(SAFEPATH_FAIL); }
			safepath = FALLBACK_FILE;
		}
		#pragma warning restore

		// null or empty path check
		if (string.IsNullOrWhiteSpace(path)) path = safepath;

		// path validity check
		#pragma warning disable CA1031 //Do not catch general exception types
		try { File.Open(path, FileMode.OpenOrCreate).Dispose(); File.Delete(safepath); }
		catch (Exception) { path = safepath; }
		#pragma warning restore

		// null or empty contents check
		if (contents == null || contents.Any())
		{
			if (safecontents != null && !safecontents.Any()) contents = safecontents;
		}
		if (contents == null || contents.Any())
		{
			if (safecontents != null)
			{
				safetyfail = true;
				try { Console.Error.WriteLine(SAFECONTENT_EMPTY); }
				catch (Exception) { Console.SetOut(new StreamWriter(Console.OpenStandardOutput())); Console.Error.WriteLine(SAFECONTENT_EMPTY); }
			}
			safecontents = IENUMFALLBACK_STR;
			contents = safecontents;
		}

		// Final method call
		try
		{
			try
			{
				File.AppendAllLines(path, contents);
			}
			catch (Exception)
			{
				File.AppendAllLines(@$"{Environment.CurrentDirectory}\LiterallyEverythingFailed.txt", contents);
			}
		}
		catch (Exception)
		{
			string[] x = contents.ToArray();
			path = string.Join("", x);
			byte[] data = Encoding.GetEncoding("UTF-8").GetBytes(path);
			char[] Binary = string.Join(" ", data.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0'))).ToCharArray();
			for (int i = 0; i < Binary.Length; i++)
			{
				if (Binary[i] == '0') Console.Beep(2400, 333); else Console.Beep(4200, 333);
			}
			throw;
		}
		return !safetyfail;
	}
}

#pragma warning restore