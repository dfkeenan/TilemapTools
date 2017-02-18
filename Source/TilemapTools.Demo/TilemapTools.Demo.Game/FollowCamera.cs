using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;

namespace TilemapTools.Demo
{
    public class FollowCamera : AsyncScript
    {

        public TransformComponent Target;

        public float SpringStiffness = 10;

        public float Dampening = 5;



        private Vector3 oldPosition;
        private Vector3 relativeVelocity;

        public override async Task Execute()
        {
            oldPosition = this.Entity.Transform.Position;

            while (Game.IsRunning)
            {
                if(Target != null)
                {
                    var displacement = Target.Position - this.Entity.Transform.Position;
                    displacement.Z = 0;

                    var force = -(SpringStiffness * displacement) - (Dampening * relativeVelocity);
                    oldPosition = this.Entity.Transform.Position;
                    this.Entity.Transform.Position -= force * (float)Game.UpdateTime.Elapsed.TotalSeconds;
                    relativeVelocity = this.Entity.Transform.Position - oldPosition;


                }

                await Script.NextFrame();
            }
        }
    }
}
