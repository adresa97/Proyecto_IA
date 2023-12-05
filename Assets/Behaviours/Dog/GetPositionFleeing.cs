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
        [Help("This object owner or human who cannot be stolen by this object")]
        public GameObject owner { get; set; }

        [InParam("isStealing")]
        [Help("Boolean that defines if this gameObject is stealing or not")]
        public bool isStealing { get; set; }

        [OutParam("position")]
        [Help("target position away from owner")]
        public Vector3 position { get; set; }

        public override void OnStart()
        {
            if (isStealing)
            {
                position = -(owner.transform.position - gameObject.transform.position).normalized * 10f;
                Debug.Log("Me escapo de papi");
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (!isStealing) return TaskStatus.FAILED;

            return TaskStatus.COMPLETED;
        }
    }
}