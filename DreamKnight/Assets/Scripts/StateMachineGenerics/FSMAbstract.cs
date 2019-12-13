using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.FSM
{
   
    public abstract class FSMAbstract <StatesType, CommandsType>
        where StatesType : struct
        where CommandsType : struct
    {
        public List<IState<StatesType>> States { get; set; }
        public Dictionary<StateTransition<StatesType, CommandsType>, StatesType> transitions;

        public StatesType CurrentState;

        public volatile bool busy = false;

        public FSMAbstract()
        {
            if (!typeof(CommandsType).IsEnum)
                throw new InvalidCastException("The generic parameter CommandsType must be enum");
            if (!typeof(StatesType).IsEnum)
                throw new InvalidCastException("The generic parameter StatesType must be enum");

            transitions = new Dictionary<StateTransition<StatesType, CommandsType>, StatesType>();

            InitStatesAndCommandsAndSetInitialState();
        }

        public abstract void InitStatesAndCommandsAndSetInitialState();

        private IState<StatesType> GetState(StatesType enumName)
        {
            return States.Find(x => x.ID.ToString() == enumName.ToString());
        }

        public void ProcessCommand(CommandsType command)
        {
            busy = true;

            StateTransition<StatesType, CommandsType> transition = new StateTransition<StatesType, CommandsType>(CurrentState, command);
            StatesType nextState;
            if (!transitions.TryGetValue(transition, out nextState))
                throw new Exception($"Invalid transition {CurrentState.ToString()} -> {command.ToString()}");

            var CurrentStateObj = GetState(CurrentState);

            Debug($"Moving from: {CurrentState.ToString()} to {nextState.ToString()}");
           // Debug($"Exiting actions:");
            CurrentStateObj.ExitingActions();

            CurrentState = nextState;

           // Debug($"Entering actions:");
            GetState(nextState).EnteringActions();

            Debug(" ");
            
            busy = false;
            
        }

        public void Debug(string s)
        {
            UnityEngine.Debug.Log(s);
        }

        public void AddTransition(StatesType oldState, CommandsType command, StatesType nextState)
        {
            try
            {
                transitions.Add(new StateTransition<StatesType, CommandsType>(oldState, command), nextState);
            }catch(Exception e)
            {
                Debug( "CONTROLED ERROR : " + e.Message);
            }
        }

        public void AddTransitionFromAny(CommandsType command, StatesType newState)
        {
            foreach(var state in States)
            {
                if (state.ToString() != newState.ToString())
                {
                    AddTransition(state.ID, command, newState);
                }
            }
        }
             
    }
    

    public class StateTransition<StatesEnum, CommandEnum>
        where StatesEnum : struct
        where CommandEnum : struct
    {
        public readonly StatesEnum PreviousState;
        public readonly CommandEnum Command;

        public StateTransition(StatesEnum _old, CommandEnum _commmand)
        {

            PreviousState = _old;
            if (!typeof(CommandEnum).IsEnum)
                throw new InvalidCastException("The generic parameter CommandEnum must be enum");
            if (!typeof(StatesEnum).IsEnum)
                throw new InvalidCastException("The generic parameter StatesEnum must be enum");
            Command = _commmand;
        }

        public override int GetHashCode()
        {
            return 17 + 31 * PreviousState.GetHashCode() + 31 * Command.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            StateTransition<StatesEnum, CommandEnum> other = obj as StateTransition<StatesEnum, CommandEnum>;
            return other != null && this.PreviousState.ToString() == other.PreviousState.ToString() && this.Command.ToString() == other.Command.ToString();
        }
    }
}
