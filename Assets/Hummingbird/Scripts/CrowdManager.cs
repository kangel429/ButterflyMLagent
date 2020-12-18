using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Random = UnityEngine.Random;


using Unity.Barracuda;
using UnityEngine.Serialization;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors.Reflection;
public class CrowdManager : MonoBehaviour
{

    public int userIndex = 2;
    public int agentStartingCount = 1;
    public List<GameObject> agents = new List<GameObject>();
    public GameObject agentPrefab;


    public int crowdStartingCount = 120;
    public List<GameObject> crowds = new List<GameObject>();
    public GameObject crowdPrefab;
    public GameObject crowdParent;


    public Material[] agentMaterial;

     


    [Tooltip("The color when the flower is full")]
    public Color[] butterflyColors;

    public NNModel[] m_Model;

    //    HummingbirdAgent agent;
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
            UserInfo userInfo = newAgent.GetComponent<UserInfo>();
            Material agentMaterial = newAgent.GetComponentInChildren<SkinnedMeshRenderer>().material;
            Material handMaterial = newAgent.transform.Find("HandAvatar").GetComponent<MeshRenderer>().material;
            userInfo.userID = i;
            agentMaterial.color = butterflyColors[i];
            handMaterial.color = butterflyColors[i];
            ReciveIndex reciveIndex = newAgent.GetComponentInChildren<ReciveIndex>();
            newAgent.transform.rotation = Quaternion.Euler(0, 0, 0);

            if (i < userIndex)
            {
                reciveIndex.userDetect = true;
                reciveIndex.m_Model = m_Model[i];
            }
            else
            {
                reciveIndex.userDetect = false;
                reciveIndex.m_Model = m_Model[i];

            }


            //Debug.Log(butterflyColors[i]);

            agents.Add(newAgent);
            //agentPrefab.SetActive(false);

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

        agentPrefab.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
