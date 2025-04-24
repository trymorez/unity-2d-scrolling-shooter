using System.Collections.Generic;
using UnityEngine;

public class SpawnPowerUp : MonoBehaviour
{
    [SerializeField] float totalChance = 1.0f;
    float emptyChance;
    [SerializeField] List<PowerUp> powerUps = new();
    [SerializeField] List<float> chances = new();

    void Start()
    {
        float addedChance = 0;

        for (int i = 0; i < powerUps.Count; i++)
        {
            addedChance += powerUps[i].Chance;
            chances.Add(addedChance);
        }

        emptyChance = totalChance - addedChance;
        Debug.Log("empty chance: " + emptyChance);
        chances.Add(totalChance);
    }

    /// <summary>
    ///     Spawn power up items
    /// </summary>
    /// <returns>
    ///     Index of spawned power up's index. -1 means no power up spawned.
    /// </returns>
    public int ChoosePowerUp()
    {
        float chance = Random.Range(0, totalChance);

        for (int i = 0; i < chances.Count - 1; i++)
        {
            if (chance >= chances[i] && chance < chances[i + 1])
            {
                Instantiate(powerUps[i], transform.position, Quaternion.identity, GameManager.World);
                return i;
            }
        }

        return -1;
    }
}
