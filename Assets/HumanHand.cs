using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanHand : MonoBehaviour
{
    Vector3 worldPosition;

    float numberOfEncounter;

    public GameObject selectObj;
    public CrowdManager crowdManager;
    public List<GameObject> agents ;
    public UserInfo[] mAllusersInfo;
    int selectUserId;

    bool checkMouseButton;
    private void Awake()
    {
        //crowdManager = GameObject.Find("CrowdManager").GetComponent<CrowdManager>();
        
    }
    private void Start()
    {
        numberOfEncounter = 0;
    }


    // Update is called once per frame
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;

        if (Physics.Raycast(ray, out hitData, 1000))
        {
            worldPosition = hitData.point;
        }
        if (Input.GetMouseButtonDown(0))
        {
            checkMouseButton = !checkMouseButton;
        }

        if(checkMouseButton)
        transform.position = new Vector3(worldPosition.x, 1.5f, worldPosition.z);

        ReciveAgentsInfo();
        FindNearestHandavAvatarObj();

    }


    

    float shortDis;
    public GameObject m_NearestButterfly;

    public void FindNearestHandavAvatarObj()
    {


        Collider[] hitColliders = Physics.OverlapSphere(transform.position,1, 1<<10);

        if (hitColliders == null) return;
        //m_NearestButterfly = hitColliders[0].gameObject;
        shortDis = Vector3.Distance(this.gameObject.transform.position, m_NearestButterfly.transform.position);


        for (int i = 0; i < hitColliders.Length; i++)
        {
            Collider found = hitColliders[i];
            float Distance = Vector3.Distance(gameObject.transform.position, found.transform.position);
            if (Distance < shortDis) // 위에서 잡은 기준으로 거리 재기
            {
                m_NearestButterfly = found.gameObject;

                shortDis = Distance;
            }


        }
        if(selectObj != m_NearestButterfly)
        {
            numberOfEncounter = 0;
            //Debug.Log("another select");
            selectObj = m_NearestButterfly;
        }
        else
        {
            if(selectObj!= null)
            {
                if (numberOfEncounter > 50)
                {

                    HummingbirdAgent agent = selectObj.GetComponent<HummingbirdAgent>();
                    UserInfo userInfo = agent.GetComponentInParent<UserInfo>();

                    if(userInfo != null)
                    {
                        for (int i = 0; i < agents.Count; i++)
                        {
                            if (userInfo.userID != mAllusersInfo[i].userID)
                            {
                                mAllusersInfo[i].gameObject.SetActive(false);
                            }
                        }

                        agent.mUserExist = true;
                        agent.selectedUser = true;
                        this.gameObject.SetActive(false);
                        

                    }
                    numberOfEncounter = 0;

                    //if(selectUserId == agents.user)
                }
                else
                {
                   // Debug.Log("select    : " + numberOfEncounter);
                    numberOfEncounter += 10*Time.deltaTime;

                }


        
            }

        }
        float dist = Vector3.Distance(selectObj.transform.position, this.gameObject.transform.position);
        if (dist > 2)
        {
           // Debug.Log("another selecting..");
            numberOfEncounter = 0;
            selectObj = null;
        }

    }

    void ReciveAgentsInfo()
    {
        
        if (agents.Count == 0 && crowdManager.agents != null)
        {
            agents = crowdManager.agents;
            mAllusersInfo = new UserInfo[agents.Count];
            for (int i=0; i< agents.Count; i++)
            {
                mAllusersInfo[i] = agents[i].GetComponent<UserInfo>();

            }
        }
    }
}
