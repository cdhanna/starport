using Smallgroup.Starport.Assets.Scripts.Events;
using Smallgroup.Starport.Assets.Scripts.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smallgroup.Starport.Assets.Scripts.JobSystem
{
    public class GameTaskRequestEventListener : GameEventListener<GameTaskRequestUnityEvent, GameTaskRequestEvent, TaskCreationRequested>
    {
    }
}
