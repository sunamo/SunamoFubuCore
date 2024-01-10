namespace SunamoFubuCore;

public interface IFileSystem
{
    bool FileExists(string filename);
    void DeleteFile(string filename);
    void MoveFile(string from, string to);
    void MoveDirectory(string from, string to);
    bool IsFile(string path);

    string GetFullPath(string path);

    long FileSizeOf(string path);

    void Copy(string source, string destination);

    Task WriteStreamToFile(string filename, Stream stream);
    Task WriteStringToFile(string filename, string text);
    Task AppendStringToFile(string filename, string text);

#if ASYNC
    Task<string>
#else
string
#endif
    ReadStringFromFile(string filename);
    void WriteObjectToFile(string filename, object target);
    T LoadFromFile<T>(string filename) where T : new();
    T LoadFromFileOrThrow<T>(string filename) where T : new();

    void CreateDirectory(string directory);

    /// <summary>
    ///     Deletes the directory
    /// </summary>
    void DeleteDirectory(string directory);

    /// <summary>
    ///     Deletes the directory to clear the content
    ///     Then recreates it. An empty clean, happy, directory.
    /// </summary>
    /// <param name="directory"></param>
    void CleanDirectory(string directory);

    bool DirectoryExists(string directory);

    void LaunchEditor(string filename);
    IEnumerable<string> ChildDirectoriesFor(string directory);
    IEnumerable<string> FindFiles(string directory, FileSet searchSpecification);

    void ReadTextFile(string path, Action<string> reader);
    void MoveFiles(string from, string to);

    string GetDirectory(string path);
    string GetFileName(string path);

    void AlterFlatFile(string path, Action<List<string>> alteration);
    void Copy(string source, string destination, CopyBehavior behavior);
}
