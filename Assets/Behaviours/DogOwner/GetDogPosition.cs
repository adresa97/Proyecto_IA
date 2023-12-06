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

        [InParam("ownerBool")]
        [Help("Component that stores boolean values")]
        public OwnerActions ownerBool { get; set; }

        [OutParam("newState")]
        [Help("State set this loop")]
        public int newState { get; set; }

        [OutParam("position")]
        [Help("This gameObject target position")]
        public Vector3 position { get; set; }

        public override void OnStart()
        {
            if (ownerBool.IsPursuing()) position = dog.transform.position;
        }

        public override TaskStatus OnUpdate()
        {
            if (!ownerBool.IsPursuing()) return TaskStatus.FAILED;

            newState = 1;
            return TaskStatus.COMPLETED;
        }
    }
}