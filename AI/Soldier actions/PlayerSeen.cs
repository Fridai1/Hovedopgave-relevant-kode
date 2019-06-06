using System.Collections;
using System.Collections.Generic;
using System.Net;
using Assets.Scripts.AI.AI_AGENTS.soldier;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.AI.Goap_Actions
{
    public class PlayerSeen : GoapAction
    {
        private NavMeshAgent agent;
        private Vector3 Playercords;
        private bool complete;
        private bool playerSeen;
        private float startTime;
        private Vector3 playercords;
        private Transform playerLastKnownTransform;
        public GameObject instantiateThis;
        int index;
        private List<PatrolPoints> lookhere;

        public PlayerSeen()
        {
            addEffect("Found player", true);
            addPrecondition("Secure area", true);
        }

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            complete = false;
            lookhere = new List<PatrolPoints>();

        }
        public override void reset()
        {
            complete = false;
             startTime = 0f;
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
            // hvis start time er 0 er det fordi vi ser spilleren lige efter patrulje og skal derfor reagere anderledes.
            if (startTime == 0)
            {
                startTime = Time.time;
                agent.transform.LookAt(BlackBoard.PlayerLastCords);
            }
            // her venter vi 0.5 sekund før vi fortsætter med at følge efter spilleren. og vi følger kun efter så længe vi kan se ham.
            if (Time.time - startTime > 0.5f && playerSeen)
            {
                this.agent.isStopped = false;
                startTime = 0;
                this.agent.SetDestination(BlackBoard.PlayerLastCords);
                Debug.Log("found agent");

                // hvis vi når tæt nok på spilleren anser vi denne action som værende færdig
                if (this.agent.hasPath && this.agent.remainingDistance < 0.5)
                {
                    complete = true;
                }
            }
            // når spilleren ikke længere er set spawner vi 5 bolde og går til dem i en vilkårlig rækkefølge.
            if (!playerSeen)
            {
                Debug.Log("looking for lost agent");
                startTime = Time.time;
                this.agent.isStopped = false;

                if (lookhere.Count == 0)
                {
                    this.agent.SetDestination(BlackBoard.PlayerLastCords);
                    lookhere = GetComponent<SoldierManager>().InvestigateArea(playerLastKnownTransform, 5, 7);
                    foreach (var l in lookhere)
                    {
                        Instantiate(instantiateThis, l.PatrolPointPosition, Quaternion.Euler(Vector3.forward));
                    }
                }
                
                // vi har bestemt at boldende skal spawner relativt tæt på hinanden og er derfor også nød til at skærme grænsen for hvornår en agent har nået bolden, får ikke at den bare står stille
                // og mener den har besøgt dem alle.
                if (this.agent.hasPath && this.agent.remainingDistance < 0.1f)
                {
                    if (index == lookhere.Count)
                    {
                        // et reset
                        lookhere.Clear();
                        index = 0;
                        complete = true;
                        
                    }
                    else
                    {
                        // udførelsen
                        this.agent.SetDestination(lookhere[index].PatrolPointPosition);

                        Debug.Log("going to " + lookhere[index].PatrolPointPosition);
                        index++;
                    }
                }
            }
            return true;
        }


        public override bool requiresInRange()
        {
            return false;
        }

        void OnTriggerStay(Collider col)
        {
            // så længe objectet bliver i vores collider, er PlayerSeen true og vi opdatere konstant vores blackboard
            playerSeen = true;
            BlackBoard.PlayerLastCords = col.transform.position;
            Debug.Log("stay");
        }

        void OnTriggerExit(Collider col)
        {
            // når spilleren forlader vores collider kan han ikke længere ses og hans sidste koordinator bliver brugt til at finde ham.
            playerSeen = false;
            playerLastKnownTransform = col.transform;
            Debug.Log("exit");
        }
    }
}