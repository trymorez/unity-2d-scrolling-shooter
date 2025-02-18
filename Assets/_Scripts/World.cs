using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 1f;
    [SerializeField] GroundTile[] groundTile;
    [SerializeField] int groundTileIndex;
    float nextCheckPointY = 0f;
    const float TILE_ORIGIN_Y = 10f;

    void Awake()
    {
        GameManager.OnStartingGame += ScrollMap;
        GameManager.OnPlayingGame += ScrollMap;
    }

    void OnDestroy()
    {
        GameManager.OnStartingGame -= ScrollMap;
        GameManager.OnPlayingGame -= ScrollMap;
    }

    void Update()
    {
        WorldProgress();
    }

    void WorldProgress()
    {
        var pos = transform.position;

        if (pos.y <= nextCheckPointY)
        {
            //finetune Y position of tileset to prevent artifact
            pos.y = nextCheckPointY;
            transform.position = pos;
            nextCheckPointY -= 10f;

            SpawnNextTile();
        }
    }

    void SpawnNextTile()
    {

        var nextTile = Instantiate(groundTile[groundTileIndex], transform);
        nextTile.transform.position = new Vector3(0, TILE_ORIGIN_Y, 0);
        groundTileIndex = (groundTileIndex + 1) % groundTile.Length;

        SeparateAllEnemyFromTile(nextTile);
    }

    void SeparateAllEnemyFromTile(GroundTile nextTile)
    {
        var tank = new List<Transform>();

        foreach (Transform child in nextTile.transform)
        {
            if (child.CompareTag("Enemy"))
            {
                tank.Add(child);
            }
        }
        foreach (Transform child in tank)
        {
            child.SetParent(transform);
        }
    }

    void ScrollMap()
    {
        var posY = transform.position.y;
        transform.position = new Vector3(0, posY -= scrollSpeed * Time.deltaTime, 0);
    }
}
