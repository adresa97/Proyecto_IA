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

        [InParam("ownerBool")]
        [Help("Component that stores boolean values")]
        public OwnerActions ownerBool { get; set; }

        private bool isCatchable;

        public override void OnStart()
        {
            isCatchable = false;

            if (ownerBool.IsPursuing() && Vector3.Distance(dog.transform.position, gameObject.transform.position) <= 2f)
            {
                isCatchable = true;
                gameObject.SendMessage("SetPursuing", false);
                gameObject.SendMessage("CatchDog");
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (!isCatchable) return TaskStatus.FAILED;

            return TaskStatus.COMPLETED;
        }
    }
}