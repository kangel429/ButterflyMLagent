using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrowdManager : MonoBehaviour
{
    public int agentStartingCount = 1000;
    public List<GameObject> agents = new List<GameObject>();
    public GameObject agentPrefab;


    public int crowdStartingCount = 20;
    public List<GameObject> crowds = new List<GameObject>();
    public GameObject crowdPrefab;
    public GameObject crowdParent;




    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < agentStartingCount; i++)
        {
            GameObject newAgent = Instantiate(
                agentPrefab,
                new Vector3(0, 2, 0),
                Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)),
                this.transform
                );
            newAgent.name = "AgentFly " + i;
            agents.Add(newAgent);
        }

        for (int i = 0; i < crowdStartingCount; i++)
        {
            GameObject newCrowd = Instantiate(
                crowdPrefab,
                new Vector3(0, 0, 0),
                Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)),
                this.transform
                );
            newCrowd.name = "CrowdFly " + i;
            newCrowd.transform.parent = crowdParent.transform;
            crowds.Add(newCrowd);
        }




    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
