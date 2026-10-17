using System.Collections.Generic;
using UnityEngine;

namespace VoxelPrototype
{
    public class VoxelWorldGenerator : MonoBehaviour
    {
        [Header("World Settings")]
        [SerializeField] private int chunkSize = 16;
        [SerializeField] private int renderRadius = 2;
        [SerializeField] private int maxHeight = 10;
        [SerializeField] private float noiseScale = 0.14f;

        [Header("References")]
        [SerializeField] private Transform player;

        private readonly Dictionary<Vector2Int, GameObject> loadedChunks = new();
        private Vector2Int lastPlayerChunk;

        private void Start()
        {
            if (player == null)
            {
                if (Camera.main != null)
                {
                    player = Camera.main.transform;
                }
            }

            if (player != null)
            {
                UpdateWorldAroundPlayer();
            }
        }

        private void Update()
        {
            if (player == null)
            {
                return;
            }

            Vector2Int currentChunk = new Vector2Int(
                Mathf.FloorToInt(player.position.x / chunkSize),
                Mathf.FloorToInt(player.position.z / chunkSize));

            if (currentChunk != lastPlayerChunk)
            {
                lastPlayerChunk = currentChunk;
                UpdateWorldAroundPlayer();
            }
        }

        private void UpdateWorldAroundPlayer()
        {
            for (int x = -renderRadius; x <= renderRadius; x++)
            {
                for (int z = -renderRadius; z <= renderRadius; z++)
                {
                    Vector2Int chunkCoord = new Vector2Int(lastPlayerChunk.x + x, lastPlayerChunk.y + z);
                    if (!loadedChunks.ContainsKey(chunkCoord))
                    {
                        GenerateChunk(chunkCoord.x, chunkCoord.y);
                    }
                }
            }
        }

        private void GenerateChunk(int chunkX, int chunkZ)
        {
            GameObject chunkObject = new GameObject($"Chunk_{chunkX}_{chunkZ}");
            chunkObject.transform.SetParent(transform);
            chunkObject.transform.position = new Vector3(chunkX * chunkSize, 0f, chunkZ * chunkSize);

            for (int x = 0; x < chunkSize; x++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    int worldX = chunkX * chunkSize + x;
                    int worldZ = chunkZ * chunkSize + z;
                    int topHeight = Mathf.FloorToInt(GetTerrainHeight(worldX, worldZ));

                    for (int y = 0; y <= topHeight; y++)
                    {
                        GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        block.transform.SetParent(chunkObject.transform);
                        block.transform.position = new Vector3(worldX, y, worldZ);

                        Renderer renderer = block.GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            renderer.material = new Material(Shader.Find("Standard"));
                            renderer.material.color = GetTerrainColor(y, topHeight);
                        }

                        if (y == 0)
                        {
                            block.name = "Stone";
                        }
                        else if (y < topHeight - 1)
                        {
                            block.name = "Dirt";
                        }
                        else
                        {
                            block.name = "Grass";
                        }
                    }
                }
            }

            loadedChunks.Add(new Vector2Int(chunkX, chunkZ), chunkObject);
        }

        private float GetTerrainHeight(int worldX, int worldZ)
        {
            float noise = Mathf.PerlinNoise((worldX + 1000f) * noiseScale, (worldZ + 1000f) * noiseScale);
            return Mathf.RoundToInt(noise * maxHeight);
        }

        private Color GetTerrainColor(int currentY, int topHeight)
        {
            if (currentY >= topHeight)
            {
                return new Color(0.2f, 0.8f, 0.25f);
            }

            if (currentY > topHeight - 2)
            {
                return new Color(0.55f, 0.38f, 0.18f);
            }

            return new Color(0.35f, 0.25f, 0.18f);
        }
    }
}
