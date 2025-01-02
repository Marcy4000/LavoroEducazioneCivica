using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    public Light2D globalLight; // Reference to the global light
    public Color dayColor = new Color(1f, 0.95f, 0.8f, 1f); // Bright, warm light for daytime
    public Color nightColor = new Color(0.1f, 0.1f, 0.3f, 1f); // Cool, dim light for nighttime

    [Header("Day/Night Transition")]
    public int dayStartHour = 6; // Start of the day (6 AM)
    public int nightStartHour = 18; // Start of the night (6 PM)

    private void Update()
    {
        UpdateLightColorBasedOnTime();
    }

    private void UpdateLightColorBasedOnTime()
    {
        int currentHour = System.DateTime.Now.Hour;
        float t = GetTimeInterpolation(currentHour);
        globalLight.color = Color.Lerp(nightColor, dayColor, t);
    }

    private float GetTimeInterpolation(int hour)
    {
        if (hour >= dayStartHour && hour < nightStartHour)
        {
            // Daytime: Interpolate from start of the day to the start of the night
            return (hour - dayStartHour) / (float)(nightStartHour - dayStartHour);
        }
        else
        {
            // Nighttime: Interpolate from start of the night to the start of the day
            int adjustedHour = (hour >= nightStartHour) ? hour - nightStartHour : hour + (24 - nightStartHour);
            return adjustedHour / (float)(24 - (nightStartHour - dayStartHour));
        }
    }
}
