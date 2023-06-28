using UnityEngine;

public class MobileOrientationSetup : MonoBehaviour
{
    [SerializeField] private ScreenOrientation _orienation;

    private void Awake()
    {
        if (Application.platform is RuntimePlatform.Android or RuntimePlatform.IPhonePlayer)
            Screen.orientation = _orienation;
    }
}