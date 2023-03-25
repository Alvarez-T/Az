namespace Az.Extensions;

public static class StringExtensions
{
    public static bool IsEmpty(this string content)
    {
        return string.IsNullOrEmpty(content) || string.IsNullOrWhiteSpace(content);
    }
    public static bool IsNotEmpty(this string content)
    {
        return !content.IsEmpty();
    }
}
