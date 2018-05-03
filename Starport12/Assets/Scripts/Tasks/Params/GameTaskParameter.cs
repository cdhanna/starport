using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smallgroup.Starport.Assets.Scripts.Tasks.Params
{

    public interface IGameTaskParameter
    {
        void OnCreateInstance(GameTask task);

    }

    public interface IGameTaskParameter<out TArg> : IGameTaskParameter
    {
    }

    [Serializable]
    public abstract class GameTaskParameter<TArg> : IGameTaskParameter<TArg>
    {

        public string Name;
        public bool SettableAfterCreation;
        public bool RequiredToCreateInstance;
        


        //public virtual TValue RequestNewValue(GameTask task)
        //{
        //    task.GetParameterValue<TValue>
        //}

        //public abstract void InvokeSetter();

        public abstract TArg GetDefault();

        public void OnCreateInstance(GameTask gameTask)
        {
            gameTask.SetValue<TArg>(this, GetDefault());
        }

        public virtual bool IsValid(GameTask instance, TArg value, TArg oldValue)
        {
            return true;
        }

        //public abstract IGameTaskParameter<object> AsDumb();
    }
    
    public class SingleContainer
    {
        public Single Data;
        public SingleContainer(Single value)
        {
            Data = value;
        }
    }
  

}
