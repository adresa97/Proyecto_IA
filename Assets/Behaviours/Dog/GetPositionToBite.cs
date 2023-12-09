using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using System.Linq;

namespace BBUnity.Actions
{
    [Action("PEC2/Dog/Vector3/GetPositionToBite")]
    [Help("If a bitable human is detected, set its position as target")]

    public class GetPositionToBite : GOAction
    {
        [InParam("owner")]
        [Help("This object owner or human who cannot be bitten by this object")]
        public GameObject owner { get; set; }

        [InParam("bitableDistance")]
        [Help("Distance from which this object can select a target to bite")]
        public float bitableDistance { get; set; }

        [OutParam("newState")]
        [Help("State set this loop")]
        public int newState { get; set; }

        [OutParam("position")]
        [Help("Human position to steal")]
        public Vector3 position { get; set; }

        private bool canBite;

        public override void OnStart()
        {
            Collider[] humans = Physics.OverlapSphere(gameObject.transform.position, bitableDistance).Where((col) => col.CompareTag("Human") && col.gameObject != owner).ToArray();
            canBite = false;
            if (humans.Length > 0)
            {
                position = humans[0].transform.position;
                canBite = true;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (!canBite) return TaskStatus.FAILED;

            newState = 1;
            return TaskStatus.COMPLETED;
        }
    }
}