using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Physics;

namespace TilemapTools.Demo
{
    public class DebugInfo : SyncScript
    {

        public Keys ToggleKey = Keys.F12;

        public bool DebuggingEnabled = false;

        public override void Update()
        {
            if(Input.IsKeyPressed(ToggleKey))
            {
                DebuggingEnabled = !DebuggingEnabled;

                if (DebuggingEnabled)
                {
                    EnableDebugInfo();
                }
                else
                {
                    DisableDebugInfo();
                }

                
            }
            
        }

        private void DisableDebugInfo()
        {
            var simulation = this.GetSimulation();
            if (simulation != null)
            {
                simulation.ColliderShapesRendering = false;
            }
        }

        private void EnableDebugInfo()
        {
            var simulation = this.GetSimulation();
            if(simulation != null)
            {
                simulation.ColliderShapesRendering = true;
            }
            
        }
    }
}
