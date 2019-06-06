using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Assets.Scripts.AI.AI_AGENTS
{
    public class Soldier : MonoBehaviour, IGoap
    {
        private NavMeshAgent agent;
        private Vector3 previousDestination;



        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        public HashSet<KeyValuePair<string, object>> getWorldState() // her sætter vi vores worldstates
        {
            HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();
            worldData.Add(new KeyValuePair<string, object>("Secure area", false));
            worldData.Add(new KeyValuePair<string, object>("Found player", false));
            return worldData;
        }

        public HashSet<KeyValuePair<string, object>> createGoalState() // her sætter vi vores goalstates
        {
            HashSet<KeyValuePair<string,object>> goal = new HashSet<KeyValuePair<string, object>>();
            goal.Add(new KeyValuePair<string, object>("Secure area", true));
            goal.Add(new KeyValuePair<string, object>("Found player", true));
            return goal;
        }

        public void planFailed(HashSet<KeyValuePair<string, object>> failedGoal) // vi har ikke implementeret kode der, håndtere planer der fejler. da dette ikke er kritisk for funktionaliteten
        {
            Debug.Log(failedGoal);
        }

        public void planFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions) // vi har ikke implementeret kode der, håndtere om der skal ske noget specielt når en plan bliver fundet. da dette ikke er kritisk for funktionaliteten
        {
           // throw new System.NotImplementedException();
           
        }

        public void actionsFinished()
        {
            
        }

        public void planAborted(GoapAction aborter)
        {
            
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