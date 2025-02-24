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
    [SerializeField] Transform plane;
    [SerializeField] Transform shadow;
    
    Collider2D collider2d;
    float width;
    float height;

    Vector2 shadowPos;
    float launchElapsed;


    Vector2 screenSize;

    void Awake()
    {
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
        screenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        collider2d = GetComponent<Collider2D>();
        width = collider2d.bounds.size.x * 0.5f;
        height = collider2d.bounds.size.y * 0.5f;
        shadowPos = shadow.position;
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
        shadow.position = new Vector2(shadowPos.x + offset, shadowPos.y + offset);

        if (launchElapsed > launchDuration)
        {
            GameManager.ChangeGameState(GameManager.GameState.Playing);
        }
    }

    void ControlPlane()
    {
        var newPos = (Vector2)transform.position + inputVector * (moveSpeed * Time.deltaTime);

        newPos.x = Mathf.Clamp(newPos.x, -screenSize.x + width, screenSize.x - width);
        newPos.y = Mathf.Clamp(newPos.y, -screenSize.y + height, screenSize.y - height);
        transform.position = newPos;


        //horizontal rotation for plane and shadow
        var maxRotate = 15f;
        var rotateSpeed = 10f;
        var endRotate = inputVector.normalized.x * maxRotate;
        var curRotate = plane.transform.eulerAngles.z;
        var smoothRotate = Mathf.LerpAngle(curRotate, -endRotate, Time.deltaTime * rotateSpeed);

        plane.transform.rotation = Quaternion.Euler(0, 0, smoothRotate);
        shadow.transform.rotation = Quaternion.Euler(0, 0, smoothRotate);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }
}
