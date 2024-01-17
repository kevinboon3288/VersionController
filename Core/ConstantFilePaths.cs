using System;

namespace VersionController.Core
{
    public class ConstantFilePaths
    {
        public static string NugetX86FilePath => Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Microsoft SDKs\NuGetPackages";
        public static string DotNugetFilePath => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\.nuget\packages";
    }
}
