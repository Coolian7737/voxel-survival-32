using UnityEngine;

namespace VoxelPrototype
{
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject bossPrefab;
        [SerializeField] private Vector3 playerSpawn = Vector3.zero;
        [SerializeField] private Vector3 bossSpawn = new Vector3(20f, 12f, 20f);

        private void Start()
        {
            EnsureWorldGenerator();
            EnsurePlayer();
            EnsureBoss();
            EnsureCraftingSystem();
        }

        private void EnsureWorldGenerator()
        {
            if (FindObjectOfType<VoxelWorldGenerator>() != null)
            {
                return;
            }

            GameObject worldObject = new GameObject("VoxelWorld");
            worldObject.transform.SetParent(transform);
            var generator = worldObject.AddComponent<VoxelWorldGenerator>();

            GameObject mainCamera = Camera.main != null ? Camera.main.gameObject : new GameObject("Main Camera");
            if (!mainCamera.CompareTag("MainCamera"))
            {
                mainCamera.tag = "MainCamera";
            }

            mainCamera.transform.position = new Vector3(0f, 12f, -12f);
            mainCamera.transform.rotation = Quaternion.Euler(22f, 0f, 0f);
            mainCamera.AddComponent<Camera>();
        }

        private void EnsurePlayer()
        {
            if (GameObject.FindWithTag("Player") != null)
            {
                return;
            }

            GameObject player = playerPrefab != null ? Instantiate(playerPrefab, playerSpawn, Quaternion.identity) : new GameObject("Player");
            player.tag = "Player";
            player.transform.position = playerSpawn;
            player.AddComponent<CharacterController>();
            player.AddComponent<PlayerController>();
            player.AddComponent<SurvivalSystem>();
            player.AddComponent<PlayerCombat>();

            var controller = player.GetComponent<CharacterController>();
            controller.height = 1.8f;
            controller.radius = 0.35f;
            controller.center = new Vector3(0f, 0.9f, 0f);

            GameObject cameraRig = new GameObject("CameraRig");
            cameraRig.transform.SetParent(player.transform);
            cameraRig.transform.localPosition = new Vector3(0f, 1.6f, 0f);

            Camera playerCamera = Camera.main != null ? Camera.main : cameraRig.AddComponent<Camera>();
            playerCamera.transform.SetParent(cameraRig.transform);
            playerCamera.transform.localPosition = new Vector3(0f, 0f, -5f);
            playerCamera.transform.localRotation = Quaternion.identity;

            var generator = FindObjectOfType<VoxelWorldGenerator>();
            if (generator != null)
            {
                generator.SetPlayer(player.transform);
            }
        }

        private void EnsureBoss()
        {
            if (GameObject.Find("Boss") != null)
            {
                return;
            }

            GameObject boss = bossPrefab != null ? Instantiate(bossPrefab, bossSpawn, Quaternion.identity) : GameObject.CreatePrimitive(PrimitiveType.Capsule);
            boss.name = "Boss";
            boss.transform.position = bossSpawn;
            boss.transform.localScale = new Vector3(2.2f, 2.2f, 2.2f);
            boss.AddComponent<BossController>();
        }

        private void EnsureCraftingSystem()
        {
            if (FindObjectOfType<CraftingSystem>() != null)
            {
                return;
            }

            GameObject craftingObject = new GameObject("CraftingSystem");
            craftingObject.transform.SetParent(transform);
            craftingObject.AddComponent<CraftingSystem>();
        }
    }
}
