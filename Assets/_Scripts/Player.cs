using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Vector2 inputVector;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float launchDuration = 3f;
    [SerializeField] float landedSize = 0.5f;
    [SerializeField] float shadowStartOffset = -0.05f;
    [SerializeField] float shadowEndOffset = -0.5f;
    [SerializeField] Transform shadow;
    [SerializeField] Transform plane;
    [SerializeField] SpriteRenderer sprite;
    Vector2 shadowPosition;
    float launchElapsed;

    Vector2 screenSize;

    void Awake()
    {
        screenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        GameManager.OnStartingGame += LaunchPlane;
        GameManager.OnPlayingGame += ControlPlane;
    }

    void OnDestroy()
    {
        GameManager.OnStartingGame -= LaunchPlane;
        GameManager.OnPlayingGame -= ControlPlane;
    }

    void Start()
    {
        shadowPosition = shadow.position;
    }

    void LaunchPlane()
    {
        launchElapsed += Time.deltaTime;

        //increase plane size (take-off effect)
        var size = Mathf.Lerp(landedSize, 1f, 
            1f - ((launchDuration - launchElapsed) / launchDuration));
        transform.localScale = new Vector2 (size, size);

        //relocate plane shadow
        var offset = Mathf.Lerp(shadowStartOffset, shadowEndOffset, 
            1f - ((launchDuration - launchElapsed) / launchDuration));
        shadow.position = new Vector2(shadowPosition.x + offset, shadowPosition.y + offset);

        if (launchElapsed > launchDuration)
        {
            GameManager.ChangeGameState(GameManager.GameState.Playing);
        }
    }

    void ControlPlane()
    {
        var newPos = (Vector2)transform.position + inputVector * (moveSpeed * Time.deltaTime);
        var spriteSizeX = sprite.bounds.size.x * 0.5f - 0.2f;
        var spriteSizeY = sprite.bounds.size.y * 0.5f - 0.3f;
        newPos.x = Mathf.Clamp(newPos.x, -screenSize.x + spriteSizeX, screenSize.x - spriteSizeX);
        newPos.y = Mathf.Clamp(newPos.y, -screenSize.y + spriteSizeY, screenSize.y - spriteSizeY);
        transform.position = newPos;


        //horizontal rotation for plane and shadow
        float rotate = inputVector.x * 10f;
        plane.transform.rotation = Quaternion.Euler(0, 0, -rotate);
        shadow.transform.rotation = Quaternion.Euler(0, 0, -rotate);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }
}
