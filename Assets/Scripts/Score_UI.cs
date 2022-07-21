using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Score_UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalScoreLabel;
    [SerializeField] TextMeshProUGUI newScoreLabel;
    [SerializeField] TextMeshProUGUI comboLabel;
    [SerializeField] float fadeSpeed = .1f;
    GameManager gm;
    bool fadeOutMode;

    void Start()
    {
        gm = GameManager.instance;
        GameManager.OnChangeScore += UpdateTotalScore;
        GameManager.OnScoreAdd += UpdateNewScore;
        GameManager.OnComboChange += UpdateCombo;
        GameManager.OnGameStart += HideScore;
        Player.OnPlayerStop += BeginFade;
        totalScoreLabel.text = "000000";
        HideScore();
    }

    void HideScore()
    {
        newScoreLabel.text = "";
        comboLabel.text = "";
    }

    void BeginFade()
    {
        fadeOutMode = true;
    }

    void Update()
    {
        if(fadeOutMode)
        {
            Color newScoreColor = newScoreLabel.color;
            Color comboLabelColor = comboLabel.color;
            comboLabelColor.a -= fadeSpeed * Time.deltaTime;
            newScoreColor.a -= fadeSpeed* Time.deltaTime;
            newScoreLabel.color = newScoreColor;
            comboLabel.color = comboLabelColor;

            if(newScoreColor.a <= 0)
            {
                fadeOutMode = false;
            }
        }
    }

    void UpdateTotalScore(int amount)
    {
        totalScoreLabel.text = amount.ToString("000000");
    }

    void UpdateNewScore(int amount)
    {
        Color newScoreColor = newScoreLabel.color;
        newScoreColor.a = 1f;
        newScoreLabel.color = newScoreColor;

        newScoreLabel.text = "+" + amount;
    }

    void UpdateCombo(int amount)
    {
        Color comboLabelColor = comboLabel.color;
        comboLabelColor.a = 1f;
        comboLabel.color = comboLabelColor;

        if(amount > 1)
            comboLabel.text = "x" + amount;
    }
}
