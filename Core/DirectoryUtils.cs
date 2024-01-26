using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VersionController.Core
{
    public class DirectoryUtils : IDirectoryUtils
    {
        private readonly ILogger _logger;
        private const string _folderPattern = @"(?<=\..*)\.";

        public DirectoryUtils(ILogger logger)
        {
            _logger = logger;
        }

        public List<string> GetNugetPackages()
        {
            List<string> nugetPackages = ReadPackages(Directory.GetDirectories(ConstantFilePaths.NugetX86FilePath));
            
            _logger.Information("NuGetPackages Folders loaded successfully");

            return nugetPackages;
        }

        public List<string> GetDotNugetPackages() 
        {
            List<string> dotNugetPackages = ReadPackages(Directory.GetDirectories(ConstantFilePaths.DotNugetFilePath));

            _logger.Information(".nuget Folders loaded successfully");

            return dotNugetPackages;
        }

        public List<string> GetFilterPackages(string filterFileNames)
        {           
            List<string> packages = ReadPackages(Directory.GetDirectories(ConstantFilePaths.DotNugetFilePath));
            return packages.FindAll(x => x.Contains(filterFileNames.ToUpper())).ToList();
        }

        private List<string> ReadPackages(string[] filterFolderPaths) 
        {
            List<string> folders = new();

            foreach (string folder in filterFolderPaths)
            {
                Match match = Regex.Match(folder, _folderPattern);
                if (match.Success)
                {
                    folders.Add(Path.GetFileName(folder));
                }
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
                        using (Process process = Process.Start(cmdPublisherStartInfo))
                        {
                            process!.WaitForExit();
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
}
