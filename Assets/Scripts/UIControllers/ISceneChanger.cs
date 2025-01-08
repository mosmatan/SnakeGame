using UnityEngine.Events;

namespace Assets.Scripts
{
    public interface ISceneChanger
    {
        public UnityEvent OnFadeOutComplete { get; }
        void ChangeTo(string sceneName);
        void StartScene();
    }
}
