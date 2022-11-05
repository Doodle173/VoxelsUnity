using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{

    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    int vertex_index = 0;
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uvs = new List<Vector2>();

    byte[,,] voxel_map = new byte[VoxelData.chunkWidth, VoxelData.chunkHeight, VoxelData.chunkWidth];

    World world;

    // Start is called before the first frame update
    void Start()
    {

        world = GameObject.Find("World").GetComponent<World>();
        populate_voxel_map();
        create_mesh_data();
        create_mesh();

    }

    void populate_voxel_map()
    {
        for (int x = 0; x < VoxelData.chunkHeight; x++)
        {
            for (int y = 0; y < VoxelData.chunkHeight; y++)
            {
                for (int z = 0; z < VoxelData.chunkHeight; z++)
                {
                    voxel_map[x, y, z] = 1;
                }
            }
        }
    }

    bool check_voxel(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x);
        int y = Mathf.FloorToInt(pos.y);
        int z = Mathf.FloorToInt(pos.z);

        if (x < 0 || x > VoxelData.chunkWidth - 1 ||
            y < 0 || y > VoxelData.chunkHeight - 1 ||
            z < 0 || z > VoxelData.chunkWidth - 1){
            return false;
        }

        return world.blockTypes[voxel_map[x, y, z]].isSolid;
    }

    void create_mesh_data()
    {
        for (int x = 0; x < VoxelData.chunkHeight; x++)
        {
            for (int y = 0; y < VoxelData.chunkHeight; y++)
            {
                for (int z = 0; z < VoxelData.chunkHeight; z++)
                {
                    add_voxel_data_to_chunk(new Vector3(x, y, z));
                }
            }
        }
    }

    void add_voxel_data_to_chunk(Vector3 pos)
    {
        for (int p = 0; p < 6; p++)
        {
            if (!check_voxel(pos + VoxelData.face_checks[p]))
            {
                byte block_id = voxel_map[(int)pos.x, (int)pos.y, (int)pos.z];

                vertices.Add(pos + VoxelData.verts[VoxelData.triangles[p, 0]]);
                vertices.Add(pos + VoxelData.verts[VoxelData.triangles[p, 1]]);
                vertices.Add(pos + VoxelData.verts[VoxelData.triangles[p, 2]]);
                vertices.Add(pos + VoxelData.verts[VoxelData.triangles[p, 3]]);

                add_texture(world.blockTypes[block_id].getTextureID(p));

                triangles.Add(vertex_index);
                triangles.Add(vertex_index + 1);
                triangles.Add(vertex_index + 2);
                triangles.Add(vertex_index + 2);
                triangles.Add(vertex_index + 1);
                triangles.Add(vertex_index + 3);

                vertex_index += 4;
            }
        }
    }

    void create_mesh()
    {
        Mesh m = new Mesh();
        m.vertices = vertices.ToArray();
        m.triangles = triangles.ToArray();
        m.uv = uvs.ToArray();

        m.RecalculateNormals();

        meshFilter.mesh = m;
    }

    void add_texture(int t_id)
    {
        float y = t_id / VoxelData.texture_atlas_size_in_blocks;
        float x = t_id - (y * VoxelData.texture_atlas_size_in_blocks);

        x *= VoxelData.normalized_block_texture_size;
        y *= VoxelData.normalized_block_texture_size;

        y = 1f - y - VoxelData.normalized_block_texture_size;

        uvs.Add(new Vector2(x, y));
        uvs.Add(new Vector2(x, y + VoxelData.normalized_block_texture_size));
        uvs.Add(new Vector2(x + VoxelData.normalized_block_texture_size, y));
        uvs.Add(new Vector2(x + VoxelData.normalized_block_texture_size, y + VoxelData.normalized_block_texture_size));
    }
}
