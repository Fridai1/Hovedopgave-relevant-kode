using UnityEngine;

namespace Assets.Scripts.AI.Mechanic_actions
{
    public class RepCarWithoutTool : GoapAction
    {
        private bool complete;
        private float startTime;

        public RepCarWithoutTool()
        {
            addPrecondition("Car ready", true);
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

            if (startTime > 10)
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