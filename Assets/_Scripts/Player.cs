using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameManager.GameState;

public class Player : MonoBehaviour
{
    Vector2 inputVector;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float launchDuration = 3f;
    [SerializeField] float crashDuration = 5f;
    [SerializeField] float landedSize = 0.5f;
    [SerializeField] float shadowStartOffset = -0.05f;
    [SerializeField] float shadowEndOffset = -0.5f;
    [SerializeField] Transform plane;
    [SerializeField] Transform shadow;
    
    //plane's width and height
    float planeWidth;
    float PlaneLength;

    Vector2 shadowPos;
    Vector2 shadowTemp;
    Vector2 planePos;
    float launchElapsed;
    float crashElapsed;
    float restartElapsed;

    //plane rotation
    float maxRotate = 15f;
    float rotateSpeed = 10f;

    Vector2 screenSize;

    void Awake()
    {
        GameManager.OnStarting += LaunchPlane;
        GameManager.OnRestarting += RestartPlane;
        GameManager.OnPlaying += ControlPlane;
        GameManager.OnExploding += CrashPlane;
        GameManager.OnExitGameState += OnExitGameState;
        GameManager.OnEnterGameState += OnEnterGameState;
    }

    void OnDestroy()
    {
        GameManager.OnStarting -= LaunchPlane;
        GameManager.OnRestarting -= RestartPlane;
        GameManager.OnPlaying -= ControlPlane;
        GameManager.OnExploding -= CrashPlane;
        GameManager.OnExitGameState -= OnExitGameState;
        GameManager.OnEnterGameState -= OnEnterGameState;
    }

    void Start()
    {
        screenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        var collider2d = GetComponent<Collider2D>();
        planeWidth = collider2d.bounds.size.x * 0.5f;
        PlaneLength = collider2d.bounds.size.y * 0.5f;
        shadowTemp = shadow.position;
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
        shadow.position = new Vector2(shadowTemp.x + offset, shadowTemp.y + offset);

        if (launchElapsed > launchDuration)
        {
            launchElapsed = 0;
            SavePlanePosition();
            GameManager.ChangeGameState(Playing);
        }
    }

    void CrashPlane()
    {
        crashElapsed += Time.deltaTime;

        //decrease plane size (crash effect)
        var size = Mathf.Lerp(1f, landedSize,
            1f - ((crashDuration - crashElapsed) / crashDuration));
        transform.localScale = new Vector2(size, size);

        //relocate plane shadow
        var offset = Mathf.Lerp(shadowStartOffset, shadowEndOffset,
            1f - ((crashDuration - crashElapsed) / crashDuration));
        shadow.position = new Vector2(shadowTemp.x - offset, shadowTemp.y - offset);

        //rotate plane on Z axis
        plane.transform.Rotate(0, 0, Time.deltaTime * -360);
    }

    void RestartPlane()
    {
        restartElapsed += Time.deltaTime;
        if (restartElapsed >= 3f)
        {
            GameManager.ChangeGameState(Playing);
        }
    }

    void OnEnterGameState(GameManager.GameState state)
    {
        switch (state)
        {
            case Exploding:
                shadowTemp = shadow.position;
                crashElapsed = 0;
                break;
            case Restarting:
                restartElapsed = 0;
                GUIManager.ShowGetReady();
                break;
        }
    }

    void ResetPlanePosition()
    {
        plane.transform.rotation = Quaternion.identity;
        transform.position = planePos;
        shadow.position = shadowPos;
    }

    void SavePlanePosition()
    {
        planePos = transform.position;
        shadowPos = shadow.position;
    }

    void OnExitGameState(GameManager.GameState state)
    {
        if (state == Exploding)
        {
            transform.localScale = Vector2.one;
            ResetPlanePosition();
        }
    }

    void ControlPlane()
    {
        var newPos = (Vector2)transform.position + inputVector * (moveSpeed * Time.deltaTime);

        newPos.x = Mathf.Clamp(newPos.x, -screenSize.x + planeWidth, screenSize.x - planeWidth);
        newPos.y = Mathf.Clamp(newPos.y, -screenSize.y + PlaneLength, screenSize.y - PlaneLength);
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
