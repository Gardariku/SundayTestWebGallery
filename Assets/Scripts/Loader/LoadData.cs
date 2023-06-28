using UnityEngine;

namespace Loader
{
    [CreateAssetMenu(fileName = "LoadData", menuName = "Gallery/Loader", order = 0)]
    public class LoadData : ScriptableObject
    {
        public string SceneName;
        public bool FakeLoad;
        public Texture LoadedTexture;
    }
}