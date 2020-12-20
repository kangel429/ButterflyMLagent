using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

    string filePath = "/Users/myungjin/Desktop/aI/1/ButterflyMLagent/results/test.txt";
    bool isFile;
    int reciveNum;
    private void Awake()
    {
        //crowdManager = GameObject.Find("CrowdManager").GetComponent<CrowdManager>();
        FileInfo fileInfo = new FileInfo(filePath);

        if (fileInfo.Exists)
        {
            isFile = true;
            reciveNum = int.Parse(ReadTxt(filePath));
        }
        else
        {
            isFile = false;
        }

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
        transform.position = new Vector3(worldPosition.x, worldPosition.y+1f, worldPosition.z);

        ReciveAgentsInfo();
        if (isFile)
        {
            Debug.Log("파일이 있다");
            DoItIfExsistData();
        }
        else
        {
            Debug.Log("파일이 없다");
            FindNearestHandavAvatarObj();

        }

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
                        WriteTxt(filePath, userInfo.userID.ToString());


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


    void DoItIfExsistData()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            mAllusersInfo[i].gameObject.SetActive(false);
        }
        mAllusersInfo[reciveNum].gameObject.SetActive(true);
        HummingbirdAgent agent = mAllusersInfo[reciveNum].GetComponentInChildren<HummingbirdAgent>();
        agent.mUserExist = true;
        agent.selectedUser = true;
        this.gameObject.SetActive(false);
    }

    void WriteTxt(string filePath, string message)
    {

        //FileStream fileStream
        //    = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);

        StreamWriter writer = new StreamWriter(filePath);

        writer.WriteLine(message);
        writer.Close();
    }

    string ReadTxt(string filePath)
    {
        FileInfo fileInfo = new FileInfo(filePath);
        string value = "";

        if (fileInfo.Exists)
        {
            StreamReader reader = new StreamReader(filePath);
            value = reader.ReadToEnd();
            reader.Close();
        }

        else
            value = "파일이 없습니다.";

        return value;
    }
}
