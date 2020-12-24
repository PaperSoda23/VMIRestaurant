using System.IO;

namespace VMIRestaurant.helper
{
    public static class DirectoryHelper
    {
        private static string GetRecursiveParent(string directory, int stepCounter)
        {
            var parentDir = Directory.GetParent(directory)?.FullName;
            return stepCounter == 0 || string.IsNullOrEmpty(parentDir) 
                ? directory
                : GetRecursiveParent(parentDir, stepCounter - 1);
        }

        public static string GetProjectRootDir() => GetRecursiveParent(Directory.GetCurrentDirectory(), 3);
    }
}