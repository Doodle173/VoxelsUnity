using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkCoordinate
{
    public int x, z;

    public ChunkCoordinate(int _x, int _z)
    {
        this.x = _x;
        this.z = _z;
    }

    public bool equals(ChunkCoordinate some_chunk)
    {
        if (some_chunk == null)
        {
            return false;
        }else if (some_chunk.x == x && some_chunk.z == z)
        {
            return true;
        }else
        {
            return false;
        }
    }
}
