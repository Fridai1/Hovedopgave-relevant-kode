using Assets.Scripts.AI.AI_AGENTS;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.AI.Goap_Actions
{
    public class Patrol : GoapAction
    {
        private bool completed; // har vi genneført vores action
        private int currentWayPoint; // nuværende waypoint
        private NavMeshAgent player; // den component der er ansvarlig for vores agents bevægelse
        public GameObject[] wpoints; // et Array af waypoints, forudbestemt af os i Unity inspectoren. -- Dette er skal automatiseres på et senere tidspunkt.
        
        public Patrol()
        {
            addEffect("Secure area", true); // effekten for actionen
            currentWayPoint = 0;
            completed = false;
        }

        void Start()
        {
            player = GetComponent<NavMeshAgent>(); // GetComponent er en Unity metode, der tillader os at tilgå andre scripts som er tilføjet til Gameobjectet.
        }
        public override void reset()
        {
            completed = false;  // 
        }

        public override bool isDone()
        {
            return completed;
        }

        public override bool checkProceduralPrecondition(GameObject agent)
        {
            return true; // her kan vi tjekke om, om noget i gameverdenen har ændret sig, så vi ikke længere skal kører vores perform.
        }

        public override bool perform(GameObject agent)
        {
           // det er her at vores Actions logic bliver udført

           if (currentWayPoint == 0 || currentWayPoint == wpoints.Length)
            {
                currentWayPoint = 0;
                player.SetDestination(wpoints[currentWayPoint].transform.position);
               
            }

           // her tilgår vi vores NavMeshAgent og tjekker at han også har en rute. Vi bestemmer også hvor tæt vi mener han skal være på hans mål før han får lov til at udfører sin logik
           // dette kan være brugbart da det ikke altid er praktisk at vores AI skal være 100% på sit punkt.
            if (player.hasPath && player.remainingDistance < 1)
            {
                Debug.Log(currentWayPoint);
                player.SetDestination(wpoints[currentWayPoint].transform.position);
                currentWayPoint++;
               
               
            }

            return true;
        }

        public override bool requiresInRange()
        {
            // denne metode, bestemmer hvorvidt vi skal være ved vores @target som kan sættes i unity inspectoren. før vi må udfører vores perform.
            return false;
        }
       
        
        
        // denne metode, bliver kaldt hvis en collider på vores gameobject, rammer en anden collider. Den ramtes collider bliver givet med som parameter.
        void OnTriggerEnter(Collider collision)
        {
            Debug.Log("Patrol: saw player");
            // hvis det gameobject vi rammer er en spiller, er vi færdige med denne action og vi opdatere vores player last seen.
            // vi stopper derudover vores agent i at fortsætte hans nuværende bevægelse.
            if (collision.tag == "Player")
            {
              completed = true;
              BlackBoard.PlayerLastCords = collision.transform.position;
              player.isStopped = true;
            }

        }

        
    }
}