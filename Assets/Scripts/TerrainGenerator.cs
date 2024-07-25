using UnityEngine;

namespace Assets.Scripts
{
    public static class TerrainGenerator
    {
        public static BlockType[,,] GenerateTerrain(float xOffset, float yOffset)
        {
            var res = new BlockType[ChunkRenderer.ChunkWidth, ChunkRenderer.ChunkHeight, ChunkRenderer.ChunkWidth];

            for(int x = 0; x < ChunkRenderer.ChunkWidth; x++)
            {
                for(int z = 0; z < ChunkRenderer.ChunkWidth; z++)
                {
                    float height = Mathf.PerlinNoise((x + xOffset) * .5f, (z + yOffset) * .5f) * 5 + 10;

                    for(int y = 0; y < height; y++)
                    {
                        res[x,y,z] = BlockType.Ground;
                    }
                }
            }

            return res;
        }  
    }
}
