using Loader;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainPanelView : MonoBehaviour
    {
        [SerializeField] private LoadData _loadData;

        public void OpenGallery()
        {
            _loadData.SceneName = SceneNames.Gallery;
            _loadData.FakeLoad = true;
            SceneManager.LoadSceneAsync(SceneNames.Loader, LoadSceneMode.Additive);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}