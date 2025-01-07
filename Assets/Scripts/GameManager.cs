using Assets.Scripts;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}

    [Header("UI")]
    [SerializeField] private GameOverPanel _gameOverPanel;

    [Header("Game Area")]
    [SerializeField] private Vector2 _leftUpCorner = new Vector2(-10, 10);
    [SerializeField] private Vector2 _rightDownCorner = new Vector2(10, -10);

    [Header("Game Settings")]
    [SerializeField] private float _gameScale = 0.3f;

    [Header("Events")]
    public UnityEvent OnGameOver;
    public Vector2 LeftUpCorner => _leftUpCorner + _offsetBoundVector;
    public Vector2 RightDownCorner => _rightDownCorner - _offsetBoundVector;
    public float GameScale => _gameScale;

    private Vector2 _offsetBoundVector; // Offset to make the game area a bit bigger for small errors

    private readonly float _boundOffset = 0.5f;

    public bool IsGameRunning { get; set; } = true;

    private void Awake()
    {
        initSingelton();
        initSettings();
    }

    private void initSingelton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void addBoundOffset()
    {
        _offsetBoundVector = new Vector2(_boundOffset, -_boundOffset);

        _leftUpCorner -= _offsetBoundVector;
        _rightDownCorner += _offsetBoundVector;
    }

    private void initSettings()
    {
        addBoundOffset();

        IsGameRunning = false;
    }

    public bool IsInsideGameArea(Vector3 position)
    {
        return position.x >= _leftUpCorner.x && position.x <= _rightDownCorner.x &&
               position.y <= _leftUpCorner.y && position.y >= _rightDownCorner.y;
    }

    public void Snake_OnCollided(eCollosionType collosionType)
    {
        if(collosionType == eCollosionType.Wall || collosionType == eCollosionType.Self)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        OnGameOver?.Invoke();

        _gameOverPanel.gameObject.SetActive(true);
        IsGameRunning = false;
    }

    public void GameOverPanel_OnRestart()
    {
        restartGame();
    }

    private void restartGame()
    {
        IsGameRunning = true;
    }
}
