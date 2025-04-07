using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Range(0f, 1f)]
    [Tooltip("Chance to spawn this power up")]
    public float Chance = 0.3f;
    [SerializeField] float rotationSpeed = 1f;
    float currentAngle = 0f;

    void Update()
    {
        currentAngle += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
        {
            Destroy(gameObject);
        }
    }
}
