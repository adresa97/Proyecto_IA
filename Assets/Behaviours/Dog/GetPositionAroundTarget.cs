using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;

namespace BBUnity.Actions
{
    [Action("PEC2/Dog/Vector3/GetPositionAroundTarget")]
    [Help("Gets a random position near a target game object")]

    public class GetPositionAroundTarget : GOAction
    {
        [InParam("owner")]
        [Help("Target game object")]
        public GameObject owner { get; set; }

        [InParam("radius")]
        [Help("Radius around target")]
        public float radius { get; set; }

        [OutParam("newState")]
        [Help("State set this loop")]
        public int newState { get; set; }

        [OutParam("position")]
        [Help("Random position set inside a circle of radius value around target")]
        public Vector3 position { get; set; }

        public override void OnStart()
        {
            if (owner == null)
            {
                Debug.LogError("There's no target selected", gameObject);
                return;
            }

            Vector3 random = Random.insideUnitSphere;
            random *= radius;
            random += owner.transform.position;

            position = random;

            Debug.Log("Me muevo alrededor de papi");
        }

        public override TaskStatus OnUpdate()
        {
            newState = 0;
            return TaskStatus.COMPLETED;
        }
    }
}