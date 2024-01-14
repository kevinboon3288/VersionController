using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VersionController.Core
{
    public class DirectoryUtils : IDirectoryUtils
    {
        private readonly ILogger _logger;
        private const string _nugetPackagesPath = @"\Microsoft SDKs\NuGetPackages";
        private const string _folderPattern = @"(?<=\..*)\.";

        public DirectoryUtils(ILogger logger)
        {
            _logger = logger;
        }

        public List<string> GetPackages() 
        {
            List<string> folders = new();

            string nugetPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + _nugetPackagesPath;
            string[] filterFolderPaths = Directory.GetDirectories(nugetPath);

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

        public List<string> GetFilterPackages(string filterFileNames)
        {
            List<string> folders = new();

            string nugetPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + _nugetPackagesPath;
            string[] filterFolderPaths = Directory.GetDirectories(nugetPath, filterFileNames);

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
            string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            string nugetPackagesPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            DirectoryInfo[] directories = new[]
            {
                new DirectoryInfo(programFilesPath + @"\Microsoft SDKs\NuGetPackages"),
                new DirectoryInfo(nugetPackagesPath + @"\.nuget\packages")
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
                            //TODO: Add log to track which directory is on-delete.
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
