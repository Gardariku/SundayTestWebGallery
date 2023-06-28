using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gallery
{
    [RequireComponent(typeof(LayoutElement))]
    public class CellAspectFitter : MonoBehaviour
    {
        private LayoutElement _layoutElement;
        private RectTransform _rectTransform;

        private void Start()
        {
            _layoutElement = GetComponent<LayoutElement>();
            _rectTransform = GetComponent<RectTransform>();
            _layoutElement.preferredHeight = _rectTransform.rect.width;
        }
    }
}