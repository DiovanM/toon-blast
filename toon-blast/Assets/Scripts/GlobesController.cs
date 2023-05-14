using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobesController : MonoBehaviour
{

    [SerializeField] private GridController gridController;

    private void Awake()
    {
        GridController.onClickTile += OnClickTile;
    }

    private void OnClickTile(TileBase tile)
    {
        if (tile.currentBlock is not GlobeBlock)
            return;

        var block = tile.currentBlock as GlobeBlock;

        var tiles = gridController.GetTilesOfId(block.normalBlockId);

        block.Play();

        foreach (var item in tiles)
        {
            item.currentBlock.Destroy();
        }

        block.Destroy();

        gridController.UpdateGrid();

    }

}
