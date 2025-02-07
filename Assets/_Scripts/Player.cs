using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Vector2 inputVector;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float launchDuration = 3f;
    [SerializeField] float landedSize = 0.5f;
    [SerializeField] float shadowStartOffset = -0.05f;
    [SerializeField] float shadowEndOffset = -0.5f;
    [SerializeField] Transform shadow;
    Vector2 shadowPosition;
    [SerializeField] Transform plane;
    [SerializeField] Gun gun;
    [SerializeField] float fireRate = 0.3f;
    float nextFireTime;
    float launchElapsed;

    public static Action OnGunFire;

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
        nextFireTime = Time.time + fireRate;
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
        transform.Translate(inputVector * (moveSpeed * Time.deltaTime));

        //horizontal rotation for plane and shadow
        float rotateAmount = inputVector.x * 10f;
        plane.transform.rotation = Quaternion.Euler(0, 0, -rotateAmount);
        shadow.transform.rotation = Quaternion.Euler(0, 0, -rotateAmount);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (GameManager.State != GameManager.GameState.Playing)
        {
            return;
        }


        if (context.performed && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            OnGunFire?.Invoke();
        }
    }
}
