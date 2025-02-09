using UnityEngine;
using UnityEngine.UIElements;

public class World : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 1f;
    [SerializeField] GroundTile[] groundTile;
    [SerializeField] int tileIndex;
    float nextCheckPointY = -10f;
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

    //void Start()
    //{
        
    //}


    void Update()
    {
        var pos = transform.position;

        if (pos.y <= nextCheckPointY)
        {
            //fine-tuning Y position to prevent seam effect
            pos.y = nextCheckPointY;
            transform.position = pos;

            nextCheckPointY -= 10f;

            tileIndex = (tileIndex + 1) % groundTile.Length;
            var nextTile = Instantiate(groundTile[tileIndex], transform);
            nextTile.transform.position = new Vector3 (0, TILE_ORIGIN_Y, 0);
        }
    }

    void ScrollMap()
    {
        var posY = transform.position.y;
        transform.position = new Vector3(0, posY -= scrollSpeed * Time.deltaTime, 0);
    }
}
