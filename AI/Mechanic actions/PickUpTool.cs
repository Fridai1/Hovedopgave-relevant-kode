using Assets.Scripts.AI.AI_AGENTS;
using UnityEngine;

namespace Assets.Scripts.AI.Mechanic_actions
{
    public class PickUpTool : GoapAction
    {
        private GameObject tool;
        private bool complete;
        private float startTime;

        public PickUpTool()
        {
            addPrecondition("Tool in range", true);
            addPrecondition("Car ready", true);
            addPrecondition("Look busy", true);
            addEffect("Has tool", true);
        }

        void Start()
        {
            
            target = tool;
            cost = 2;
        }
        public override void reset()
        {
            complete = false;
            startTime = 0f;
            target = tool;
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
            if (startTime == 0)
            {
                startTime = Time.time;
            }

            if (startTime > 0.5f)
            {
                GetComponent<Mechanic>().inventory["Has tool"] = true;
                complete = true;
            }

            return true;
        }

        public override bool requiresInRange()
        {
            return true;
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.tag == "Tool")
            {
                tool = collider.gameObject;
            }
        }
    }
}