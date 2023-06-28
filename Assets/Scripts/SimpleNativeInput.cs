using UnityEngine;
using UnityEngine.Events;

public class SimpleNativeInput : MonoBehaviour
{
    [SerializeField] private UnityEvent _onNativeBackEvent;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            _onNativeBackEvent?.Invoke();
    }
}