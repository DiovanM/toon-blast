using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlockSpawner : MonoBehaviour
{

    public Transform blocksParent;
    [SerializeField] private List<Transform> points;

    public NormalBlock SpawnBlock(int column)
    {

        var blocksPrefabs = GameSettings.GameConfig.normalBlocks;

        var prefab = blocksPrefabs[UnityEngine.Random.Range(0, blocksPrefabs.Count)];
        var startPosition = points[column].position;

        var block = Instantiate(prefab, blocksParent);
        block.transform.position = startPosition;
        block.Setup();

        return block;
    }

    public void SpawnBlocks(TileBase[] tiles, int column, Action<Vector2Int, NormalBlock> onFinish)
    {

        var blocksPrefabs = GameSettings.GameConfig.normalBlocks;

        var startPosition = points[column].position;

        for (int i = 0; i < tiles.Length; i++)
        {
            var prefab = blocksPrefabs[UnityEngine.Random.Range(0, blocksPrefabs.Count)];

            var block = Instantiate(prefab, blocksParent);
            block.transform.position = startPosition;
            block.Setup();

            onFinish?.Invoke(tiles[i].coordinate, block);
        }

    }

}
