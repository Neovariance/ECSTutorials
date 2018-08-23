using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace BovineLabs.Part1
{
    public static class Bootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            var meshArchetype = entityManager.CreateArchetype(
                typeof(Vertex), typeof(Uv), typeof(Normal), typeof(Triangle), // Mesh components
                typeof(MeshInstanceRenderer), // The actual mesh
                typeof(Position)); // Required by MeshInstanceRenderSystem
            
            var entity = entityManager.CreateEntity(meshArchetype);

            var mesh = new Mesh();
            mesh.MarkDynamic();

            var material = new Material(Shader.Find("Standard")) {enableInstancing = true};

            var meshInstanceRenderer = new MeshInstanceRenderer
            {
                mesh = mesh,
                material = material
            };
            
            entityManager.SetSharedComponentData(entity, meshInstanceRenderer);
        }
    }
}