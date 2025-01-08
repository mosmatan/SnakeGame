using UnityEngine;
public class PlayField : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _playField;

    private void Start()
    {
        initField();
    }

    private void initField()
    {
        _playField.transform.localScale = new Vector3(Mathf.Abs(GameManager.Instance.RightDownCorner.x - GameManager.Instance.LeftUpCorner.x + 1),
                                                     Mathf.Abs(GameManager.Instance.LeftUpCorner.y - GameManager.Instance.RightDownCorner.y + 1),
                                                     1);
    }
}
