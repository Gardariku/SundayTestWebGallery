using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Gallery
{
    [RequireComponent(typeof(RawImage))]
    public class CellView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private ContentLoader _contentLoader;
        private RawImage _image;
        [field: SerializeField] public int Index { get; private set; }
        [SerializeField] private bool _isLoaded;
        private ScrollController _controller;

        private void Awake()
        {
            _image = GetComponent<RawImage>();
            _contentLoader.LoadedImage += OnImageLoaded;
        }

        private void OnImageLoaded(Texture texture)
        {
            _isLoaded = true;
            _image.texture = texture;
            _controller.AddTexture(Index, texture);
        }

        public void Init(int index, ScrollController controller)
        {
            Index = index;
            _controller = controller;
            _image.texture = controller.DefaultSprite.texture;
            _contentLoader.StartLoad(index);
        }

        public void Reload(Texture texture, int index)
        {
            _isLoaded = true;
            _image.texture = texture;
            Index = index;
        }
        public void Reload(int index)
        {
            _isLoaded = false;
            Index = index;
            _image.texture = _controller.DefaultSprite.texture;
            _contentLoader.StartLoad(index);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isLoaded)
                _controller.OpenFullPicture(Index);
        }
    }
}