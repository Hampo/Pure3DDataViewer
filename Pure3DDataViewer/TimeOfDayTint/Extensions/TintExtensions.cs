using TimeOfDayTint.Enums;

namespace TimeOfDayTint.Extensions;
internal static class TintExtensions
{
    private static readonly Dictionary<TimeOfDay, Color> TimeOfDayTints = new()
    {
        { TimeOfDay.Dawn, Color.FromArgb(255, 255, 220, 180) },
        { TimeOfDay.Day, Color.FromArgb(255, 255, 255, 255) },
        { TimeOfDay.Sunset, Color.FromArgb(255, 255, 160, 80) },
        { TimeOfDay.Night, Color.FromArgb(255, 100, 120, 200) },
    };

    internal static Color GetTint(this TimeOfDay timeOfDay)
    {
        if (TimeOfDayTints.TryGetValue(timeOfDay, out var tint))
            return tint;
        return Color.White;
    }

    internal static Color Lerp(this Color current, Color @new, float blend)
    {
        blend = Math.Clamp(blend, 0f, 1f);
        return Color.FromArgb(
            255,
            (int)(current.R + (@new.R - current.R) * blend),
            (int)(current.G + (@new.G - current.G) * blend),
            (int)(current.B + (@new.B - current.B) * blend)
        );
    }

    private static int Clamp255(int v) => Math.Clamp(v, 0, 255);

    internal static Color Multiply(this Color a, Color b)
    {
        return Color.FromArgb(
            a.A,
            Clamp255(a.R * b.R / 255),
            Clamp255(a.G * b.G / 255),
            Clamp255(a.B * b.B / 255)
        );
    }

    internal static Color ApplyBrightness(this Color c, float brightness)
    {
        return Color.FromArgb(
            c.A,
            Clamp255((int)(c.R * brightness)),
            Clamp255((int)(c.G * brightness)),
            Clamp255((int)(c.B * brightness))
        );
    }
}
