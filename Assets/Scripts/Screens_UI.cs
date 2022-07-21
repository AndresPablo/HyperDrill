using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Screens_UI : MonoBehaviour
{
    [Header("Play")]
    [SerializeField]GameObject playScreen;
    [Header("Menu")]
    [SerializeField]GameObject startMenuScreen;
    [Header("Game Over")]
    [SerializeField]GameObject gameOverScreen;
    [SerializeField]TextMeshProUGUI finalScoreLabel;
    [SerializeField]TextMeshProUGUI highScoreLabel;
    [SerializeField]TextMeshProUGUI newRecordLabel;

    GameManager gm;


    void Start()
    {
        gm = GameManager.instance;
        GameManager.OnGameStart += EnablePlayScreen;
    }

    public void EnablePlayScreen(){
        playScreen.SetActive(true);
    }

    public void OpenMenu(){
        startMenuScreen.SetActive(true);
    }

    public void CloseGameOverScreen(){
        gameOverScreen.SetActive(false);
    }

    public void OpenGameOverScreen(int finalScore, bool newRecord){
        gameOverScreen.SetActive(true);
        playScreen.SetActive(false);
        finalScoreLabel.text = finalScore + "";
        if(newRecord)
        {
            newRecordLabel.gameObject.SetActive(true);
            highScoreLabel.text = finalScore.ToString();
        }else{
            newRecordLabel.gameObject.SetActive(false);
            highScoreLabel.text = PlayerPrefs.GetInt("HighScore").ToString();
        }
            
    }


}
