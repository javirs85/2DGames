using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.FSM
{

    public enum MovementStates { idle, running, jumping, falling,  attacking, BeingHurt, BeingKilled }
    public enum MovementCommands { StartsRunning, StopMoving, LandedAfterAir, Jump, Fall, getHurt, getKilled, DoAttack }

    public class MovementFSM : FSMAbstract<MovementStates, MovementCommands>
    {
        public UnityEngine.Animator Animator;

        public MovementFSM(UnityEngine.Animator anim) => Animator = anim;

        public override void InitStatesAndCommandsAndSetInitialState()
        {
            States = new List<IState<MovementStates>>
            {
                new IdleState(MovementStates.idle, this),
                new RunningState(MovementStates.running, this),
                new JumpingState(MovementStates.jumping, this),
                new FallingState(MovementStates.falling, this),
                new AttackingState(MovementStates.attacking, this),
                new BeingHurtState(MovementStates.BeingHurt, this),
                new BeingKiltState(MovementStates.BeingKilled, this)
            };

            //AddTransition(MovementStates.idle, MovementCommands.DoAttack, MovementStates.attacking);
            AddTransition(MovementStates.idle, MovementCommands.StartsRunning, MovementStates.running);
            AddTransition(MovementStates.idle, MovementCommands.Jump, MovementStates.jumping);

            AddTransition(MovementStates.running, MovementCommands.StopMoving, MovementStates.idle);
            AddTransition(MovementStates.running, MovementCommands.Jump, MovementStates.jumping);

            AddTransition(MovementStates.jumping, MovementCommands.Fall, MovementStates.falling);
            AddTransition(MovementStates.jumping, MovementCommands.Jump, MovementStates.jumping);

            AddTransition(MovementStates.falling, MovementCommands.LandedAfterAir, MovementStates.idle);
            AddTransition(MovementStates.falling, MovementCommands.LandedAfterAir, MovementStates.running);
            AddTransition(MovementStates.falling, MovementCommands.Jump, MovementStates.jumping);
            
            AddTransitionFromAny(MovementCommands.Fall, MovementStates.falling);
            AddTransitionFromAny(MovementCommands.getHurt, MovementStates.BeingHurt);
            AddTransitionFromAny(MovementCommands.getKilled, MovementStates.BeingKilled);
        }
    }
}
