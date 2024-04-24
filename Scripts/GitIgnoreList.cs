using Godot;
using System;
using System.Diagnostics;
using System.IO;

public partial class GitIgnoreList : Node
{
    public override void _Ready()
    {
        // Folders specified in .gitignore
        string[] Folders = new string[] {"Audio", "Graphics"};
        var CurrentDir = Directory.GetCurrentDirectory();

        var lines = "";

        foreach (var folder in Folders)
        {
            var TargetDir = Path.Combine(CurrentDir, folder);

            if (Directory.Exists(TargetDir))
            {
                foreach (var file in Directory.GetFiles(TargetDir, "*", SearchOption.AllDirectories))
                {
                    if (file.Contains(".import"))
                        continue;

                    lines += file + "\n";
                }
            }
        }

        try
        {
            File.WriteAllText(Path.Combine(CurrentDir, "MissingFiles.txt"), lines);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error writing missing files:\n{ex.Message}");
        }
    }
}
