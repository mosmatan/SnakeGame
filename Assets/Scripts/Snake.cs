using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using UnityEngine.Events;

public class Snake : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _head;

    [Header("Snake Settings")]
    [SerializeField] private GameObject _bodyPartPrefab;
    [SerializeField] private Vector3 _initialPosition = Vector3.zero;
    [SerializeField] private int _initialBodyParts = 3;
    [SerializeField] private float _stepSize = 1f;
    [SerializeField] private float _snakeScale = 0.3f;

    [Header("Events")]
    public UnityEvent<Vector3> Moved;
    public UnityEvent<eCollosionType> OnCollided;

    private Vector3 _direction = Vector3.up;
    private LinkedList<GameObject> _bodyParts = new LinkedList<GameObject>();

    private void Awake()
    {
        initSnakeSettings();
        initSnakeBody();
    }

    private void initSnakeSettings()
    {
        _snakeScale = GameManager.Instance.GameScale;
    }

    private void initSnakeBody()
    {
        // Set head
        _direction = Vector3.up;
        rotateHead();

        // Set body parts
        for (int i = 1; i <= _initialBodyParts; i++)
        {
            var newBodyPart = Instantiate(_bodyPartPrefab,
                                          _head.transform.position - Vector3.up * i * _stepSize,
                                          Quaternion.identity,
                                          this.transform);

            _bodyParts.AddLast(newBodyPart);
        }
    }

    public void Move(eDirections newDir)
    {
        var newDirectionV = Vector3.zero;

        switch (newDir)
        {
            case eDirections.Up:
                newDirectionV = Vector3.up;
                break;
            case eDirections.Down:
                newDirectionV = Vector3.down;
                break;
            case eDirections.Left:
                newDirectionV = Vector3.left;
                break;
            case eDirections.Right:
                newDirectionV = Vector3.right;
                break;
        }

        if (newDirectionV != -_direction) // Prevent snake from going backwards
        {
            _direction = newDirectionV;
        }

        moveSnake();
    }

    private void moveSnake()
    {
        var newHeadPosition = _head.transform.position + _direction * _stepSize;

        rotateHead();

        // Check if snake is inside game area
        if (isCollidingSelfOrWall(newHeadPosition))
        {
            return;
        }

        // Move last body part to head position
        var lastPart = _bodyParts.Last;
        lastPart.Value.transform.position = _head.transform.position;
        _bodyParts.RemoveLast();
        _bodyParts.AddFirst(lastPart);

        // Move head to new position
        _head.transform.position = newHeadPosition;

        Moved?.Invoke(_head.transform.position);
    }

    private void rotateHead()
    {
        if (_direction == Vector3.up)
        {
            _head.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if(_direction == Vector3.down)
        {
            _head.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if(_direction == Vector3.left)
        {
            _head.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if(_direction == Vector3.right)
        {
            _head.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
    }

    private bool isCollidingWithBody(Vector3 newHeadPosition)
    {
        const float minDistance = 0.1f;

        foreach (var bodyPart in _bodyParts)
        {
            // Skip last body part because it will be moved
            if (bodyPart == _bodyParts.Last.Value)
            {
                continue;
            }

            var dis = Vector2.Distance(newHeadPosition, bodyPart.transform.position);

            if (dis < minDistance)
            {
                OnCollided?.Invoke(eCollosionType.Self);
                return true;
            }
        }

        return false;
    }

    private bool isCollidingSelfOrWall(Vector3 newHeadPosition)
    {
        var isCollidedWithWall = !GameManager.Instance.IsInsideGameArea(newHeadPosition / _snakeScale);

        if (isCollidedWithWall)
        {
            OnCollided?.Invoke(eCollosionType.Wall);
        }

        return isCollidedWithWall || isCollidingWithBody(newHeadPosition);
    }

    public void Food_OnEaten()
    {
        grow();
    }

    private void grow()
    {
        OnCollided?.Invoke(eCollosionType.Food);

        var newBodyPart = Instantiate(_bodyPartPrefab,
                                      _bodyParts.Last.Value.transform.position,
                                      Quaternion.identity,
                                      this.transform);

        _bodyParts.AddLast(newBodyPart);
    }

    public void GameManager_OnGameOver()
    {
       resetSnake();
    }

    private void resetSnake()
    {
        
        foreach (var bodyPart in _bodyParts)
        {
            Destroy(bodyPart);
        }

        _bodyParts.Clear();

        _head.transform.position = _initialPosition;

        initSnakeBody();
    }


}
