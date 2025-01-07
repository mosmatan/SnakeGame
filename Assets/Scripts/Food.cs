using UnityEngine;
using UnityEngine.Events;

public class Food : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnEaten;

    private float _gameScale = 0.3f;

    private int _leftBound = -9;
    private int _rightBound = 9;
    private int _topBound = 9;
    private int _bottomBound = -9;

    private void Start()
    {
        initSettings();
    }

    private void initSettings()
    {
        _gameScale = GameManager.Instance.GameScale;

        _leftBound = (int)GameManager.Instance.LeftUpCorner.x + 1;
        _rightBound = (int)GameManager.Instance.RightDownCorner.x - 1;
        _topBound = (int)GameManager.Instance.LeftUpCorner.y - 1;
        _bottomBound = (int)GameManager.Instance.RightDownCorner.y + 1;
    }

    public void Snake_Moved(Vector3 headPosition)
    {
        // Check if the snake head is on the same position as the food
        if (Vector3.Distance(headPosition, transform.position) < 0.1f)
        {
            OnEaten?.Invoke();
            SetNewRandomPosition();
        }
    }

    public void GameOverPanel_OnRestart()
    {
        SetNewRandomPosition();
    }

    public void SetNewRandomPosition()
    {
        transform.position = new Vector3(Random.Range(_leftBound, _rightBound) * _gameScale,
                                         Random.Range(_bottomBound, _topBound) * _gameScale, 0);
    }

}
