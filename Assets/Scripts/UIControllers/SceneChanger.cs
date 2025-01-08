using UnityEngine;
using Assets.Scripts;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
public class SceneChanger : MonoBehaviour, ISceneChanger
{
    [Header("Refernces")]
    [SerializeField] private Image _fadePanel;

    [Header("Fade Settings")]
    [SerializeField] private float _fadeDuration = 1.0f;
    [SerializeField] private float _fadeDelay = 0.5f;
    [SerializeField] private float _fadeTargetAlpha = 1.0f;
    [SerializeField] private float _fadeStartAlpha = 0.0f;

    [Header("Events")]
    [SerializeField] public UnityEvent OnFadeOutComplete;

    private bool _isFading = false;
    private float _alpha = 0.0f;
    private string _sceneName;

    UnityEvent ISceneChanger.OnFadeOutComplete => OnFadeOutComplete;

    public void ChangeTo(string sceneName)
    {
        if(_isFading)
        {
            return;
        }

        _isFading = true;
        _sceneName = sceneName;

        fadeIn();
    }

    public void StartScene()
    {
        if (_isFading)
        {
            return;
        }

        _isFading = true;

        fadeOut();
    }

    private void fadeOut()
    {
        _alpha = _fadeTargetAlpha;

        _fadePanel.color = new Color(_fadePanel.color.r, _fadePanel.color.g, _fadePanel.color.b, _alpha);

        StartCoroutine(startFadeOutRoutine());
    }

    private IEnumerator startFadeOutRoutine()
    {
        yield return new WaitForSeconds(_fadeDelay);

        StartCoroutine(fadeOutRoutine());
    }

    private IEnumerator fadeOutRoutine()
    {
        while (_alpha > _fadeStartAlpha)
        {
            _alpha -= Time.deltaTime / _fadeDuration;
            _fadePanel.color = new Color(_fadePanel.color.r, _fadePanel.color.g, _fadePanel.color.b, _alpha);
            yield return null;
        }

        OnFadeOutComplete?.Invoke();

        _isFading = false;
        gameObject.SetActive(false);
    }

    private void fadeIn()
    {
        _alpha = _fadeStartAlpha;

        StartCoroutine(fadeInRoutine());
    }

    private IEnumerator fadeInRoutine()
    {
        while(_alpha < _fadeTargetAlpha)
        {
            _alpha += Time.deltaTime / _fadeDuration;
            _fadePanel.color = new Color(_fadePanel.color.r, _fadePanel.color.g, _fadePanel.color.b, _alpha);
            yield return null;
        }

        StartCoroutine(changeSceneRoutine());
    }

    private IEnumerator changeSceneRoutine()
    {
        yield return new WaitForSeconds(_fadeDelay);

        SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);
    }
}
