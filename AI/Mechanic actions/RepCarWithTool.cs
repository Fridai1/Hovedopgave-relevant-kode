using UnityEngine;

namespace Assets.Scripts.AI.Mechanic_actions
{
    public class RepCarWithTool : GoapAction
    {
        private bool complete;
        private float startTime;

        public RepCarWithTool()
        {
            addPrecondition("Car ready", true);
            addPrecondition("Has tool", true);
            addEffect("Car repaired", true);
        }

        void Start()
        {
            target = BlackBoard.Car;
        }
        public override void reset()
        {
            complete = false;
            startTime = 0f;
            target = BlackBoard.Car;
        }

        public override bool isDone()
        {
            return complete;
        }

        public override bool checkProceduralPrecondition(GameObject agent)
        {
            return true;
        }

        public override bool perform(GameObject agent)
        {
            if (startTime == 0f)
            {
                startTime = Time.time;
            }

            if (startTime > 5)
            {
                complete = true;
            }

            return true;
        }

        public override bool requiresInRange()
        {
            return true;
        }
    }
}