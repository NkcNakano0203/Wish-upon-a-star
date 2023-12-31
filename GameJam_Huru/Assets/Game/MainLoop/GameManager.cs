using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Ready,
        Game,
        Finish
    }
    private Subject<int> readyCountDownSubject = new();
    public IObservable<int> ReadyCountDownSubject => readyCountDownSubject;

    private Subject<int> gameCountDownSubject = new();
    public IObservable<int> GameCountDownSubject => gameCountDownSubject;

    private Subject<bool> gameEndSubject = new();
    public IObservable<bool> GameEndSubject => gameEndSubject;

    private GameState currentGameState = GameState.Ready;
    public GameState GetCurrentGameState => currentGameState;
    bool isCounting = false;
    [SerializeField]
    StarFactory starFactory;

    void Start()
    {

    }

    void Update()
    {
        switch (currentGameState)
        {
            case GameState.Ready:
                if (isCounting) return;
                isCounting = true;
                StartCoroutine(CountDownCol(3));
                break;
            case GameState.Game:
                if (isCounting) return;
                isCounting = true;
                StartCoroutine(CountDownCol(30));
                break;
            case GameState.Finish:
                gameEndSubject.OnNext(true);
                break;
            default:
                if (isCounting) return;
                isCounting = true;
                StartCoroutine(CountDownCol(500));
                break;
        }
    }

    IEnumerator CountDownCol(int count)
    {
        for (int i = count; i > 0; i--)
        {
            if (currentGameState == GameState.Ready)
            {
                readyCountDownSubject.OnNext(i);
            }
            else if (currentGameState == GameState.Game)
            {
                gameCountDownSubject.OnNext(i);
            }
            yield return new WaitForSeconds(1);
        }

        if (currentGameState == GameState.Ready)
        {
            currentGameState = GameState.Game;
        }
        else if (currentGameState == GameState.Game)
        {
            currentGameState = GameState.Finish;
        }
        isCounting = false;
    }
}