using Unity.Entities;
using Unity.Mathematics;

namespace BovineLabs.Part1
{
	public struct Vertex : IBufferElementData
	{
		public float3 Value;
	}
    
	public struct Uv : IBufferElementData
	{
		public float2 Value;
	}
    
	public struct Normal : IBufferElementData
	{
		public float3 Value;
	}

	public struct Triangle : IBufferElementData
	{
		public int Value;
	}
}
