using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace BBUnity.Actions
{
    [Action("PEC2/Owner/Vector3/GetWanderPosition")]
    [Help("Get a random position near this gameObject inside a vision range")]

    public class GetWanderPosition : GOAction
    {
        [InParam("visionRange")]
        [Help("Angle of view of this object")]
        public float visionRange { get; set; }

        [InParam("distance")]
        [Help("Distance at wich wander point will be set")]
        public int distance { get; set; }

        [InParam("agent")]
        [Help("This gameObject NavAgent")]
        public NavMeshAgent agent { get; set;  }

        [OutParam("position")]
        [Help("Target position to wander")]
        public Vector3 position { get; set; }

        public override void OnStart()
        {
            float randomAngle = Random.Range(-visionRange, visionRange);
            Vector3 target = gameObject.transform.position + new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)).normalized * distance;

            position = target;
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.COMPLETED;
        }
    }
}