using System.Collections.Generic;

namespace VersionController.Core
{
    public interface IDirectoryUtils
    {
        List<string> GetNugetPackages();
        List<string> GetDotNugetPackages();
        List<string> GetFilterPackages(string filterFileNames);
        void Delete(List<string> filterFileNames, string filePath);
        void Publish(string directory, string fileName);
    }
}