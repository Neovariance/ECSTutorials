using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

namespace BovineLabs.Part1
{
    public class MeshApplySystem : ComponentSystem
    {   
        private struct Meshes
        {
            public readonly int Length;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public SharedComponentDataArray<MeshInstanceRenderer> MeshInstance;
            // Can't use [ReadOnly] until we get a CopyTo or GetPointerReadOnly with buffers
            /*[ReadOnly]*/ public BufferArray<Vertex> Vertices;
            /*[ReadOnly]*/ public BufferArray<Uv> Uvs;
            /*[ReadOnly]*/ public BufferArray<Normal> Normals;
            /*[ReadOnly]*/ public BufferArray<Triangle> Triangles;
        }

        [Inject] private Meshes _meshes;
        
        private readonly List<Vector3> _vertices = new List<Vector3>();
        private readonly List<Vector3> _normals = new List<Vector3>();
        private readonly List<Vector2> _uvs = new List<Vector2>();
        private readonly List<int> _triangles = new List<int>();

        protected override void OnUpdate()
        {
            for (var index = 0; index < _meshes.Length; index++)
            {
                // Conveniently Vector3 and float3 have the same memory footprint, so we can interpret between them
                var meshInstance = _meshes.MeshInstance[index];
                var vertices = _meshes.Vertices[index].Reinterpret<Vector3>();
                var normals = _meshes.Normals[index].Reinterpret<Vector3>();
                var uvs = _meshes.Uvs[index].Reinterpret<Vector2>();
                var triangles = _meshes.Triangles[index].Reinterpret<int>();
                
                // Iterating the array and adding elements 1 by 1 is pretty slow, so instead
                // we have a couple of extension methods that add to List via memcpy
                _vertices.NativeAddRange(vertices);
                _normals.NativeAddRange(normals);
                _uvs.NativeAddRange(uvs);
                _triangles.NativeAddRange(triangles);

                var mesh = meshInstance.mesh;
                mesh.Clear();
                
                // Using SetX methods of the mesh, reasons why is explained in future tutorials
                // This means we need to use List<T> instead of T[] 
                mesh.SetVertices(_vertices);
                mesh.SetNormals(_normals);
                mesh.SetUVs(0, _uvs);
                mesh.SetTriangles(_triangles, 0);
                
                _vertices.Clear();
                _normals.Clear();
                _uvs.Clear();
                _triangles.Clear();
            }
        }
    }
}