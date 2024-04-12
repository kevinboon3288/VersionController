namespace PackageModule.Core
{
    public interface IDirectoryUtils
    {
        List<(string, string?)> GetNugetPackages();
        List<(string, string?)> GetDotNugetPackages();
        List<(string, string?)> GetFilterPackages(string filterFileNames);
        void Delete(List<string> filterFileNames, string filePath);
        void Publish(string directory, string fileName);
    }
}