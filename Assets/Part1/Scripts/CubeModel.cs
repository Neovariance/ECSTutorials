using System;
using Unity.Collections;
using Unity.Mathematics;

namespace BovineLabs.Part1
{
    public struct CubeModel : IDisposable
    {
        public NativeArray<float3> Vertices;
        public NativeArray<float3> Normals;
        public NativeArray<float2> Uvs;
        public NativeArray<int> Indices;

        public static CubeModel Create()
        {
            return new CubeModel
            {
                Vertices = new NativeArray<float3>(new[]
                    {
                        new float3(0f, 0f, 0f), new float3(0f, 1f, 1f),
                        new float3(0f, 1f, 0f), new float3(0f, 0f, 1f),

                        new float3(1f, 0f, 0f), new float3(1f, 1f, 1f),
                        new float3(1f, 1f, 0f), new float3(1f, 0f, 1f),

                        new float3(0f, 1f, 0f), new float3(0f, 1f, 1f),
                        new float3(1f, 1f, 0f), new float3(1f, 1f, 1f),

                        new float3(0f, 0f, 0f), new float3(0f, 0f, 1f),
                        new float3(1f, 0f, 0f), new float3(1f, 0f, 1f),

                        new float3(0f, 0f, 0f), new float3(0f, 1f, 0f),
                        new float3(1f, 1f, 0f), new float3(1f, 0f, 0f),

                        new float3(0f, 0f, 1f), new float3(0f, 1f, 1f),
                        new float3(1f, 1f, 1f), new float3(1f, 0f, 1f)
                    },
                    Allocator.Persistent),

                Normals = new NativeArray<float3>(new[]
                    {
                        new float3(-1, 0, 0),
                        new float3(+1, 0, 0),
                        new float3(0, +1, 0),
                        new float3(0, -1, 0),
                        new float3(0, 0, -1),
                        new float3(0, 0, +1)
                    },
                    Allocator.Persistent),

                Uvs = new NativeArray<float2>(new[]
                {
                    new float2(1f, 0f), new float2(0f, 1f),
                    new float2(1f, 1f), new float2(0f, 0f),

                    new float2(0f, 0f), new float2(1f, 1f),
                    new float2(0f, 1f), new float2(1f, 0f),

                    new float2(0f, 0f), new float2(0f, 1f),
                    new float2(1f, 0f), new float2(1f, 1f),

                    new float2(0f, 0f), new float2(0f, 1f),
                    new float2(1f, 0f), new float2(1f, 1f),

                    new float2(0f, 0f), new float2(0f, 1f),
                    new float2(1f, 1f), new float2(1f, 0f),

                    new float2(1f, 0f), new float2(1f, 1f),
                    new float2(0f, 1f), new float2(0f, 0f)
                }, Allocator.Persistent),

                Indices = new NativeArray<int>(new[]
                    {
                        0, 1, 2, 0, 3, 1, // left -
                        0, 1, 3, 0, 2, 1, // right +
                        0, 3, 2, 0, 1, 3, // top +
                        0, 3, 1, 0, 2, 3, // bottom -
                        0, 2, 3, 0, 1, 2, // front +
                        0, 2, 1, 0, 3, 2 // back -
                    },
                    Allocator.Persistent)
            };
        }

        public void Dispose()
        {
            Vertices.Dispose();
            Normals.Dispose();
            Uvs.Dispose();
            Indices.Dispose();
        }
    }
}