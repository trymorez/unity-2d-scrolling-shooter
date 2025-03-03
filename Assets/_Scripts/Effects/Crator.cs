using UnityEngine;

public class Crator : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
        {
            Destroy(gameObject);
        }
    }
}
