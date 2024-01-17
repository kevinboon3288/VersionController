using System.Collections.Generic;
using VersionController.PackageModule.Models;

namespace VersionController.Core
{
    public interface IDirectoryUtils
    {
        List<string> GetPackages();
        List<string> GetFilterPackages(string filterFileNames);
        void Delete(List<string> filterFileNames);
        void Publish(string directory, string fileName);
    }
}