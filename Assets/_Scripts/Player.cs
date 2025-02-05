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
    [SerializeField] Transform plane;
    Vector2 shadowPos;
    float launchElapsed;


    void Awake()
    {
        GameManager.OnStartingGame += LaunchingPlane;
        GameManager.OnPlayingGame += ControlPlane;
    }

    void OnDestroy()
    {
        GameManager.OnStartingGame -= LaunchingPlane;
        GameManager.OnPlayingGame -= ControlPlane;
    }

    void Start()
    {
        shadowPos = shadow.position;
        
    }

    Vector2 ls;
    void LaunchingPlane()
    {
        launchElapsed += Time.deltaTime;

        //plane size
        var size = Mathf.Lerp(landedSize, 1f, 1f - ((launchDuration - launchElapsed) / launchDuration));
        transform.localScale = new Vector2 (size, size);

        //plane shadow
        var offset = Mathf.Lerp(shadowStartOffset, shadowEndOffset, 1f - ((launchDuration - launchElapsed) / launchDuration));
        
        shadow.position = new Vector3(shadowPos.x + offset, shadowPos.y + offset, 0);

        if (launchElapsed > launchDuration)
        {
            ls = transform.localScale;
            GameManager.ChangeGameState(GameManager.GameState.Playing);
        }
    }

    void ControlPlane()
    {
        transform.Translate(inputVector * (moveSpeed * Time.deltaTime));

        float rotationAmount = inputVector.x * 10f;

        plane.transform.rotation = Quaternion.Euler(0, 0, -rotationAmount);
        shadow.transform.rotation = Quaternion.Euler(0, 0, -rotationAmount);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }
}
