using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
    [Range(0f, 1f)]
    [Tooltip("Chance to spawn this power up")]
    public float Chance = 0.3f;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] SpriteRenderer shadow;
    public ePowerUpType Type;
    public enum ePowerUpType
    {
        Gun = 1,
        Shield = 2,
    }
    

    void Update()
    {
        sprite.transform.rotation = Quaternion.Euler(0, 0, PowerUpManager.currentAngle);
        shadow.transform.rotation = Quaternion.Euler(0, 0, PowerUpManager.currentAngle);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
        {
            Destroy(gameObject);
        }
    }
}
