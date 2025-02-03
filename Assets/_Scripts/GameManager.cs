using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Action OnUpdatingGame;
    public static Action OnStartingGame;
    public static Action<GameState> OnEnterGameState;
    public static Action<GameState> OnExitGameState;
    public enum GameState
    {
        Starting,
        Playing,
        Paused,
    }
    public GameState State = GameState.Starting;
    
    void ChangeGameState(GameState newState)
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
                OnUpdatingGame?.Invoke();
                break;
            case GameState.Paused:
                break;
        }
    }

}
