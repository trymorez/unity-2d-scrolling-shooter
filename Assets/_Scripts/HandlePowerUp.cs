using UnityEngine;

public class HandlePowerUp : MonoBehaviour
{
    PlayerGun playerGun;

    void Start()
    {
        playerGun = GetComponent<PlayerGun>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PowerUp"))
        {
            var powerUp = other.GetComponent<PowerUp>();

            switch (powerUp.Type)
            {
                case PowerUp.ePowerUpType.Shield:
                    Debug.Log("Shield Up");
                    break;
                case PowerUp.ePowerUpType.Gun:
                    playerGun.UpgradeGun();
                    Debug.Log("Gun Up");
                    break;
            }

            Destroy(other.gameObject);
        }
    }
}
