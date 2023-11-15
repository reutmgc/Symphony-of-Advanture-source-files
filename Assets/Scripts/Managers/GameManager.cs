using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour, IRegistrableService
{
    public  UnityEvent gameOverEvent;
    public  UnityEvent startGameEvent;
    string playerName = "Reut";
    public static bool paused = false;

    private void Awake()
    {     
        ServiceLocator.Instance.Register<GameManager>(this);
    }
    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame()
    {
        SetGameSessionIndex(GetGameSessionIndex()+1);
        startGameEvent.Invoke();
    }

    public void UnpauseGame()
    {
        if (paused)
        {
            Debug.Log("Game Unpaused");
            Time.timeScale = 1;
            ServiceLocator.Instance.Get<InputManager>().ActionMapGoBack();
            paused = false;
        }
    }
    public void PauseGame()
    {
        if (!paused)
        {
            Debug.Log("Game paused");
            Time.timeScale = 0;
            ServiceLocator.Instance.Get<InputManager>().ActivatePausedUIMap();
            paused = true;
        }
    }
    public void ExitGame()
    {
        GameOver();
        Application.Quit();
        Debug.Log("exiting game");

    }

    public void GameOver()
    {
        ExportManager exportManager = ServiceLocator.Instance.Get<ExportManager>();
        exportManager.SendCSVByEmail();
        gameOverEvent.Invoke();
    }

    #region DATA 
    public int GetGameSessionIndex()
    {
        return PlayerPrefs.GetInt("GameSessionIndex");
    }
    void SetGameSessionIndex(int i)
    {
        PlayerPrefs.SetInt("GameSessionIndex",i);
    }
    public void SetPlayerName(string n)
    {
        playerName = n;
    }
    public string GetPlayerName()
    {
        return playerName;
    }
    public string  GetPlayerID()
    {
        return SystemInfo.deviceUniqueIdentifier.ToString();
    }
    public string GetConfigurationFileID()
    {
        return "00001";
    }
    #endregion
}
