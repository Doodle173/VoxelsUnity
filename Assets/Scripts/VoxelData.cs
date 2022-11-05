using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelData
{
    public static readonly int chunkWidth = 5;
    public static readonly int chunkHeight = 15;

    public static readonly int texture_atlas_size_in_blocks = 4;

    public static readonly int world_size_in_chunks = 50;
    public static readonly int view_distance_in_chunks = 8;

    public static float normalized_block_texture_size{
        get { return 1f / (float)texture_atlas_size_in_blocks; }
    }

    public static int world_size_in_blocks
    {
        get { return world_size_in_chunks * chunkWidth; }
    }

    public static readonly Vector3[] verts = new Vector3[8] {

        new Vector3(0.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 1.0f, 0.0f),
        new Vector3(0.0f, 1.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(1.0f, 0.0f, 1.0f),
        new Vector3(1.0f, 1.0f, 1.0f),
        new Vector3(0.0f, 1.0f, 1.0f),

    };

    public static readonly int[,] triangles = new int[6, 4] {

        { 0, 3, 1, 2 },
        { 5, 6, 4, 7 },
        { 3, 7, 2, 6 },
        { 1, 5, 0, 4 },
        { 4, 7, 0, 3 },
        { 1, 2, 5, 6 }

    };

    public static readonly Vector2[] voxel_uvs = new Vector2[4] {

        new Vector2(0, 0),
        new Vector2(0, 1),
        new Vector2(1, 0),
        new Vector2(1, 1)

    };

    public static readonly Vector3[] face_checks = new Vector3[6]{
        new Vector3(0, 0, -1),
        new Vector3(0, 0, 1),
        new Vector3(0, 1, 0),
        new Vector3(0, -1, 0),
        new Vector3(-1, 0, 0),
        new Vector3(1, 0, 0),
    };

}
