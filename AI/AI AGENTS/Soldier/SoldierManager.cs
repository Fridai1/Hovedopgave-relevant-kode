using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.AI.AI_AGENTS.soldier
{



    public class PatrolPoints
    {
       
        public PatrolPoints()
        {
        }

        public Vector3 PatrolPointPosition;//patrol pointets position


        public void SetRandomPos(Vector3 v3Pos, float Range) // metode til at sætte en tilfældig position baseret på en central location og en radius.
        {
            Vector2 v2RandPosition = Random.insideUnitCircle * Range;
            PatrolPointPosition = v3Pos += new Vector3(v2RandPosition.x, 0, v2RandPosition.y);
        }
    }
    public class SoldierManager : MonoBehaviour
    {

        public List<PatrolPoints> patrolPointList;//en liste af patrol points

        private Vector3 patrolCenter;//Center patrol pointet

        [SerializeField]
        private int amountOfPatrolPoints;//mængden af patrol points

        private NavMeshAgent agentRef;//Reference til Navmesh agent


        // Start is called before the first frame update
        private void Start()
        {
            patrolPointList = new List<PatrolPoints>();
        }

       
        public List<PatrolPoints> InvestigateArea(Transform PositionToInvestigate, int AmountOfPoints, float RangeToInvestigate)
        {
           
            patrolCenter = PositionToInvestigate.position;
            patrolPointList.Clear();
            GeneratePositions(AmountOfPoints, RangeToInvestigate); //laver vores random points
            

            return patrolPointList;

        }

        
        private void GeneratePositions(int AmountOfPoints, float Range)
        {
            for (int i = 0; i < AmountOfPoints; i++)//Looper igennem points
            {
                PatrolPoints ppPatrolPlaceholder = new PatrolPoints();//
                bool bCanReachTarget = false;//Kan vores agent nå pointet
                while (bCanReachTarget == false)
                {
                    ppPatrolPlaceholder.SetRandomPos(patrolCenter, Range); 

                    NavMeshPath nmpPath = new NavMeshPath();
                    agentRef.CalculatePath(ppPatrolPlaceholder.PatrolPointPosition, nmpPath);

                    if (nmpPath.status == NavMeshPathStatus.PathPartial || nmpPath.status == NavMeshPathStatus.PathInvalid)
                    {
                        Debug.Log("Bad Point");
                    }
                    else
                    {
                        bCanReachTarget = true;
                    }
                }
                patrolPointList.Add(ppPatrolPlaceholder);
            }
        }

        

       
    }
}