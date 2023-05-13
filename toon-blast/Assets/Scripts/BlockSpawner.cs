using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlockSpawner : MonoBehaviour
{

    [SerializeField] private List<Transform> points;
    [SerializeField] private List<NormalBlock> blocksPrefabs;
    [SerializeField] private Ease lerpEase;
    [SerializeField] private Transform blocksParent;

    [SerializeField] private float columnDistance = 60f;

    public void SpawnBlock(Vector2 position, int column, Action<NormalBlock> onFinish)
    {

        var prefab = blocksPrefabs[UnityEngine.Random.Range(0, blocksPrefabs.Count)];
        var startPosition = points[column].position;

        var block = Instantiate(prefab, blocksParent);
        block.transform.position = startPosition;
        block.Setup();

        onFinish?.Invoke(block);

    }

    public void SpawnBlocks(TileBase[] tiles, int column, Action<Vector2Int, NormalBlock> onFinish)
    {

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
