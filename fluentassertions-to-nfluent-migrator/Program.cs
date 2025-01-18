using System.Text;
using fluentassertions_to_nfluent_migrator;

internal class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Error: No directory path provided.");
            return;
        }

        Console.WriteLine("Replacement started");
        var directoryPath = args[0];
        UpdateCsprojFiles(directoryPath);
        UpdateCsFiles(directoryPath);

        Console.WriteLine("Replacement complete.");
    }

    private static void UpdateCsFiles(string directoryPath)
    {
        const string csFileExtension = "cs";
        var files = Directory.GetFiles(directoryPath, $"*.{csFileExtension}", SearchOption.AllDirectories);

        // Spinner loader setup
        var spinner = new[] { '|', '/', '-', '\\' };
        int i = 0;

        Console.WriteLine("Processing .cs files...");

        // Process each .cs file
        foreach (var file in files)
        {
            // Display the spinner during the replacement
            Console.Write($"Processing {Path.GetFileName(file)}... ");
            var content = File.ReadAllText(file);
            var updatedContent = CsFileContentReplacer.Replace(content);
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

            // Print Done after processing a file
            Console.WriteLine("Done");
        }
    }

    private static void UpdateCsprojFiles(string directoryPath)
    {
        const string csProjFileExtension = "csproj";
        var csProjFiles = Directory.GetFiles(directoryPath, $"*.{csProjFileExtension}", SearchOption.AllDirectories);

        foreach (var file in csProjFiles)
        {
            var content = File.ReadAllText(file);
            var updatedContent = CsProjFileContentReplacer.Replace(content);
            File.WriteAllText(file, updatedContent, GetEncoding(file));
        }
    }

    private static Encoding GetEncoding(string filename)
    {
        // Read the BOM
        var bom = new byte[4];
        using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
        {
            file.ReadExactly(bom, 0, 4);
        }

        if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
        if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
        if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0) return Encoding.UTF32;
        if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode;
        if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode;
        if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return new UTF32Encoding(true, true);

        return Encoding.ASCII;
    }
}
