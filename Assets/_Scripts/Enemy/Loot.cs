using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] float totalChance = 1.0f;
    float noLootChance;
    [SerializeField] List<Item> lootItems = new List<Item>();

    void Start()
    {
        
    }
}
