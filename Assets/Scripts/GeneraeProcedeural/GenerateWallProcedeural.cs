using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GenerateWallProcedeural : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Material _material;
    private Mesh _mesh;
    public void GenerataComponents()
    {
        _meshRenderer = gameObject.AddComponent<MeshRenderer>();
        _meshFilter = gameObject.AddComponent<MeshFilter>();
        _meshRenderer.sharedMaterial = new Material(Shader.Find("Shader Graphs/Walls"));
        _mesh = new Mesh();
    }

    public void CreateMesh(Vector3 initialPosition, Vector3 finalPosition)
    {
        Debug.Log(initialPosition + " " + finalPosition);
        Vector3[] vertices = new Vector3[4]
        {
            transform.InverseTransformPoint(initialPosition),
            transform.InverseTransformPoint(finalPosition),
            transform.InverseTransformPoint(initialPosition + Vector3.up * 2),
            transform.InverseTransformPoint(finalPosition + Vector3.up * 2)
        };
        _mesh.vertices = vertices;

        bool front = Vector3.SignedAngle(finalPosition - initialPosition,
            Camera.main.transform.position - initialPosition, Vector3.up) < 0
            ? false
            : true;
        int[] triangles;
        if (!front)
        {
            triangles= new int[6]
            {
                0,1,2,
                1,3,2
            };
        }
        else
        {
            triangles= new int[6]
            {
                0,2,1,
                2,3,1
            };
        }
        _mesh.triangles = triangles;
        _meshFilter.mesh = _mesh;
    }
    [ContextMenu("Creacion quad")]
    public void CreateQuad(Vector3 initialPosition, Vector3 finalPosition)
    {
        GenerataComponents();
        CreateMesh(initialPosition, finalPosition);
        gameObject.AddComponent<MeshCollider>();
    }
}
