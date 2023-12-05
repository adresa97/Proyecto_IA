using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{
    [Action("PEC2/Dog/Vector3/GetPositionToSteal")]
    [Help("If a stealable human is detected, set its position as target")]

    public class GetPositionToSteal : GOAction
    {
        [InParam("owner")]
        [Help("This object owner or human who cannot be stolen by this object")]
        public GameObject owner { get; set; }

        [InParam("stealDistance")]
        [Help("Distance this object can steal from")]
        public float stealDistance { get; set; }

        [OutParam("position")]
        [Help("Human position to steal")]
        public Vector3 position { get; set; }

        private bool canSteal;

        public override void OnStart()
        {
            Collider[] humans = Physics.OverlapSphere(gameObject.transform.position, 3f);
            int index = 0;
            canSteal = false;
            while (!canSteal && index < humans.Length)
            {
                if (humans[index].CompareTag("Human") && (humans[index].gameObject != owner))
                {
                    Vector3 separation = (humans[index].transform.position - gameObject.transform.position).normalized * stealDistance;
                    position = humans[index].transform.position - separation;
                    canSteal = true;
                    Debug.Log("He encontrado carne fresca" + humans[index].name);
                }
                index++;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (!canSteal) return TaskStatus.FAILED;

            return TaskStatus.COMPLETED;
        }
    }
}