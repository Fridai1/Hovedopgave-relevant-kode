using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.AI.AI_AGENTS.Damaged_Car
{
    public class Car : MonoBehaviour, IGoap
    {
        private NavMeshAgent agent;
        private Vector3 previousDestination;
        public HashSet<KeyValuePair<string, object>> getWorldState()
        {
            throw new System.NotImplementedException();
        }

        public HashSet<KeyValuePair<string, object>> createGoalState()
        {
            throw new System.NotImplementedException();
        }

        public void planFailed(HashSet<KeyValuePair<string, object>> failedGoal)
        {
            throw new System.NotImplementedException();
        }

        public void planFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions)
        {
            throw new System.NotImplementedException();
        }

        public void actionsFinished()
        {
            throw new System.NotImplementedException();
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
    }
}