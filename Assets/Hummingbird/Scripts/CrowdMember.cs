using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMember : MonoBehaviour
{
    public CharacterController controller;
    public float cohesionWeight = 0.1f;
    public float alignmentWeight = 0.1f;
    public float avoidanceWeight = 0.1f;
    public float obstacleAvoidanceForceWeight = 0.1f;
    public CrowdManager crowdManager;
    public List<GameObject> agents;
    public float neighborRadius; // set in the inspector.
    public float avoidanceRadius = 0.1f; // [m]
    public FibonacciRays fibonacciRays;
    int userMask;
    public float movementScalingFactor; // set in the inspector.
    public float maxSpeed; // set in the inspector.
    private void Start()
    {
        userMask = 1 << LayerMask.NameToLayer("user");
        GameObject parentObject = this.gameObject.transform.parent.gameObject; // parentObject is CrowdManager.
        crowdManager = parentObject.GetComponent<CrowdManager>();
        agents = crowdManager.agents;
        fibonacciRays = new FibonacciRays();
    }
    List<GameObject> GetNeighbors(List<GameObject> agents, float neighborRadius) 
    {
        List<GameObject> neighborAgents = new List<GameObject>(); // 비어있는 리스트 생성. 
        foreach (GameObject agent in agents)
        {
            if (Vector3.Distance(this.gameObject.transform.position, agent.transform.position) < neighborRadius)
            {
                if (agent != this.gameObject)
                {
                    neighborAgents.Add(agent);
                }
            }
        }
        return neighborAgents;
    }
    // 'Cohesion' function of the 'Crowd Member' class
    Vector3 CohesionVector()
    {
        Vector3 currentVelocity = new Vector3();
        float agentSmoothTime = 0.5f;
        Vector3 cohMove = Vector3.zero;
        if (GetNeighbors(agents, neighborRadius).Count == 0)
        {
            return Vector3.zero;
        }
        foreach (GameObject a in GetNeighbors(agents, neighborRadius))
        {
            cohMove += a.transform.position;
        }
        cohMove /= GetNeighbors(agents, neighborRadius).Count;
        cohMove -= this.transform.position;
        cohMove = Vector3.SmoothDamp(this.transform.forward, cohMove, ref currentVelocity, agentSmoothTime);
        return cohMove;
    }

    // 'Alignment' function of the 'Crowd Member' class
    Vector3 AlignmentVector()
    {
        Vector3 alignMove = Vector3.zero;
        if (GetNeighbors(agents, neighborRadius).Count == 0)
        {
            return this.transform.forward;
        }
        foreach (GameObject a in GetNeighbors(agents, neighborRadius))
        {
            alignMove += a.transform.forward;
        }
        alignMove /= GetNeighbors(agents, neighborRadius).Count;
        return alignMove;
    }

    // 'Avoidance' function of the 'Crowd Member' class
    Vector3 AvoidanceVector()
    {
        Vector3 avoidMove = Vector3.zero;
        int nAvoid = 0;
        if (GetNeighbors(agents, neighborRadius).Count == 0)
        {
            return Vector3.zero;
        }
        foreach (GameObject a in GetNeighbors(agents, neighborRadius))
        {
            float distance = Vector3.Distance(this.transform.position, a.transform.position);
            if (distance < avoidanceRadius)
            {
                avoidMove += this.transform.position - a.transform.position;
                nAvoid++;
            }
        }
        if (nAvoid > 0)
        {
            avoidMove /= nAvoid;
        }
        return avoidMove;
    }

    // 'Obstacle avoidance' function of the 'Crowd Member' class
    Vector3 AttractVectorToUsers()
    {
        Vector3[] rayDirections = fibonacciRays.directions;
        float raySphereRadius = 0.2f;
        float obstCollisionAvoidDst = 0.2f;

        for (int i = 0; i < rayDirections.Length; i++)
        {
            Vector3 dir = this.transform.TransformDirection(rayDirections[i]);
            Ray ray = new Ray(this.transform.position, dir);
            if (!Physics.SphereCast(ray, raySphereRadius, obstCollisionAvoidDst, userMask))
            // SphereCast(Ray ray, float radius, float maxDistance, int layerMask);
            {
                return dir;
            }
        }
        return Vector3.zero;
    }

    // 'Move' function of the 'Crowd Member' class
    void Move()
    {
        Vector3 move = Vector3.zero;
        int numOfBehavs = 4;
        Vector3[] behaviors = new Vector3[numOfBehavs];
        behaviors[0] = CohesionVector();
        behaviors[1] = AlignmentVector();
        behaviors[2] = AvoidanceVector();
        behaviors[3] = AttractVectorToUsers(); // ObstacleAvoidanceForce -> AttractVectorToUsers : 자기랑 가까운 유저에게 접근한다. 
        float[] weights = new float[numOfBehavs];
        weights[0] = cohesionWeight;
        weights[1] = alignmentWeight;
        weights[2] = avoidanceWeight;
        weights[3] = obstacleAvoidanceForceWeight;
        for (int i = 0; i < behaviors.Length; i++)
        {
            Vector3 partialMove = behaviors[i] * weights[i];
            if (partialMove != Vector3.zero)
            {
                if (partialMove.sqrMagnitude > weights[i] * weights[i])
                {
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }
                move += partialMove;
            }
        }
        move *= movementScalingFactor; 
        if (move.sqrMagnitude > maxSpeed) 
        {
            move = move.normalized * maxSpeed;
        }

        this.controller.Move(move * Time.deltaTime);
    }
    // Update is called once per frame
    void fixedUpdate() 
    {
        Move();
    }
}
