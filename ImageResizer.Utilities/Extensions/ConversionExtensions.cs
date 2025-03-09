namespace ImageResizer.Utilities.Extensions;

public static class ConversionExtensions
{
    public static int ToInt(this string value)
    {
        return int.Parse(value);
    }
    public static int ToIntOrDefault(this string value, int defaultValue = 0)
    {
        return int.TryParse(value, out var result) ? 
                result : 
                defaultValue;
    }
}