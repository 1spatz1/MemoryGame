using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MemoryGame.WPF.Functions;

public static class GetFilesFunction
{
    public static IEnumerable<string> GetFiles(string path, string searchPatternExpression, SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        // Make new regex
        Regex reSearchPattern = new Regex(searchPatternExpression);
        return Directory.EnumerateFiles(path, "*", searchOption).Where(file => reSearchPattern.IsMatch(Path.GetFileName(file)));
    }
    public static List<string> GetFileNames(string path, string searchPatternExpression)
    {
        // Take a snapshot of the file system.  
        DirectoryInfo dir = new DirectoryInfo(path);  
  
        // This method assumes that the application has discovery permissions  
        // for all folders under the specified path.  
        IEnumerable<FileInfo> fileList = dir.GetFiles("*.*", SearchOption.TopDirectoryOnly);
  
        //Create the query  
        Regex reSearchPattern = new Regex(searchPatternExpression);

        IEnumerable<FileInfo> fileQuery =
            from file in fileList
            where reSearchPattern.IsMatch(file.Name)
            select file;
        
        return fileQuery.Select(file => file.Name).ToList();
    }
}