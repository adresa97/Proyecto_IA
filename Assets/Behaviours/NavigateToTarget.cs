using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using UnityEngine.AI;

namespace BBUnity.Actions
{
    [Action("PEC2/Navigation/NavigateToTarget")]
    [Help("Set targetPosition as this gameObject NavAgent destination")]
    public class NavigateToTarget : GOAction
    {
        [InParam("position")]
        [Help("Target position to set as NavAgent destination")]
        public Vector3 position;

        [InParam("agent")]
        [Help("This gameObject NavAgent")]
        public NavMeshAgent agent;

        private NavMeshPath path;

        public override void OnStart()
        {
            path = new NavMeshPath();

            if (agent == null)
            {
                Debug.LogError(gameObject.name + "does not have a NavAgent", gameObject);
                return;
            }

            position.y = 0;
            agent.CalculatePath(position, path);
            agent.destination = position;

            agent.isStopped = false;
        }

        public override TaskStatus OnUpdate()
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) return TaskStatus.COMPLETED;

            return TaskStatus.RUNNING;
        }
    }
}