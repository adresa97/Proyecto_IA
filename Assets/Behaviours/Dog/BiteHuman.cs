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

        [InParam("dogBool")]
        [Help("Component that stores boolean values")]
        public DogActions dogBool { get; set; }

        private bool hasChangedStatus;

        public override void OnStart()
        {
            Collider[] humans = Physics.OverlapSphere(gameObject.transform.position, biteDistance);
            int index = 0;
            hasChangedStatus = false;
            while (!dogBool.IsBiting() && index < humans.Length)
            {
                if (humans[index].CompareTag("Human") && (humans[index].gameObject != owner))
                {
                    gameObject.SendMessage("SetBiting", true);
                    hasChangedStatus = true;
                    gameObject.SendMessage("AlarmOwner");
                }
                index++;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (hasChangedStatus && dogBool.IsBiting()) return TaskStatus.COMPLETED;

            return TaskStatus.FAILED;
        }
    }
}