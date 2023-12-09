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

        [InParam("dogBool")]
        [Help("Component that stores boolean values")]
        public DogActions dogBool { get; set; }

        [OutParam("newState")]
        [Help("State set this loop")]
        public int newState { get; set; }

        [OutParam("position")]
        [Help("target position away from owner")]
        public Vector3 position { get; set; }

        public override void OnStart()
        {
            if (dogBool.IsBiting())
            {
                position = gameObject.transform.position - (owner.transform.position - gameObject.transform.position).normalized * 3f;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (!dogBool.IsBiting()) return TaskStatus.FAILED;

            newState = 2;
            return TaskStatus.COMPLETED;
        }
    }
}