public class Top
{
    public string Style { get; }
    public string Color { get; }
    public string Pattern { get; }
    public string Length { get; }

    public Top(string style, string color, string pattern, string length)
    {
        Style = style;
        Color = color;
        Pattern = pattern;
        Length = length;
    }
}