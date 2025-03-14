using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Transform World;
    [SerializeField] Transform world;
    public static Action OnStarting;
    public static Action OnRestarting;
    public static Action OnPlaying;
    public static Action OnExploding;
    public static Action OnPaused;
    public static Action<GameState> OnEnterGameState;
    public static Action<GameState> OnExitGameState;
    public enum GameState
    {
        Starting,
        Restarting,
        Playing,
        Exploding,
        Paused,
    }
    public static GameState State = GameState.Starting;

    void Start()
    {
        World = world;
    }

    public static void ChangeGameState(GameState newState)
    {
        OnExitGameState?.Invoke(State);
        State = newState;
        OnEnterGameState?.Invoke(State);
    }

    public static void GameOver()
    {
        Debug.Log("Game Over");
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
        }
    }

}
