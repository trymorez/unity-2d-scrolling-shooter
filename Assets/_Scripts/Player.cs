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
    
    //plane's width and height
    float width;
    float height;

    Vector2 shadowPos;
    float launchElapsed;

    //plane rotation
    float maxRotate = 15f;
    float rotateSpeed = 10f;

    Vector2 screenSize;

    void Awake()
    {
        GameManager.OnStarting += LaunchPlane;
        GameManager.OnPlaying += ControlPlane;
    }

    void OnDestroy()
    {
        GameManager.OnStarting -= LaunchPlane;
        GameManager.OnPlaying -= ControlPlane;
    }

    void Start()
    {
        screenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        var collider2d = GetComponent<Collider2D>();
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
