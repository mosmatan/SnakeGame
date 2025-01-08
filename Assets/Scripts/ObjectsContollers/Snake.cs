using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using UnityEngine.Events;
using System.Linq;

public class Snake : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _head;
    [SerializeField] private GameObject _tail;
    [SerializeField] private AudioSource _eatAudio;

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
    private Vector3 _prevDirection = Vector3.up;
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
        _head.transform.position = _initialPosition;
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

        moveTail(_head.transform.position - 2 * Vector3.up * _initialBodyParts * _stepSize, Quaternion.Euler(0, 0, 0));
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
            _prevDirection = _direction;
            _direction = newDirectionV;
        }

        moveSnake();
    }

    private void moveSnake()
    {
        var newHeadPosition = _head.transform.position + _direction * _stepSize;

        Moved?.Invoke(newHeadPosition);

        rotateHead();

        // Check if snake is inside game area
        if (isCollidingSelfOrWall(newHeadPosition))
        {
            return;
        }

        moveBodyPart();

        _head.transform.position = newHeadPosition;
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

    private void rotateBodyPart(GameObject bodyPart)
    {
        if (_prevDirection == Vector3.up)
        {
            bodyPart.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (_prevDirection == Vector3.down)
        {
            bodyPart.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (_prevDirection == Vector3.left)
        {
            bodyPart.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (_prevDirection == Vector3.right)
        {
            bodyPart.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
    }

    private void moveBodyPart()
    {
        var lastPart = _bodyParts.Last;

        var prevLastPart = lastPart.Previous == null ? lastPart : lastPart.Previous;

        var newTailPosition = lastPart.Value.transform.position;

        lastPart.Value.transform.position = _head.transform.position;
        rotateBodyPart(lastPart.Value);

        var newTailRotation = prevLastPart.Value.transform.rotation;

        moveTail(newTailPosition, newTailRotation);

        _bodyParts.RemoveLast();
        _bodyParts.AddFirst(lastPart);
    }

    private void moveTail(Vector3 position, Quaternion rotation)
    {
        _tail.transform.position = position;
        _tail.transform.rotation = rotation;
    }

    private bool isCollidingWithBody(Vector3 newHeadPosition)
    {
        const float minDistance = 0.1f;

        // Check if snake collided with itself
        var collided = _bodyParts.Any(bodyPart => Vector2.Distance(newHeadPosition, bodyPart.transform.position) < minDistance);

        if(collided)
        {
            OnCollided?.Invoke(eCollosionType.Self);
        }

        return collided;
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

        _eatAudio.Play();

        var newBodyPart = Instantiate(_bodyPartPrefab,
                                      _bodyParts.Last.Value.transform.position,
                                      _bodyParts.Last.Value.transform.rotation,
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

        initSnakeBody();
    }
}
