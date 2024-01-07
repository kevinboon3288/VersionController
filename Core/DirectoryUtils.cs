using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VersionController.Core
{
    public class DirectoryUtils
    {
        private const string _nugetPackagesPath = @"\Microsoft SDKs\NuGetPackages";
        private const string _iconResourcePath = @"/Resource;component/Resources/Icons/";
        private const string _folderPattern = @"(?<=\..*)\.";
        private string _filterFileNames = String.Empty;

        public DirectoryUtils(string filterFileNames)
        {
            _filterFileNames = filterFileNames;
        }

        public List<Folder> GetFilterFolders()
        {
            List<Folder> folders = new List<Folder>();

            string nugetPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + _nugetPackagesPath;
            string[] filterFolderPaths = Directory.GetDirectories(nugetPath, _filterFileNames);

            foreach (string folder in filterFolderPaths)
            {
                Match match = Regex.Match(folder, _folderPattern);
                if (match.Success)
                {
                    folders.Add(new Folder(Path.GetFileName(folder.Substring(match.Index + 1).ToUpper()), _iconResourcePath + "Fail16x16.png"));
                }
            }

            return folders;
        }

        public void Delete()
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

                    DirectoryInfo[] nugetFolders = directory.GetDirectories(_filterFileNames, SearchOption.AllDirectories);

                    Parallel.ForEach(nugetFolders, nugetFolder =>
                    {
                        try
                        {
                            //TODO: Add log to track which directory is on-delete.
                            Directory.Delete(nugetFolder.FullName, true);
                        }
                        catch (IOException ex)
                        {
                            throw new IOException($"Unable to delete because file in used : {ex.Message}");
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            throw new UnauthorizedAccessException($"Unable to delete because file is access denied : {ex.Message}");
                        }
                    });
                });

            });

            //TODO: Add log for delete completed 
            //message = "Deleted all folder in .nuget and NugetPackages directory";
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
                catch (Exception ex)
                {
                    throw new ArgumentException($"Unable to publish : {ex.Message}");
                }

                //TODO: Add log for publish completed 
                //message = "Publish of NuGet in NugetPackages directory is Completed";
            }
        }
    }
}
