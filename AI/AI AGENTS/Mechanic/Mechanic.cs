using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.AI.AI_AGENTS
{
    public class Mechanic : MonoBehaviour, IGoap
    {
        private NavMeshAgent agent;
        private Vector3 previousDestination;
        public Dictionary<string, bool> inventory;
        private bool ToolIsInRange;

        void Start()
        {
            inventory = new Dictionary<string, bool>();
            inventory.Add("Has tool", false);
            this.agent = GetComponent<NavMeshAgent>();
            BlackBoard.IsCarRdy = false;
            BlackBoard.Car = GameObject.FindWithTag("Car");
        }


        public HashSet<KeyValuePair<string, object>> getWorldState()
        {
            HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();
            worldData.Add(new KeyValuePair<string, object>("Has tool", inventory["Has tool"]));
            worldData.Add(new KeyValuePair<string, object>("Tool in range", ToolIsInRange));
            worldData.Add(new KeyValuePair<string, object>("Car ready", BlackBoard.IsCarRdy));
            return worldData;
        }

        public HashSet<KeyValuePair<string, object>> createGoalState()
        {
            HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
            goal.Add(new KeyValuePair<string, object>("Car repaired", true));
            goal.Add(new KeyValuePair<string, object>("Look busy", true));

            return goal;
        }

        public void planFailed(HashSet<KeyValuePair<string, object>> failedGoal)
        {
            
        }

        public void planFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions)
        {
            
        }

        public void actionsFinished()
        {
            
        }

        public void planAborted(GoapAction aborter)
        {
            throw new System.NotImplementedException();
        }

        public bool moveAgent(GoapAction nextAction) // logik der flytter agenten, hvis der skulle være behov for at flytte agenten inden vi kan perform.
        {
            if (previousDestination == nextAction.target.transform.position)
            {
                nextAction.setInRange(true);
                return true;
            }

            agent.SetDestination(nextAction.target.transform.position);

            if (agent.hasPath && agent.remainingDistance < 2)
            {
                nextAction.setInRange(true);
                previousDestination = nextAction.target.transform.position;
                return true;
            }
            else
            {
                return false;
            }
        }

        void Update()
        {
            if (agent.hasPath) // hjælper med at dreje agenten på en realistisk måde.
            {
                Vector3 toTarget = agent.steeringTarget - transform.position;
                float turnAngle = Vector3.Angle(transform.forward, toTarget);
                agent.acceleration = turnAngle * agent.speed;
            }
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.tag == "Tool")
            {
                ToolIsInRange = true;
            }
        }

        void OnTriggerExit(Collider col)
        {
            if (col.tag == "Tool")
            {
                ToolIsInRange = false;
            }
        }
    }
}