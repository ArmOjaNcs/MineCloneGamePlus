using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class ChunkRenderer : MonoBehaviour
    {
        public const int ChunkWidth = 10;
        public const int ChunkHeight = 128;
        public const float BlockScale = .5f;

        private List<Vector3> _vertices = new List<Vector3>();
        private List<int> _triangles = new List<int>();

        public ChunkData ChunkData;
        public GameWorld ParrentWorld;

        private void Start()
        {
            Mesh chunkMesh = new Mesh();

            for (int y = 0; y < ChunkHeight; y++)
            {
                for (int x = 0; x < ChunkWidth; x++)
                {
                    for (int z = 0; z < ChunkWidth; z++)
                    {
                        GenerateBlocks(x, y, z);
                    }
                }
            }

            chunkMesh.vertices = _vertices.ToArray();
            chunkMesh.triangles = _triangles.ToArray();
            chunkMesh.RecalculateNormals();
            chunkMesh.RecalculateBounds();

            chunkMesh.Optimize();

            GetComponent<MeshFilter>().sharedMesh = chunkMesh;
            GetComponent<MeshCollider>().sharedMesh = chunkMesh;
        }

        private void GenerateBlocks(int x, int y, int z)
        {
            var blockPosition = new Vector3Int(x, y, z);

            if (GetBlockAtPosition(blockPosition) == BlockType.Air)
                return;

            if (GetBlockAtPosition(blockPosition + Vector3Int.right) == BlockType.Air) GenerateRightSide(blockPosition);
            if (GetBlockAtPosition(blockPosition + Vector3Int.left) == BlockType.Air) GenerateLeftSide(blockPosition);
            if (GetBlockAtPosition(blockPosition + Vector3Int.back) == BlockType.Air) GenerateBackSide(blockPosition);
            if (GetBlockAtPosition(blockPosition + Vector3Int.forward) == BlockType.Air) GenerateFrontSide(blockPosition);
            if (GetBlockAtPosition(blockPosition + Vector3Int.up) == BlockType.Air) GenerateTopSide(blockPosition);
            if (GetBlockAtPosition(blockPosition + Vector3Int.down) == BlockType.Air) GenerateBottomSide(blockPosition);
        }

        private BlockType GetBlockAtPosition(Vector3Int position)
        {
            if (position.x >= 0 && position.x < ChunkWidth &&
               position.y >= 0 && position.y < ChunkHeight &&
               position.z >= 0 && position.z < ChunkWidth)
                return ChunkData.Blocks[position.x, position.y, position.z];
            else
            {
                if (position.y < 0 || position.y >= ChunkHeight)
                    return BlockType.Air;

                Vector2Int adjChunkPos = ChunkData.ChunkPosition;

                if (position.x < 0)
                {
                    adjChunkPos.x--;
                    position.x += ChunkWidth;
                }
                else if (position.x >= ChunkWidth)
                {
                    adjChunkPos.x++;
                    position.x -= ChunkWidth;
                }

                if (position.z < 0)
                {
                    adjChunkPos.y--;
                    position.z += ChunkWidth;
                }
                else if (position.z >= ChunkWidth)
                {
                    adjChunkPos.y++;
                    position.z -= ChunkWidth;
                }

                if (ParrentWorld.ChunkDatas.TryGetValue(adjChunkPos, out ChunkData aChunk))
                {
                    return aChunk.Blocks[position.x, position.y, position.z];
                }
                else
                {
                    return BlockType.Air;
                }
            }
        }

        private void GenerateRightSide(Vector3Int blockPosition)
        {

            _vertices.Add((new Vector3(1, 0, 0) + blockPosition) * BlockScale);
            _vertices.Add((new Vector3(1, 1, 0) + blockPosition) * BlockScale);
            _vertices.Add((new Vector3(1, 0, 1) + blockPosition) * BlockScale);
            _vertices.Add((new Vector3(1, 1, 1) + blockPosition) * BlockScale);

            AddLastVerticesSquare();
        }

        private void GenerateLeftSide(Vector3Int blockPosition)
        {
            _vertices.Add((new Vector3(0, 0, 0) + blockPosition) * BlockScale);
            _vertices.Add((new Vector3(0, 0, 1) + blockPosition) * BlockScale);
            _vertices.Add((new Vector3(0, 1, 0) + blockPosition) * BlockScale);
            _vertices.Add((new Vector3(0, 1, 1) + blockPosition) * BlockScale);

            AddLastVerticesSquare();
        }

        private void GenerateFrontSide(Vector3Int blockPosition)
        {
            _vertices.Add((new Vector3(0, 0, 1) + blockPosition) * BlockScale);
            _vertices.Add((new Vector3(1, 0, 1) + blockPosition) * BlockScale);
            _vertices.Add((new Vector3(0, 1, 1) + blockPosition) * BlockScale);
            _vertices.Add((new Vector3(1, 1, 1) + blockPosition) * BlockScale);

            AddLastVerticesSquare();
        }

        private void GenerateBackSide(Vector3Int blockPosition)
        {

            _vertices.Add((new Vector3(0, 0, 0) + blockPosition) * BlockScale);
            _vertices.Add((new Vector3(0, 1, 0) + blockPosition) * BlockScale);
            _vertices.Add((new Vector3(1, 0, 0) + blockPosition) * BlockScale);
            _vertices.Add((new Vector3(1, 1, 0) + blockPosition) * BlockScale);

            AddLastVerticesSquare();
        }

        private void GenerateTopSide(Vector3Int blockPosition)
        {
            _vertices.Add((new Vector3(0, 1, 0) + blockPosition) * BlockScale);
            _vertices.Add((new Vector3(0, 1, 1) + blockPosition) * BlockScale);
            _vertices.Add((new Vector3(1, 1, 0) + blockPosition) * BlockScale);
            _vertices.Add((new Vector3(1, 1, 1) + blockPosition) * BlockScale);

            AddLastVerticesSquare();
        }

        private void GenerateBottomSide(Vector3Int blockPosition)
        {
            _vertices.Add((new Vector3(0, 0, 0) + blockPosition) * BlockScale);
            _vertices.Add((new Vector3(1, 0, 0) + blockPosition) * BlockScale);
            _vertices.Add((new Vector3(0, 0, 1) + blockPosition) * BlockScale);
            _vertices.Add((new Vector3(1, 0, 1) + blockPosition) * BlockScale);

            AddLastVerticesSquare();
        }

        private void AddLastVerticesSquare()
        {
            _triangles.Add(_vertices.Count - 4);
            _triangles.Add(_vertices.Count - 3);
            _triangles.Add(_vertices.Count - 2);

            _triangles.Add(_vertices.Count - 3);
            _triangles.Add(_vertices.Count - 1);
            _triangles.Add(_vertices.Count - 2);
        }
    }
}