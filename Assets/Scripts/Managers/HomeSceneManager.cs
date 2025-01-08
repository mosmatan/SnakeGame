using UnityEngine;
using Assets.Scripts;

public class HomeSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject _sceneChanger;

    private void Start()
    {
        _sceneChanger.gameObject.SetActive(true);
        _sceneChanger.GetComponent<ISceneChanger>().StartScene();
    }

    public void StartButton_Click()
    {
        _sceneChanger.gameObject.SetActive(true);
        _sceneChanger.GetComponent<ISceneChanger>().ChangeTo("GameScene");
    }

    public void QuitButton_Click()
    {
        Application.Quit();
    }
}
