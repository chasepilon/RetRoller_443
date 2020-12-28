using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RetroAesthetics.Demos
{
    public class InitializeGame : MonoBehaviour
    {
        public SceneField loadingScene;

        public bool fadeInMenu = true;
        public bool fadeOutMenu = true;
        public float delay = 5.0f;

        private string levelScene = "Title";
        private RetroCameraEffect _cameraEffect;
        private AsyncOperation _loadingSceneAsync;


        void Start()
        {
            if (fadeInMenu)
            {
                _cameraEffect = GameObject.FindObjectOfType<RetroCameraEffect>();
                if (_cameraEffect != null)
                {
                    _cameraEffect.FadeIn(3);
                }
            }

            if (levelScene != null)
            {
                if (_cameraEffect != null)
                {
                    if (loadingScene != null)
                    {
                        _loadingSceneAsync = SceneManager.LoadSceneAsync(loadingScene);
                        if (_loadingSceneAsync == null)
                        {
                            Debug.LogWarning(string.Format(
                                "Please add scene `{0}` to the built scenes in the Build Settings.",
                                loadingScene.SceneName));
                            return;
                        }
                        _loadingSceneAsync.allowSceneActivation = false;
                    }

                    _cameraEffect.FadeOut(0.5f, LoadNextScene);
                }
                //else
                //{
                //    LoadNextScene();
                //}
            }
            else
            {
                Debug.LogWarning("Level scene is not set.");
            }

            PlayerPrefs.SetString("LastScene", string.Empty);
            PlayerPrefs.SetInt("CassettesUnlocked", 0);
        }

        private void Update()
        {
            if (delay <= 0.0f)
                LoadNextScene();
            else
                delay = delay - Time.deltaTime;
        }

        private void LoadNextScene()
        {
            if (_loadingSceneAsync != null)
            {
                _loadingSceneAsync.allowSceneActivation = true;
            }
            SceneManager.LoadSceneAsync(levelScene);
        }
    }
}
