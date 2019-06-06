using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.AI.Mechanic_actions
{
    public class LookingBusy : GoapAction
    {
        private bool complete;
        public Transform[] waypoints;
        private NavMeshAgent agent;
        private int currentWayPoint;

        public LookingBusy()
        {
            addEffect("Look busy", true);
            addPrecondition("Car ready", false);
        }

        void Start()
        {
            this.agent = GetComponent<NavMeshAgent>();
        }
        public override void reset()
        {
            complete = false;
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
            Debug.Log("performing: looking busy");
            // det er her at vores Actions logic bliver udført
            if (BlackBoard.IsCarRdy)
            {
                complete = true;
            }

            if (currentWayPoint == 0 || currentWayPoint > waypoints.Length -1 && this.agent.remainingDistance < 0.2)
            {
                currentWayPoint = 0;
                this.agent.SetDestination(waypoints[currentWayPoint].transform.position);

            }

            // her tilgår vi vores NavMeshAgent og tjekker at han også har en rute. Vi bestemmer også hvor tæt vi mener han skal være på hans mål før han får lov til at udfører sin logik
            // dette kan være brugbart da det ikke altid er praktisk at vores AI skal være 100% på sit punkt.

            if (this.agent.remainingDistance < 0.2)
            {
                
                Debug.Log(currentWayPoint);
                this.agent.SetDestination(waypoints[currentWayPoint].transform.position);
                currentWayPoint++;


            }
            return true;
        }

        public override bool requiresInRange()
        {
            return false;
        }
    }
}