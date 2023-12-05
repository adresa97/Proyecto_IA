using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;

namespace BBUnity.Actions
{
    [Action("PEC2/Owner/Vector3/GetDogPosition")]
    [Help("Get this owner's dog position")]

    public class GetDogPosition : GOAction
    {
        [InParam("dog")]
        [Help("This owner's dog")]
        public GameObject dog { get; set; }

        [InParam("isPursuing")]
        [Help("Boolean value that defines if this owner is pursuing its dog or not")]
        public bool isPursuing { get; set; }

        [OutParam("position")]
        [Help("This gameObject target position")]
        public Vector3 position { get; set; }

        public override void OnStart()
        {
            if (isPursuing) position = dog.transform.position;
        }

        public override TaskStatus OnUpdate()
        {
            if (!isPursuing) return TaskStatus.FAILED;

            return TaskStatus.COMPLETED;
        }
    }
}