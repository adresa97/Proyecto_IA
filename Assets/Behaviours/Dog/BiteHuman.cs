using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{
    [Action("PEC2/Dog/SetBool/BiteHuman")]
    [Help("Bite a human")]

    public class BiteHuman : GOAction
    {
        [InParam("owner")]
        [Help("This object owner or human who cannot be bitten by this object")]
        public GameObject owner { get; set; }

        [InParam("biteDistance")]
        [Help("Distance this object can bite from")]
        public float biteDistance { get; set; }

        [OutParam("isBiting")]
        [Help("Boolean that defines if this gameObject is stealing or not")]
        public bool isBiting { get; set; }

        private bool hasChangedStatus;

        public override void OnStart()
        {
            Collider[] humans = Physics.OverlapSphere(gameObject.transform.position, biteDistance);
            int index = 0;
            hasChangedStatus = false;
            while (!isBiting && index < humans.Length)
            {
                if (humans[index].CompareTag("Human") && (humans[index].gameObject != owner))
                {
                    isBiting = true;
                    hasChangedStatus = true;
                    owner.GetComponent<BehaviorExecutor>().SetBehaviorParam("isPursuing", true);
                    Debug.Log("Le he robado la chuleta al pavo este" + humans[index].name);

                }
                index++;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (hasChangedStatus && isBiting) return TaskStatus.COMPLETED;

            return TaskStatus.FAILED;
        }
    }
}