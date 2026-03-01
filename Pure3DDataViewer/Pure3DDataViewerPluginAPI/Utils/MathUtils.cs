namespace Pure3DDataViewerPluginAPI.Utils;

public static class MathUtils
{
    public const double Pi = Math.PI;
    public const double TwoPi = Math.PI * 2.0;
    public const double Deg2Rad = Math.PI / 180.0;
    public const double Rad2Deg = 180.0 / Math.PI;

    public static double RadToDeg(double radians) => radians * Rad2Deg;

    public static double DegToRad(double degrees) => degrees * Deg2Rad;

    public static float RadToDeg(float radians) => radians * (float)Rad2Deg;

    public static float DegToRad(float degrees) => degrees * (float)Deg2Rad;

    /// <summary>
    /// Normalizes angle to range [0, 360)
    /// </summary>
    public static double NormalizeDegrees(double degrees)
    {
        degrees %= 360.0;
        if (degrees < 0)
            degrees += 360.0;
        return degrees;
    }

    /// <summary>
    /// Normalizes angle to range [0, 2π)
    /// </summary>
    public static double NormalizeRadians(double radians)
    {
        radians %= TwoPi;
        if (radians < 0)
            radians += TwoPi;
        return radians;
    }

    public static double Clamp(double value, double min, double max) => value < min ? min : (value > max ? max : value);

    public static float Clamp(float value, float min, float max) => value < min ? min : (value > max ? max : value);

    public static double Lerp(double a, double b, double t) => a + (b - a) * Clamp(t, 0.0, 1.0);

    public static float Lerp(float a, float b, float t) => a + (b - a) * Clamp(t, 0f, 1f);

    public static bool NearlyEqual(double a, double b, double epsilon = 1e-10) => Math.Abs(a - b) < epsilon;

    public static bool NearlyEqual(float a, float b, float epsilon = 1e-6f) => Math.Abs(a - b) < epsilon;

    /// <summary>
    /// Wraps value into [min, max)
    /// </summary>
    public static double Wrap(double value, double min, double max)
    {
        double range = max - min;
        return value - range * Math.Floor((value - min) / range);
    }
}
