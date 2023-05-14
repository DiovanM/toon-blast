using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StringBlockBaseDictionary : SerializableDictionaryBase<string, BlockBase> { }

[Serializable]
public class StringGlobeBlockDictionary : SerializableDictionaryBase<string, GlobeBlock> { }

[Serializable]
public class CoordinateBlockBaseDictionary : SerializableDictionaryBase<Vector2Int, BlockBase> { }
