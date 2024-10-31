using UnityEngine;
using TMPro;
using System;

public class ClockController : MonoBehaviour
{
    [Header("Clock Hands")]
    public Transform hourHand;
    public Transform minuteHand;
    public Transform secondHand;

    [Header("UI Elements")]
    public TMP_Text timeText;
    public TMP_InputField timeInput;

    private TimeSpan currentTime;
    private bool isDraggingHour = false;
    private bool isDraggingMinute = false;
    private bool isDraggingSecond = false;

    void Start()
    {
        currentTime = new TimeSpan(0, 0, 0);
        UpdateClockHands();
        UpdateTimeText();
    }

    void OnEnable()
    {
        timeInput.onEndEdit.AddListener(SetTimeFromInput);
    }

    void OnDisable()
    {
        timeInput.onEndEdit.RemoveListener(SetTimeFromInput);
    }

    void Update()
    {
        if (!isDraggingHour && !isDraggingMinute && !isDraggingSecond)
        {
            currentTime += TimeSpan.FromSeconds(Time.deltaTime);
            if (currentTime.TotalSeconds >= 86400)
            {
                currentTime = TimeSpan.Zero;
            }
            UpdateClockHands();
            UpdateTimeText();
        }
    }

    void UpdateClockHands()
    {
        float hourDegree = (float)(currentTime.TotalHours / 12) * 360f;
        float minuteDegree = (float)(currentTime.TotalMinutes / 60) * 360f;
        float secondDegree = (float)(currentTime.TotalSeconds / 60) * 360f;

        hourHand.localRotation = Quaternion.Euler(0, 0, -hourDegree);
        minuteHand.localRotation = Quaternion.Euler(0, 0, -minuteDegree);
        secondHand.localRotation = Quaternion.Euler(0, 0, -secondDegree);
    }

    void UpdateTimeText()
    {
        timeText.text = currentTime.ToString(@"hh\:mm\:ss");
    }

    public void SetTimeFromInput(string input)
    {
        if (TimeSpan.TryParseExact(input, @"hh\:mm\:ss", null, out TimeSpan parsedTime))
        {
            currentTime = parsedTime;
            UpdateClockHands();
            UpdateTimeText();
        }
        else
        {
            Debug.LogWarning("Неверный формат времени. Используйте формат HH:MM:SS");
        }
    }
    public void BeginDragHour() { isDraggingHour = true; }
    public void EndDragHour() { isDraggingHour = false; }
    public void BeginDragMinute() { isDraggingMinute = true; }
    public void EndDragMinute() { isDraggingMinute = false; }
    public void BeginDragSecond() { isDraggingSecond = true; }
    public void EndDragSecond() { isDraggingSecond = false; }

    public void UpdateTimeFromHand(HandType handType, float angle)
    {
        switch (handType)
        {
            case HandType.Hour:
                double hours = (angle % 360) / 360 * 12;
                int intHours = (int)Math.Floor(hours);
                currentTime = new TimeSpan(intHours, currentTime.Minutes, currentTime.Seconds);
                break;
            case HandType.Minute:
                double minutes = (angle % 360) / 360 * 60;
                int intMinutes = (int)Math.Floor(minutes);
                currentTime = new TimeSpan(currentTime.Hours, intMinutes, currentTime.Seconds);
                break;
            case HandType.Second:
                double seconds = (angle % 360) / 360 * 60;
                int intSeconds = (int)Math.Floor(seconds);
                currentTime = new TimeSpan(currentTime.Hours, currentTime.Minutes, intSeconds);
                break;
        }
        UpdateClockHands();
        UpdateTimeText();
    }

    public enum HandType
    {
        Hour,
        Minute,
        Second
    }
}

