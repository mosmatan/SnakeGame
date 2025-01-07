using Assets.Scripts;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Snake _snake;

    [Header("Settings")]
    [SerializeField] private float _stepTimeout = 0.5f;
    [SerializeField] private float _speedUpFactor = 0.95f;

    [Header("Keys")]
    [SerializeField] private KeyCode _up;
    [SerializeField] private KeyCode _down;
    [SerializeField] private KeyCode _left;
    [SerializeField] private KeyCode _right;

    private float _stepTimeoutInitial;
    private float _stepTimer = 0f;
    private eDirections _lastPlayerInput = eDirections.Up;

    private void Awake()
    {
        _stepTimeoutInitial = _stepTimeout;
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.Instance.IsGameRunning)
        {
            return;
        }

        snakeMovement();
    }

    private eDirections getPlayerInput()
    {
        if (Input.GetKeyDown(_up))
        {
            return eDirections.Up;
        }
        else if (Input.GetKeyDown(_down))
        {
            return eDirections.Down;
        }
        else if (Input.GetKeyDown(_left))
        {
            return eDirections.Left;
        }
        else if (Input.GetKeyDown(_right))
        {
            return eDirections.Right;
        }

        return _lastPlayerInput;
    }

    private void snakeMovement()
    {
        var playerInput = getPlayerInput();

        if (playerInput != _lastPlayerInput)
        {
            _lastPlayerInput = playerInput;
        }

        // Move the snake every stepTimeout seconds
        _stepTimer += Time.deltaTime;

        if (_stepTimer >= _stepTimeout)
        {
            _snake.Move(_lastPlayerInput);

            _stepTimer = 0f;
        }
    }

    public void Food_OnEaten()
    {
        speedUp();
    }

    private void speedUp()
    {
        _stepTimeout *= _speedUpFactor;
    }

    public void GameManager_OnGameOver()
    {
        restart();
    }

    private void restart()
    {
        _stepTimeout = _stepTimeoutInitial;
    }
}
