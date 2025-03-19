using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static Transform World;
    [SerializeField] Transform world;
    [SerializeField] GameObject gameOver;
    PlayerInput gameOverInput;

    public static Action OnStarting;
    public static Action OnRestarting;
    public static Action OnPlaying;
    public static Action OnExploding;
    public static Action OnPaused;
    public static Action OnGameOver;
    public static Action<GameState> OnEnterGameState;
    public static Action<GameState> OnExitGameState;
    public enum GameState
    {
        Starting,
        Restarting,
        Playing,
        Exploding,
        Paused,
        GameOver,
    }
    public static GameState State = GameState.Starting;

    static GameManager instance;

    void Awake()
    {
        gameOverInput = GetComponent<PlayerInput>();
        OnGameOver += HandleGameOver;
    }

    void OnDestroy()
    {
        OnGameOver -= HandleGameOver;
    }

    void Start()
    {
        World = world;
        instance = this;
    }

    public static void ChangeGameState(GameState newState)
    {
        OnExitGameState?.Invoke(State);
        State = newState;
        OnEnterGameState?.Invoke(State);
    }

    public static void GameOver()
    {
        instance.gameOver.SetActive(true);
        instance.gameOverInput.enabled = true;
        ChangeGameState(GameState.GameOver);
    }

    public void OnAnyKey(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            State = GameState.Starting;
            SceneManager.LoadScene(0);
        }
    }

    void HandleGameOver()
    {

    }

    void Update()
    {
        switch (State)
        {
            case GameState.Starting:
                OnStarting?.Invoke();
                break;
            case GameState.Restarting:
                OnRestarting?.Invoke();
                OnPlaying?.Invoke();
                break;
            case GameState.Playing:
                OnPlaying?.Invoke();
                break;
            case GameState.Exploding:
                OnExploding?.Invoke();
                break;
            case GameState.Paused:
                break;
            case GameState.GameOver:
                OnGameOver?.Invoke();
                break;
        }
    }

}
