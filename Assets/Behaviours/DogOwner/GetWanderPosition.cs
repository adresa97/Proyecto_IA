using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using Unity.VisualScripting;
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
        public NavMeshAgent agent { get; set; }

        [OutParam("newState")]
        [Help("State set this loop")]
        public int newState { get; set; }

        [OutParam("position")]
        [Help("Target position to wander")]
        public Vector3 position { get; set; }

        private NavMeshPath path;

        public override void OnStart()
        {
            path = new NavMeshPath();

            SetRandomDestination();
        }

        public override TaskStatus OnUpdate()
        {
            newState = 0;
            return TaskStatus.COMPLETED;
        }

        private void SetRandomDestination()
        {
            float randomAngle = Random.Range(-visionRange, visionRange);
            Vector3 direction = Quaternion.AngleAxis(randomAngle, Vector3.up) * gameObject.transform.forward * distance;

            Vector3 target = gameObject.transform.position + direction;

            position = target;
        }
    }
}