using UnityEngine;

public class DraggableHand : MonoBehaviour
{
    public ClockController.HandType handType;
    private bool isDragging = false;
    private ClockController clockController;
    private float currentAngle;

    void Start()
    {
        clockController = FindObjectOfType<ClockController>();
        currentAngle = transform.localEulerAngles.z;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    isDragging = true;
                    switch (handType)
                    {
                        case ClockController.HandType.Hour:
                            clockController.BeginDragHour();
                            break;
                        case ClockController.HandType.Minute:
                            clockController.BeginDragMinute();
                            break;
                        case ClockController.HandType.Second:
                            clockController.BeginDragSecond();
                            break;
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            switch (handType)
            {
                case ClockController.HandType.Hour:
                    clockController.EndDragHour();
                    break;
                case ClockController.HandType.Minute:
                    clockController.EndDragMinute();
                    break;
                case ClockController.HandType.Second:
                    clockController.EndDragSecond();
                    break;
            }
        }
        if (isDragging)
        {
            float deltaAngle = Input.GetAxis("Mouse X") * 8f; 
            currentAngle -= deltaAngle;
            
            currentAngle = (currentAngle + 360) % 360;
           
            transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
           
            clockController.UpdateTimeFromHand(handType, currentAngle);
        }
    }
}