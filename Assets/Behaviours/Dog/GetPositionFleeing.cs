using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{
    [Action("PEC2/Dog/Vector3/GetPositionFleeing")]
    [Help("Set a near position away from owner")]

    public class GetPositionFleeing : GOAction
    {
        [InParam("owner")]
        [Help("This object owner or human who cannot be bitten by this object")]
        public GameObject owner { get; set; }

        [InParam("isBiting")]
        [Help("Boolean that defines if this gameObject is biting or not")]
        public bool isBiting { get; set; }

        [OutParam("newState")]
        [Help("State set this loop")]
        public int newState { get; set; }

        [OutParam("position")]
        [Help("target position away from owner")]
        public Vector3 position { get; set; }

        public override void OnStart()
        {
            if (isBiting)
            {
                position = -(owner.transform.position - gameObject.transform.position).normalized * 10f;
                Debug.Log("Me escapo de papi");
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (!isBiting) return TaskStatus.FAILED;

            newState = 2;
            return TaskStatus.COMPLETED;
        }
    }
}