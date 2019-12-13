using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.FSM
{
    public interface IState<StateType>
        where StateType : struct
    {
        void EnteringActions();
        void ExitingActions();
        StateType ID { get; set; }
    }

}
