using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using System.Linq;

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
            Collider[] humans = Physics.OverlapSphere(gameObject.transform.position, biteDistance).Where((col) => col.CompareTag("Human") && col.gameObject != owner).ToArray();
            hasChangedStatus = false;
            if (!dogBool.IsBiting() && humans.Length > 0)
            {
                gameObject.SendMessage("SetBiting", true);
                hasChangedStatus = true;
                gameObject.SendMessage("AlarmOwner");
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (hasChangedStatus) return TaskStatus.COMPLETED;

            return TaskStatus.FAILED;
        }
    }
}