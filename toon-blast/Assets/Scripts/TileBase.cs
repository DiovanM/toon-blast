using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileBase : MonoBehaviour
{

    public Action<TileBase> onBlockDestroyed;
    public Action<TileBase> onBlockClicked;

    //Debug
    public TextMeshProUGUI label;
    //

    public Vector2Int coordinate;

    public PointerHandler pointerHandler;

    public BlockBase currentBlock;

    public bool busy;

    private void Awake()
    {
        pointerHandler.onPointerClick = (e) => onBlockClicked?.Invoke(this);
    }

    public void AddBlock(BlockBase block)
    {
        block.transform.SetParent(transform, false);
        currentBlock = block;

        currentBlock.onDestroy = OnBlockDestroyed;
    }

    private void OnBlockDestroyed()
    {
        onBlockDestroyed?.Invoke(this);
        currentBlock = null;
        pointerHandler.onPointerClick = null;
    }

}
