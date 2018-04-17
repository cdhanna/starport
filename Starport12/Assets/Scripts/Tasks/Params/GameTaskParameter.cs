using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smallgroup.Starport.Assets.Scripts.Tasks.Params
{

    [Serializable]
    public abstract class GameTaskParameter
    {

        public string Name;
        public bool SettableAfterCreation;
        public bool RequiredToCreateInstance;
        


        //public virtual TValue RequestNewValue(GameTask task)
        //{
        //    task.GetParameterValue<TValue>
        //}

        //public abstract void InvokeSetter();

        public abstract object GetDefault();
    }
    

  

}
