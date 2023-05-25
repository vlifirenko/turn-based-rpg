using Scellecs.Morpeh;
using UnityEngine;

namespace TurnBasedRPG.Ecs.Systems.Unit
{
    public class UnitInitializer : IInitializer 
    {
        public World World { get; set; }

        public void OnAwake()
        {
            Debug.Log("init");
        }

        public void Dispose() { }
    }
}