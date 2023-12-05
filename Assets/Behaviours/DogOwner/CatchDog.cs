using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;

namespace BBUnity.Actions
{
    [Action("PEC2/Owner/SetBool/CatchDog")]
    [Help("Reset owner and dog status")]

    public class CatchDog : GOAction
    {
        [InParam("dog")]
        [Help("This owner's dog")]
        public GameObject dog { get; set; }

        [OutParam("isPursuing")]
        [Help("Value that defines wether this owner is pursuing its dog or not")]
        public bool isPursuing { get; set; }

        private bool isCatchable;

        public override void OnStart()
        {
            isCatchable = false;

            if (isPursuing && Vector3.Distance(dog.transform.position, gameObject.transform.position) <= 1f)
            {
                isCatchable = true;
                isPursuing = false;
                dog.GetComponent<BehaviorExecutor>().SetBehaviorParam("isStealing", false);
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (!isCatchable) return TaskStatus.FAILED;

            return TaskStatus.COMPLETED;
        }
    }
}