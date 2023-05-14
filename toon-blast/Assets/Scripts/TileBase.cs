using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileBase : MonoBehaviour
{

    public Vector2Int coordinate;

    public PointerHandler pointerHandler;

    public BlockBase currentBlock;

    //Debug

    public TextMeshProUGUI label;

    public bool busy;

    public void AddBlock(BlockBase block)
    {
        block.transform.SetParent(transform, false);
        currentBlock = block;
    }

}
