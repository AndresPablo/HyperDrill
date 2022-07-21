using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region SINGLETON
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion SINGLETON

    #region VARS
    public Player player;
    [SerializeField] Screens_UI screens;
    [Space]
    [SerializeField] EntitySpawner spawner;

    public int totalScore;
    public int newScore;
    public int lastScore;
    public int highScore;
    public int combo;
    #endregion VARS

    #region EVENTS
    public delegate void EmptyVoidDelegate();
    public delegate void EmptyIntDelegate(int amount);
    public static event EmptyIntDelegate OnChangeScore;
    public static event EmptyVoidDelegate OnGameStart;
    public static event EmptyVoidDelegate OnGameRestart;
    public static event EmptyVoidDelegate OnPlayerDeath;
    public static event EmptyVoidDelegate OnGameover;
    public static event EmptyIntDelegate OnTotalScoreChange;
    public static event EmptyIntDelegate OnScoreAdd;
    public static event EmptyIntDelegate OnComboChange;
    #endregion EVENTS


    void Start()
    {
        Player.OnPlayerStop += IncreaseTotalScore;
    }
    
    public void GameStart()
    {
        player.transform.position = new Vector2(0,0);
        highScore = PlayerPrefs.GetInt("HighScore");    // TODO Debug only
        player.enabled = true;
        player.RefuelFull();
        ResetScore();
        ResetCombo();
        if(OnGameStart != null)
            OnGameStart();
    }

    public void ResetCombo(){
        combo = 0;

        if(OnComboChange != null)
            OnComboChange(combo);
    }
    
    public void AddNewScore(int amount)
    {
        combo++;

        newScore += amount * combo;

        if(OnScoreAdd != null)
            OnScoreAdd(newScore);
        
        if(OnComboChange != null)
            OnComboChange(combo);
    }

    public void IncreaseTotalScore(){
        totalScore += newScore;

        if(OnChangeScore != null)
            OnChangeScore(totalScore);
        
        newScore = 0;
        ResetCombo();
    }

    public void ResetScore()
    {
        totalScore = 0;
        if(OnChangeScore != null)
            OnChangeScore(totalScore); 
    }

    public void OutOfFuel(){
        GameOver();
    }

    public void KillPlayer(){
        GameOver();
        player.Kill();
    }
    
    public void GameOver(){
        player.enabled = false;
        if(totalScore > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", totalScore);
            screens.OpenGameOverScreen(totalScore, true);
        }else{
            screens.OpenGameOverScreen(totalScore, false);
        }

        if(OnGameover != null)
            OnGameover();
    }
    
 }