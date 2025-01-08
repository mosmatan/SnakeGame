using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ScoreManager _scoreManager;

    [Header("UI")]
    [SerializeField] private Text _titleText;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _bestScoreText;

    [Header("Texts")]
    [SerializeField] private string _gameOverTextString = "Game Over";
    [SerializeField] private string _scoreTextString = "Score:";
    [SerializeField] private string _bestScoreTextString = "Best Score:";

    [Header("Events")]
    public UnityEvent OnRestart;
    public UnityEvent OnHomeScene;

    private readonly string _hideString = "";


    private void OnEnable()
    {
        _titleText.text = _gameOverTextString;
        _scoreText.text = $"{_scoreTextString} {_scoreManager.Score}";
        _bestScoreText.text = $"{_bestScoreTextString} {_scoreManager.BestScore}";
    }


    private void Update()
    {
        restartGame();
    }

    private void restartGame()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnHomeScene?.Invoke();
            return;
        }

        if(Input.anyKeyDown)
        {
            OnRestart?.Invoke();

            this.gameObject.SetActive(false);
        }
    }
}
