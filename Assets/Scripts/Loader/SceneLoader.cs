using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

namespace Loader
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField]
        private Image _background;
        [SerializeField] private float _fadeTime = 0.2f;
        [SerializeField] private float _fakeLoadTime = 2f;
        [SerializeField] private int _fakeLoadParts = 5;
        [SerializeField] private LoadData _loadData;
        [SerializeField] private GameObject _progressBar;
        [SerializeField] private Image _progressFill;

        private void Start()
        {
            _background.DOFade(1f, _fadeTime).onComplete += () => StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene()
        {
            if (_loadData.FakeLoad)
            {
                _progressBar.SetActive(true);
                var wait = new WaitForSeconds(_fakeLoadTime / _fakeLoadParts);
                for (int i = 1; i <= _fakeLoadParts; i++)
                {
                    yield return wait;
                    _progressFill.fillAmount = (float) i / _fakeLoadParts;
                }
                yield return wait;
                _progressBar.SetActive(false);
            }
            
            SceneManager.LoadScene(_loadData.SceneName);
        }
    }
}