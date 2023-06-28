using System;
using System.Collections.Generic;
using Loader;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Gallery
{
    public class ScrollController : MonoBehaviour
    {
        [SerializeField] private LoadData _loadData;
        [SerializeField] private GridLayoutGroup _gridLayout;
        [SerializeField] private RectTransform _upperFiller;
        [SerializeField] private RectTransform _bottomFiller;
        // TODO: might use deque for efficiency?
        [SerializeField] private List<CellView> _cells;
        [SerializeField] private CellView _cellPrefab;
        [SerializeField] private Transform _cellParent;
        [SerializeField] private RectTransform _contentTransform;
        [field: SerializeField] public Sprite DefaultSprite { get; private set; }
        [Header("Parameters")]
        // TODO: Download html page at given address and count table rows in it
        [SerializeField] private int _fullLength = 66;
        [SerializeField] private int _currentIndex = 1;
        [SerializeField] private float _padding = 20f;
        [SerializeField] private int _columns = 2;
        [SerializeField] private int _maxVisibleCells;
        
        private float _cellSide;
        private Vector3[] _viewportCorners = new Vector3[4];
        private RectTransform _gridRectTransform;
        [SerializeField] private List<Texture> _loadedImages = new ();

        private void Awake()
        {
            _gridRectTransform = _gridLayout.GetComponent<RectTransform>();
        }

        private void Start()
        {
            CalculateCells();
            _gridLayout.cellSize = new Vector2(_cellSide, _cellSide);
            int hiddenRows = DivRoundUp(_fullLength - _maxVisibleCells, _columns);
            float fillerSize = hiddenRows * _cellSide + (hiddenRows - 1) * _padding;
            _bottomFiller.sizeDelta += new Vector2(0, fillerSize);

            if (_maxVisibleCells > _cells.Count)
            {
                for (int i = _cells.Count; i < _maxVisibleCells; i++)
                {
                    _cells.Add(Instantiate(_cellPrefab, _cellParent));
                }
            }
            else if (_cells.Count > _maxVisibleCells)
            {
                for (int i = _cells.Count - 1; i >= _maxVisibleCells; i++)
                {
                    Destroy(_cells[i]);
                    _cells.RemoveAt(i);
                }
            }

            for (int i = 0; i < _cells.Count; i++)
            {
                _cells[i].Init(i + 1, this);
                _cells[i].gameObject.name = $"Cell {_cells[i].Index}";
            }

            Canvas.ForceUpdateCanvases();
            _contentTransform.GetWorldCorners(_viewportCorners);
        }

        private void Update()
        {
            var gridCorners = new Vector3[4];
            _gridRectTransform.GetWorldCorners(gridCorners);
            
            if (gridCorners[0].y > _viewportCorners[0].y + _padding)
            {
                int newCells = Math.Min(_fullLength - (_currentIndex + _maxVisibleCells), _columns);
                if (newCells <= 0) return;
                
                for (int i = 0; i < _columns; i++)
                {
                    _cells[0].transform.SetAsLastSibling();
                    int cellIndex = _currentIndex + _maxVisibleCells + i;
                    if (_loadedImages.Count >= cellIndex && _loadedImages[cellIndex - 1] != null)
                        _cells[0].Reload(_loadedImages[cellIndex - 1], _currentIndex + _maxVisibleCells + i);
                    else
                        _cells[0].Reload(cellIndex);
            
                    _cells[0].gameObject.SetActive(i <= newCells);
                    _cells.Add(_cells[0]);
                    _cells.RemoveAt(0);
                }
                _currentIndex = _cells[0].Index;
            
                _upperFiller.sizeDelta += new Vector2(0, _cellSide);
                if (_currentIndex / _columns >= 1)
                    _upperFiller.sizeDelta += new Vector2(0, _padding);
                _bottomFiller.sizeDelta = Vector2.Max(Vector2.zero,
                    new Vector2(0, _bottomFiller.sizeDelta.y - (_cellSide + _padding)));
            }
            else if (gridCorners[1].y < _viewportCorners[1].y - _padding)
            {
                int newCells = Math.Clamp(_currentIndex - _columns, 0, _columns);
                if (newCells <= 0) return;
            
                for (int i = 1; i <= _columns; i++)
                {
                    _cells[^1].transform.SetAsFirstSibling();
                    int cellIndex = _currentIndex - i;
                    if (_loadedImages.Count >= cellIndex && _loadedImages[cellIndex - 1] != null)
                        _cells[^1].Reload(_loadedImages[cellIndex - 1], cellIndex);
                    else
                        _cells[^1].Reload(cellIndex);

                    _cells[^1].gameObject.SetActive(true);
                    _cells.Insert(0, _cells[^1]);
                    _cells.RemoveAt(_cells.Count - 1);
                }
                _currentIndex = _cells[0].Index;
            
                _bottomFiller.sizeDelta += new Vector2(0, _cellSide + _padding);
                _upperFiller.sizeDelta = new Vector2(0, Math.Max(_upperFiller.sizeDelta.y - (_cellSide + _padding), 0));
            }
        }

        private void CalculateCells()
        {
            var rect = GetComponent<RectTransform>().rect;
            _cellSide = (rect.width - _padding * (_columns + 2)) / _columns;
            _maxVisibleCells = DivRoundUp(rect.height - _padding * 2, _cellSide) * _columns + _columns;
        }

        private int DivRoundUp(float numerator, float denominator)
        {
            return (int)(numerator / denominator + 0.5f);
        }

        public void AddTexture(int index, Texture texture)
        {
            if (_loadedImages.Count >= index)
            {
                _loadedImages[index - 1] = texture;
                return;
            }
            
            while (_loadedImages.Count < index - 1)
                _loadedImages.Add(null);
            _loadedImages.Add(texture);
        }

        public void OpenFullPicture(int index)
        {
            _loadData.LoadedTexture = _loadedImages[index - 1];
            _loadData.SceneName = SceneNames.FullPicture;
            _loadData.FakeLoad = false;
            SceneManager.LoadScene(SceneNames.Loader, LoadSceneMode.Additive);
        }

        public void GoBack()
        {
            _loadData.LoadedTexture = null;
            _loadData.FakeLoad = false;
            _loadData.SceneName = SceneNames.MainMenu;
            SceneManager.LoadScene(SceneNames.Loader, LoadSceneMode.Additive);
        }
    }
}