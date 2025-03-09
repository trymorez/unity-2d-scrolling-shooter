using System;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 1f;
    [SerializeField] GroundTile[] groundTile;
    [SerializeField] int groundTileIndex;
    float nextCheckPointY = 0f;
    float tileOriginY = 10f;
    public static Transform worldTransform;

    public static Action<float> OnScrollMap;

    void Awake()
    {
        worldTransform = transform;
        GameManager.OnStarting += ScrollMap;
        GameManager.OnPlaying += ScrollMap;
    }

    void OnDestroy()
    {
        GameManager.OnStarting -= ScrollMap;
        GameManager.OnPlaying -= ScrollMap;
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
        nextTile.transform.position = new Vector3(0, tileOriginY, 0);
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
        OnScrollMap?.Invoke(scrollSpeed * Time.deltaTime);
    }
}
