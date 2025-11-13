using System;
using UnityEngine;
using UnityEngine.UI;

public class DynamicBackgroundImage : MonoBehaviour
{
    [Header("UI Image for Background")]
    public Image backgroundImage;  // Assign your full-screen image here

    [Header("Backgrounds for Each Day")]
    public Sprite mondayBG;
    public Sprite tuesdayBG;
    public Sprite wednesdayBG;
    public Sprite thursdayBG;
    public Sprite fridayBG;
    public Sprite saturdayBG;
    public Sprite sundayBG;

    private void Start()
    {
        if (backgroundImage == null)
        {
            Debug.LogError("[DynamicBackgroundImage] Background Image is not assigned!");
            return;
        }

        SetBackgroundByDay();
    }

    private void SetBackgroundByDay()
    {
        DayOfWeek today = DateTime.Now.DayOfWeek;
        Sprite selected = null;

        switch (today)
        {
            case DayOfWeek.Monday:
                selected = mondayBG;
                break;
            case DayOfWeek.Tuesday:
                selected = tuesdayBG;
                break;
            case DayOfWeek.Wednesday:
                selected = wednesdayBG;
                break;
            case DayOfWeek.Thursday:
                selected = thursdayBG;
                break;
            case DayOfWeek.Friday:
                selected = fridayBG;
                break;
            case DayOfWeek.Saturday:
                selected = saturdayBG;
                break;
            case DayOfWeek.Sunday:
                selected = sundayBG;
                break;
        }

        if (selected != null)
        {
            backgroundImage.sprite = selected;
            Debug.Log($"[DynamicBackgroundImage] Background set for {today}");
        }
        else
        {
            Debug.LogWarning($"[DynamicBackgroundImage] No background assigned for {today}");
        }
    }
}
