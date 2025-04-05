using UnityEngine;

public class Item : MonoBehaviour
{
    [Range(0f, 1f)]
    [Tooltip("Chance to spawn this item")]
    public float Chance = 0.3f;


}
