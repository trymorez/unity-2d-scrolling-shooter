using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int Health;
    [SerializeField] SpriteRenderer[] bodyPart;
    [SerializeField] float flashingTime = 0.3f;
    Color[] originalColor;
    bool isFlashing;

    void Start()
    {
        originalColor = new Color[bodyPart.Length];
        for (int i = 0; i < bodyPart.Length; i++)
        {
            originalColor[i] = bodyPart[i].color;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            var hitEffect = HitEffectPoolManager.Get();
            hitEffect.transform.position = other.transform.position;

            other.gameObject.SetActive(false);
        }
        if (!isFlashing)
        {
            StartFlashing();
        }
    }

    void StartFlashing()
    {
        isFlashing = true;
        for (int i = 0; i < bodyPart.Length; i++)
        {
            bodyPart[i].color = Color.red;
        }
        Invoke("EndFlashing", flashingTime);
    }

    void EndFlashing()
    {
        for (int i = 0; i < bodyPart.Length; i++)
        {
            bodyPart[i].color = originalColor[i];
        }
        isFlashing = false;
    }
}
