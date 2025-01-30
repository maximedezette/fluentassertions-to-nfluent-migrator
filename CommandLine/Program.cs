using System.Text;

namespace CommandLine;

internal class Program
{
    
    static List<string> _timeoutFiles = [];
    const int DEFAULT_PROCESSING_TIMEOUT = 30;
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: No directory path provided.");
            return;
        }

        Console.WriteLine("Migration started");
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var directoryPath = args[0];
        UpdateCsprojFiles(directoryPath);
        UpdateTestFiles(directoryPath);

        watch.Stop();
        var elapsed = (int)watch.Elapsed.TotalSeconds;
       Console.OutputEncoding = Encoding.UTF8;

        if (_timeoutFiles.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n******************************************");
            Console.WriteLine($"**** \u2728  Migration complete in {elapsed}s  \u2728  ****");
            Console.WriteLine("****************************************** \n");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n**************************************************");
            Console.WriteLine($"*** ⚠️ Migration partially complete in {elapsed}s ⚠️  ***");
            Console.WriteLine($"***              {_timeoutFiles.Count} file(s) skipped              ***");
            Console.WriteLine("**************************************************\n");
        }

        Console.ResetColor();
    }

    private static void UpdateTestFiles(string directoryPath)
    {
        var files = GetTestFiles(directoryPath);
        ProcessReplacement(files, new CsFileContentReplacer.CsFileContentReplacer());
    }

    private static string[] GetTestFiles(string directoryPath)
    {
        var testFilePatterns = new[] { "*StepDef*.cs", "*Test*.cs" };

        var files = testFilePatterns
            .SelectMany(pattern => Directory.GetFiles(directoryPath, pattern, SearchOption.AllDirectories))
            .Distinct()
            .ToArray();
        return files;
    }

    private static void UpdateCsprojFiles(string directoryPath)
    {
        var csProjFiles = GetCsProjFiles(directoryPath);
        ProcessReplacement(csProjFiles, new CsProjFileContentReplacer.CsProjFileContentReplacer());
    }

    private static string[] GetCsProjFiles(string directoryPath)
    {
        const string csProjFileExtension = "csproj";
        var csProjFiles = Directory.GetFiles(directoryPath, $"*.{csProjFileExtension}", SearchOption.AllDirectories);
        return csProjFiles;
    }

    private static void ProcessReplacement(string[] csProjFiles, IReplacer replacer)
    {
        var spinner = new[] { '|', '/', '-', '\\' };
        int i = 0;

        Console.WriteLine("Processing .csproj files...");

        foreach (var file in csProjFiles)
        {
            // Display the spinner during the replacement
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"Processing {Path.GetFileName(file)}... ");
            var content = File.ReadAllText(file, Encoding.UTF8);
            string updatedContent;
            try
            {
                updatedContent =
                    ReplaceWithTimeout(replacer, content, Path.GetFileName(file), TimeSpan.FromSeconds(DEFAULT_PROCESSING_TIMEOUT));
            }
            catch (TimeoutException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: Processing {Path.GetFileName(file)} took too long (Possible infinite loop)");
                Console.ResetColor();
                continue;
            }

            File.WriteAllText(file, updatedContent, GetEncoding(file));
            
            // Only move the cursor if it's safe
            if (Console.CursorLeft > 0)
            {
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop); // Move cursor to the left
                Console.Write(spinner[i % spinner.Length]); // Print the next spinner character
                Thread.Sleep(100); // Wait a bit for the spinner effect
                i++;
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop); // Remove spinner
            }


            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(0, Console.CursorTop);
            
            Console.WriteLine($"Processing {Path.GetFileName(file)}... Done");
            Console.ResetColor();
        }
        
        
        if (_timeoutFiles.Count > 0)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n ⚠️ The following files took too long to process:");
            foreach (var file in _timeoutFiles)
            {
                Console.WriteLine($"   - {file}");
            }
            Console.ResetColor();
        }
    }

    private static string ReplaceWithTimeout(IReplacer replacer, string content, string fileName, TimeSpan timeout)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(timeout);

        var task = Task.Run(() => replacer.Replace(content), cancellationTokenSource.Token);

        if (task.Wait(timeout))
        {
            return task.Result;
        }

        AddTimeoutFile(fileName);
        throw new TimeoutException($"Replacement in file {fileName} exceeded {timeout.TotalSeconds} seconds.");
    }

    private static void AddTimeoutFile(string fileName)
    {
        _timeoutFiles.Add(fileName);
    }

    private static Encoding GetEncoding(string filename)
    {
        // Read the BOM
        var bom = new byte[4];
        using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
        {
            // Read up to 4 bytes to detect the BOM
            file.Read(bom, 0, bom.Length);
        }

        // Detect encoding by BOM
        if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7; // UTF-7
        if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8; // UTF-8 with BOM
        if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0) return Encoding.UTF32; // UTF-32 LE
        if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; // UTF-16 LE
        if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; // UTF-16 BE
        if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff)
            return new UTF32Encoding(true, true); // UTF-32 BE

        // If no BOM is found, assume UTF-8 (without BOM)
        return new UTF8Encoding(false);
    }
}