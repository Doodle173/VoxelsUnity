using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class World : MonoBehaviour
{

    public Transform player;
    public Vector3 spawn;

    public Material material;
    public Block[] blockTypes;

    Chunk[,] chunks = new Chunk[VoxelData.world_size_in_chunks, VoxelData.world_size_in_chunks];
    List<ChunkCoordinate> active_chunks = new List<ChunkCoordinate> ();
    ChunkCoordinate player_last_chunk_coord;

    // Start is called before the first frame update
    void Start()
    {
        generate_world();
        player_last_chunk_coord = get_chunk_coord_from_vec3(player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!get_chunk_coord_from_vec3(player.transform.position).equals(player_last_chunk_coord))
            check_view_distance();
    }

    private void generate_world()
    {
        for (int x = VoxelData.world_size_in_chunks / 2 - VoxelData.view_distance_in_chunks / 2; x < VoxelData.world_size_in_chunks / 2 + VoxelData.view_distance_in_chunks / 2; x++)
        {
            for (int z = VoxelData.world_size_in_chunks / 2 - VoxelData.view_distance_in_chunks / 2; z < VoxelData.world_size_in_chunks / 2 + VoxelData.view_distance_in_chunks / 2; z++)
            {
                create_chunk(new ChunkCoordinate(x, z));
            }
        }

        spawn = new Vector3(VoxelData.world_size_in_blocks/2, VoxelData.chunkHeight+2, VoxelData.world_size_in_blocks/2);
        player.position = spawn;
    }

    private void check_view_distance()
    {
        int chunk_x = Mathf.FloorToInt(player.position.x / VoxelData.chunkWidth);
        int chunk_z = Mathf.FloorToInt(player.position.z / VoxelData.chunkWidth);

        List<ChunkCoordinate> prev_active_chunks = new List<ChunkCoordinate>(active_chunks);

        for (int x = chunk_x - VoxelData.view_distance_in_chunks / 2; x < chunk_x + VoxelData.view_distance_in_chunks/2; x++)
        {
            for (int z = chunk_z - VoxelData.view_distance_in_chunks / 2; z < chunk_z + VoxelData.view_distance_in_chunks / 2; z++)
            {
                if (is_chunk_in_world(x, z))
                {
                    ChunkCoordinate this_chunk = new ChunkCoordinate(x, z);

                    if (chunks[x, z] == null)
                    {
                        create_chunk(this_chunk);
                    }else if (!chunks[x, z].isActive)
                    {
                        chunks[x, z].isActive = true;
                        active_chunks.Add(this_chunk);
                    }

                    for (int i=0; i < prev_active_chunks.Count; i++)
                    {
                        if (prev_active_chunks[i].x == x && prev_active_chunks[i].z == z)
                        {
                            prev_active_chunks.RemoveAt(i);
                        }
                    }

                }
            }
        }

        foreach (ChunkCoordinate co in prev_active_chunks)
            chunks[co.x, co.z].isActive = false;
    }

    bool is_chunk_in_world(int x, int z)
    {
        if (x > 0 && x < VoxelData.world_size_in_chunks - 1 && z > 0 && z < VoxelData.world_size_in_chunks - 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    ChunkCoordinate get_chunk_coord_from_vec3(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x / VoxelData.chunkWidth);
        int z = Mathf.FloorToInt(pos.z / VoxelData.chunkWidth);

        return new ChunkCoordinate(x, z);
    }

    private void create_chunk(ChunkCoordinate co)
    {
        chunks[co.x, co.z] = new Chunk(new ChunkCoordinate(co.x, co.z), this);
        active_chunks.Add(new ChunkCoordinate(co.x, co.z));
    }

    public byte get_voxel(Vector3 pos)
    {
        if (pos.x < 0 || pos.x > VoxelData.world_size_in_blocks - 1 || pos.y < 0 || pos.y > VoxelData.chunkHeight - 1 || pos.z < 0 || pos.z > VoxelData.world_size_in_blocks - 1)
        {
            return 0;
        }

        if (pos.y < 1)
        {
            return 1;
        }else if (pos.y == VoxelData.chunkHeight-1)
        {
            return 3;
        }
        else
        {
            return 2;
        }
    }
}
