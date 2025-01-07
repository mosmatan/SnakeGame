using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _bestScoreText;

    private readonly string _hideString = "";

    private int _score = 0;
    private int _bestScore = 0;

    public int Score => _score;
    public int BestScore => _bestScore;

    public void Snake_OnCollided(eCollosionType collosionType)
    {
        if(collosionType == eCollosionType.Food)
        {
            increaseScore();
        }
    }

    private void increaseScore()
    {
        _score++;

        if (_score > _bestScore)
        {
            _bestScore = _score;
        }

        showScores();
    }

    public void GameManager_OnGameOver()
    {
        hideScores();
    }

    public void GameOverPanel_OnRestart()
    {
        _score = 0;

        showScores();
    }

    private void hideScores()
    {
        _scoreText.text = _hideString;
        _bestScoreText.text = _hideString;
    }

    private void showScores()
    {
        _scoreText.text = $"Score: {_score}";
        _bestScoreText.text = $"Best Score: {_bestScore}";
    }
    
}
