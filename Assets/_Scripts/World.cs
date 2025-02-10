using UnityEngine;
using UnityEngine.UIElements;

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

    private void WorldProgress()
    {
        var pos = transform.position;

        if (pos.y <= nextCheckPointY)
        {
            Debug.Log("next tile");
            //fine-tuning Y position of tileset to prevent seam crack
            pos.y = nextCheckPointY;
            transform.position = pos;

            nextCheckPointY -= 10f;

            var nextTile = Instantiate(groundTile[groundTileIndex], transform);
            nextTile.transform.position = new Vector3(0, TILE_ORIGIN_Y, 0);
            groundTileIndex = (groundTileIndex + 1) % groundTile.Length;
        }
    }

    void ScrollMap()
    {
        var posY = transform.position.y;
        transform.position = new Vector3(0, posY -= scrollSpeed * Time.deltaTime, 0);
    }
}
