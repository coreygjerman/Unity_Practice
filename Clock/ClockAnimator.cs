using UnityEngine;
using UnityEngine.UI;
using System;

public class ClockAnimator : MonoBehaviour
{
    public Transform hours, minutes, seconds;
    public bool analog;

    //Divide 360 degrees of a circle into how many point on the clock the arms have to choose from.
    private const float
        hoursToDegrees = 360f / 12f,
        minutesToDegrees = 360f / 60f,
        secondsToDegrees = 360f / 60f;

    private void Update()
    {
        if (analog)
        {
            TimeSpan timespan = DateTime.Now.TimeOfDay;
            hours.localRotation =
                Quaternion.Euler(0f, 0f, (float)timespan.TotalHours * -hoursToDegrees);
            minutes.localRotation =
                Quaternion.Euler(0f, 0f, (float)timespan.TotalMinutes * -minutesToDegrees);
            seconds.localRotation =
                Quaternion.Euler(0f, 0f, (float)timespan.TotalSeconds * -secondsToDegrees);
        }
        else
        {
            DateTime time = DateTime.Now;
            hours.localRotation =
                Quaternion.Euler(0f, 0f, time.Hour * -hoursToDegrees);
            minutes.localRotation =
                Quaternion.Euler(0f, 0f, time.Minute * -minutesToDegrees);
            seconds.localRotation =
                Quaternion.Euler(0f, 0f, time.Second * -secondsToDegrees);

        }
    }

    public void exitApplication()
    {
        Application.Quit();
    }

    public void analogToggle()
    {
        if (analog)
            analog = false;
        else
            analog = true;        
    }

}

