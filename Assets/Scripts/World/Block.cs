using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Block
{

    public string blockName;
    public bool isSolid;

    [Header("Texture Values")]
    public int leftFaceTexture;
    public int rightFaceTexture;

    public int topFaceTexture;
    public int bottomFaceTexture;

    public int frontFaceTexture;
    public int backFaceTexture;

    public int getTextureID(int face_idx)
    {
        switch (face_idx)
        {
            case 0:
                return backFaceTexture;
            case 1:
                return frontFaceTexture;
            case 2:
                return topFaceTexture;
            case 3:
                return bottomFaceTexture;
            case 4:
                return leftFaceTexture;
            case 5:
                return rightFaceTexture;
            default:
                Debug.Log("Failed to get a texture ID. Invalid face index.");
                return 0;
        }
    }

}
