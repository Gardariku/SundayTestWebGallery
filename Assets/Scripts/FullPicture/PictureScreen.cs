using Loader;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FullPicture
{
    public class PictureScreen : MonoBehaviour
    {
        [SerializeField] private LoadData _loadData;
        [SerializeField] private RawImage _image;
        [SerializeField] private Texture _defaultImage;

        private void Start()
        {
            _image.texture = _loadData.LoadedTexture ? _loadData.LoadedTexture : _defaultImage;
        }

        public void GoBack()
        {
            _loadData.LoadedTexture = null;
            _loadData.FakeLoad = true;
            _loadData.SceneName = SceneNames.Gallery;
            SceneManager.LoadScene(SceneNames.Loader, LoadSceneMode.Additive);
        }
    }
}