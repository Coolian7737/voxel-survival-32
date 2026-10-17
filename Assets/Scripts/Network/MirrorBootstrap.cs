using Mirror;
using UnityEngine;

namespace VoxelPrototype
{
    public class MirrorBootstrap : MonoBehaviour
    {
        [SerializeField] private bool autoStartHostInEditor = true;

        private void Start()
        {
#if UNITY_EDITOR
            if (autoStartHostInEditor)
            {
                var manager = FindObjectOfType<NetworkManager>();
                if (manager != null)
                {
                    manager.StartHost();
                }
            }
#endif
        }
    }
}
