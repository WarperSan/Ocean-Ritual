using EntityModule.Entities;
using UnityEngine;

namespace BossesModule.Worm
{
    [RequireComponent(typeof(WormTree))]
    public class WormEntity : BossEntity
    {
        public Transform ArenaOrigin => this.barrierParent;
        public float ArenaRadius => this.barrierRange;
    }
}

