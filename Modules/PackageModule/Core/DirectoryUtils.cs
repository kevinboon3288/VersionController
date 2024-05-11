namespace PackageModule.Core;

public class DirectoryUtils : IDirectoryUtils
{
    private readonly ILogger _logger;

    public DirectoryUtils(ILogger logger)
    {
        _logger = logger;
    }

    public List<(string, string?)> GetNugetPackages()
    {
        List<(string, string?)> nugetPackages = ReadPackages(Directory.GetDirectories(ConstantFilePaths.NugetX86FilePath));
        
        _logger.Information("NuGetPackages Folders loaded successfully");

        return nugetPackages;
    }

    public List<(string, string?)> GetDotNugetPackages() 
    {
        List<(string, string?)> dotNugetPackages = ReadPackages(Directory.GetDirectories(ConstantFilePaths.DotNugetFilePath));

        _logger.Information(".nuget Folders loaded successfully");

        return dotNugetPackages;
    }

    public List<(string, string?)> GetDotNugetFilterPackages(string filterFileNames)
    {
        List<(string fileName, string? version)> packages = ReadPackages(Directory.GetDirectories(ConstantFilePaths.DotNugetFilePath));

        return packages.Where(x => x.fileName.Contains(filterFileNames) || (!string.IsNullOrEmpty(x.version) && x.version.Contains(filterFileNames))).ToList();   
    }

    public List<(string, string?)> GetNugetFilterPackages(string filterFileNames)
    {
        List<(string fileName, string? version)> packages = ReadPackages(Directory.GetDirectories(ConstantFilePaths.NugetX86FilePath));

        return packages.Where(x => x.fileName.Contains(filterFileNames) || (!string.IsNullOrEmpty(x.version) && x.version.Contains(filterFileNames))).ToList();
    }

    private List<(string, string?)> ReadPackages(string[] filterFolderPaths) 
    {
        List<(string, string?)> folders = new();

        foreach (string folder in filterFolderPaths)
        {
            string? version = Path.GetFileName(Directory.GetDirectories(folder, "*", SearchOption.TopDirectoryOnly).FirstOrDefault());

            folders.Add((Path.GetFileName(folder), version));
        }            

        return folders;
    }

    public void Delete(List<string> filterFileNames, string filePath)
    {
        Task.Run(() =>
        {
            DirectoryInfo directory = new DirectoryInfo(filePath);
            if (!directory.Exists)
            {
                _logger.Warning($"{directory.Name} is not existed.");
                return;
            }

            Parallel.ForEach(directory.GetDirectories(), nugetFolder =>
            {
                try
                {
                    if (filterFileNames.Any(x => x.Equals(nugetFolder.Name)))
                    {
                        Directory.Delete(nugetFolder.FullName, true);
                        _logger.Information($"{nugetFolder.FullName} delete successfully");
                    }
                }
                catch (IOException ex)
                {
                    _logger.Error($"{nugetFolder.Name} unable to delete because file in used : {ex.Message}");
                }
                catch (UnauthorizedAccessException ex)
                {
                    _logger.Error($"{nugetFolder.Name} unable to delete because file is access denied : {ex.Message}");
                }
            });
        }).Await();

        _logger.Information("Deleted all folder in .nuget and NugetPackages directory");
    }

    public void Publish(string directory, string fileName)
    {
        string cmdPublisherFilePath = System.IO.Path.Combine(directory, fileName);

        if (System.IO.File.Exists(cmdPublisherFilePath))
        {
            ProcessStartInfo cmdPublisherStartInfo = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                Arguments = $"/c \"{cmdPublisherFilePath}\"",
                Verb = "runas",
                UseShellExecute = false,
                WorkingDirectory = directory
            };

            try
            {
                Task.Run(() =>
                {
                    _logger.Information("Publishing the packages");

                    using (Process? process = Process.Start(cmdPublisherStartInfo))
                    {
                        if(process != null)
                        {
                            process.WaitForExit(); 
                            _logger.Information($"{process.ExitCode}");
                        }
                    }
                });
            }
            catch (ArgumentException ex)
            {
                _logger.Error($"Unable to publish : {ex.Message}");
            }

            _logger.Information("Publish of NuGet in NugetPackages directory is Completed");
        }
    }
}
