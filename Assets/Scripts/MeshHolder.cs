using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public interface IMeshHolder
{
    MeshHolder MeshHolder { get; set; }
}
public class MeshHolder : MonoBehaviour
{
    public Mesh Mesh { get; set; }
    public MeshHolder(Mesh mesh)
    {
        Mesh = mesh;
    }
}
