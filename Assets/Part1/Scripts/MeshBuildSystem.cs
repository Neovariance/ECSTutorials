using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace BovineLabs.Part1
{
    [UpdateBefore(typeof(MeshBuildSystem))]
    public class MeshBuildSystem : JobComponentSystem
    {
        private const int CubesToCreate = 3;

        private struct MeshData
        {
            public readonly int Length;
            public BufferArray<Vertex> Vertices;
            public BufferArray<Uv> Uvs;
            public BufferArray<Normal> Normals;
            public BufferArray<Triangle> Triangles;
        }

        [Inject] private MeshData _meshes;
        
        private CubeModel _cubeModel;

        [BurstCompile]
        private struct GenerateMeshJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<float3> ModelVertices;
            [ReadOnly] public NativeArray<float3> ModelNormals;
            [ReadOnly] public NativeArray<float2> ModelUVs;
            [ReadOnly] public NativeArray<int> ModelTriangles;
            public BufferArray<Vertex> Vertices;
            public BufferArray<Uv> Uvs;
            public BufferArray<Normal> Normals;
            public BufferArray<Triangle> Triangles;

            public void Execute(int index)
            {
                // This Reinterpret<T> is just a convenience
                var vertices = Vertices[index].Reinterpret<float3>();
                var uvs = Uvs[index].Reinterpret<float2>();
                var normals = Normals[index].Reinterpret<float3>();
                var triangles = Triangles[index].Reinterpret<int>();

                // Clear any existing data
                vertices.Clear();
                uvs.Clear();
                normals.Clear();
                triangles.Clear();

                // For this simple example, our mesh is just going to be a few cubes next to each other
                for (var cube = 0; cube < CubesToCreate; cube++)
                {
                    var offset = new float3(1.5f * cube, 0, 0);

                    for (var face = 0; face < 6; face++)
                    {
                        // 4 Verts per face
                        for (var n = 0; n < 4; n++)
                        {
                            var vertexIndex = face * 4 + n;

                            var vert = ModelVertices[vertexIndex];
                            vert += offset;
                            vertices.Add(vert);

                            uvs.Add(ModelUVs[vertexIndex]);

                            // The whole face has same normal
                            normals.Add(ModelNormals[face]);
                        }

                        // 6 Tris per face
                        for (var n = 0; n < 6; n++)
                        {
                            // This is a bit confusing but it's because of how our CubeModel is layed out
                            // which is not standard, but it'll make sense in a later tutorial
                            // Basically we offset 24 verts per cube, 4 verts per face to get the correct face
                            triangles.Add(cube * 24 + face * 4 + ModelTriangles[face * 6 + n]);
                        }
                    }
                }
            }
        }

        private ComponentGroup _group;
        
        protected override void OnCreateManager(int capacity)
        {
            _cubeModel = CubeModel.Create();

            _group = GetComponentGroup(ComponentType.Create<Vertex>());
        }

        protected override void OnDestroyManager()
        {
            _cubeModel.Dispose();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            return new GenerateMeshJob
                {
                    ModelVertices = _cubeModel.Vertices,
                    ModelNormals = _cubeModel.Normals,
                    ModelUVs = _cubeModel.Uvs,
                    ModelTriangles = _cubeModel.Indices,

                    Vertices = _meshes.Vertices,
                    Uvs = _meshes.Uvs,
                    Normals = _meshes.Normals,
                    Triangles = _meshes.Triangles,
                }
                .Schedule(_meshes.Length, 64, inputDeps);
        }
    }
}