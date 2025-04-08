using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static float rotationSpeed = 80f;
    public static float currentAngle = 0f;

    void Update()
    {
        currentAngle += rotationSpeed * Time.deltaTime;
    }
}