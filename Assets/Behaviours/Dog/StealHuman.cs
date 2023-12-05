using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{
    [Action("PEC2/Dog/SetBool/StealHuman")]
    [Help("Steal from human")]

    public class StealHuman : GOAction
    {
        [InParam("owner")]
        [Help("This object owner or human who cannot be stolen by this object")]
        public GameObject owner { get; set; }

        [InParam("stealDistance")]
        [Help("Distance this object can steal from")]
        public float stealDistance { get; set; }

        [OutParam("isStealing")]
        [Help("Boolean that defines if this gameObject is stealing or not")]
        public bool isStealing { get; set; }

        private bool hasChangedStatus;

        public override void OnStart()
        {
            Collider[] humans = Physics.OverlapSphere(gameObject.transform.position, stealDistance);
            int index = 0;
            hasChangedStatus = false;
            while (!isStealing && index < humans.Length)
            {
                if (humans[index].CompareTag("Human") && (humans[index].gameObject != owner))
                {
                    isStealing = true;
                    hasChangedStatus = true;
                    owner.GetComponent<BehaviorExecutor>().SetBehaviorParam("isPursuing", true);
                    Debug.Log("Le he robado la chuleta al pavo este" + humans[index].name);

                }
                index++;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (hasChangedStatus && isStealing) return TaskStatus.COMPLETED;

            return TaskStatus.FAILED;
        }
    }
}