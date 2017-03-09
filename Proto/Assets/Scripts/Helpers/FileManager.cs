 using System.IO;

public class FileManager
{
    private readonly string _file;
    public FileManager(string filePath)
    {
        _file = filePath;
    }

    public void Append(string text)
    {
        var writer = File.AppendText(_file);
        writer.Write(text);
        writer.Flush();
    }

    public void Write(string text)
    {
        File.WriteAllText(_file, text);
    }

    public string Read()
    {
        return File.Exists(_file) ? File.ReadAllText(_file) : "";
    }
}