 using System.IO;

public class FileManager
{
    private readonly string _file;
    public FileManager(string filePath)
    {
        _file = filePath;
    }

    public void Write(string text)
    {
        var writer = File.AppendText(_file);
        writer.Write(text);
        writer.Flush();
    }

    public string Read()
    {
        return File.ReadAllText(_file);
    }
}