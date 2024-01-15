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

        public List<string> GetPackages() 
        {
            return ReadPackages(Directory.GetDirectories(ConstantFilePaths.NugetX86FilePath));
        }

        public List<string> GetFilterPackages(string filterFileNames)
        {           
            List<string> packages = ReadPackages(Directory.GetDirectories(ConstantFilePaths.NugetX86FilePath));
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
                    folders.Add(Path.GetFileName(folder.Substring(match.Index + 1).ToUpper()));
                }
            }

            _logger.Information("Folders loaded successfully");

            return folders;
        }

        public void Delete(string filterFileNames)
        {
            DirectoryInfo[] directories = new[]
            {
                new DirectoryInfo(ConstantFilePaths.NugetX86FilePath),
                new DirectoryInfo(ConstantFilePaths.DotNugetFilePath)
            };

            Task.Run(() =>
            {
                Parallel.ForEach(directories, directory =>
                {
                    if (!directory.Exists)
                    {
                        return;
                    }

                    DirectoryInfo[] nugetFolders = directory.GetDirectories(filterFileNames, SearchOption.AllDirectories);

                    Parallel.ForEach(nugetFolders, nugetFolder =>
                    {
                        try
                        {
                            Directory.Delete(nugetFolder.FullName, true);
                            _logger.Information($"{nugetFolder.FullName} delete successfully");
                        }
                        catch (IOException ex)
                        {
                            _logger.Error($"Unable to delete because file in used : {ex.Message}");
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            _logger.Error($"Unable to delete because file is access denied : {ex.Message}");
                        }
                    });
                });

            });

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
