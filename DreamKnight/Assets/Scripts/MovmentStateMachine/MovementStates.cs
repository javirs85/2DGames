using Assets.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Assets.FSM
{
    //idle, running, jumping, falling, landing, attacking, hurt, dieing 
    public abstract class MovementState : IState<MovementStates>
    {
        public MovementState(Assets.FSM.MovementStates iD)
        {
            ID = iD;
        }

        public MovementState(Assets.FSM.MovementStates iD, MovementFSM reference)
        {
            ID = iD;
            MachineRef = reference;
        }

        protected MovementFSM MachineRef;

        public MovementStates ID { get ; set ; }

        public abstract void EnteringActions();

        public abstract void ExitingActions();
    }

    class IdleState : MovementState
    {
        public IdleState(MovementStates mID) : base(mID) { }
        public IdleState(MovementStates mID, MovementFSM reference) : base(mID, reference) { }

        public override void EnteringActions()
        {
            this.MachineRef.Animator.ResetTrigger("StartRunning");
            this.MachineRef.Animator.ResetTrigger("Jump");
            this.MachineRef.Animator.ResetTrigger("Failing");
            //this.MachineRef.Animator.ResetTrigger("LandAndIdle");

            this.MachineRef.Animator.SetTrigger("StopMoving");
        }

        public override void ExitingActions()
        {
        }
    }

    class RunningState : MovementState
    {
        public RunningState(MovementStates mID) : base(mID) { }
        public RunningState(MovementStates mID, MovementFSM reference) : base(mID, reference) { }


        public override void EnteringActions()
        {
            this.MachineRef.Animator.ResetTrigger("StopMoving");
            this.MachineRef.Animator.SetTrigger("StartRunning");
        }

        public override void ExitingActions()
        {
        }
    }

    class JumpingState : MovementState
    {
        public JumpingState(MovementStates mID) : base(mID) { }
        public JumpingState(MovementStates mID, MovementFSM reference) : base(mID, reference) { }

        public override void EnteringActions()
        {
            this.MachineRef.Animator.SetTrigger("Jump");
        }

        public override void ExitingActions()
        {
        }
    }

    class FallingState : MovementState
    {
        public FallingState(MovementStates mID) : base(mID) { }
        public FallingState(MovementStates mID, MovementFSM reference) : base(mID, reference) { }

        public override void EnteringActions()
        {
            this.MachineRef.Animator.SetTrigger("Falling");
        }

        public override void ExitingActions()
        {
            this.MachineRef.Animator.SetTrigger("LandAndIdle");
        }
    }
    
    

    class AttackingState : MovementState
    {
        public AttackingState(MovementStates mID) : base(mID) { }
        public AttackingState(MovementStates mID, MovementFSM reference) : base(mID, reference) { }

        public override void EnteringActions()
        {
            throw new NotImplementedException();
        }

        public override void ExitingActions()
        {
            throw new NotImplementedException();
        }
    }

    class BeingHurtState : MovementState
    {
        public BeingHurtState(MovementStates mID) : base(mID) { }
        public BeingHurtState(MovementStates mID, MovementFSM reference) : base(mID, reference) { }

        public override void EnteringActions()
        {
            throw new NotImplementedException();
        }

        public override void ExitingActions()
        {
            throw new NotImplementedException();
        }
    }

    class BeingKiltState : MovementState
    {
        public BeingKiltState(MovementStates mID) : base(mID) { }
        public BeingKiltState(MovementStates mID, MovementFSM reference) : base(mID, reference) { }

        public override void EnteringActions()
        {
            throw new NotImplementedException();
        }

        public override void ExitingActions()
        {
            throw new NotImplementedException();
        }
    }

}
