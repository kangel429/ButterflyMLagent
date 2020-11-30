using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrowdManager : MonoBehaviour
{
    public int startingCount = 1000;
    public List<GameObject> agents = new List<GameObject>();
    public GameObject agentPrefab;
    public int agentDensity = 100;
    public float yPos = 0.1f; 

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < startingCount; i++)
        {
            GameObject newAgent = Instantiate(
                agentPrefab,
                new Vector3(Random.insideUnitCircle.x * agentDensity, yPos, Random.insideUnitCircle.y * agentDensity),
                Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)),
                this.transform
                );
            newAgent.name = "Agent " + i;
            agents.Add(newAgent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
