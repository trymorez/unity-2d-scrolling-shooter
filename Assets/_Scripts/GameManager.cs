using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Transform World;
    [SerializeField] Transform world;
    public static Action OnPlayingGame;
    public static Action OnStartingGame;
    public static Action<GameState> OnEnterGameState;
    public static Action<GameState> OnExitGameState;
    public enum GameState
    {
        Starting,
        Playing,
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

    void Update()
    {
        switch (State)
        {
            case GameState.Starting:
                OnStartingGame?.Invoke();
                break;
            case GameState.Playing:
                OnPlayingGame?.Invoke();
                break;
            case GameState.Paused:
                break;
        }
    }

}
