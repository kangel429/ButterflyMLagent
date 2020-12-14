﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanHand : MonoBehaviour
{
    Vector3 worldPosition;

    int numberOfEncounter;

    public GameObject selectObj;
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

        transform.position = new Vector3(worldPosition.x, 1.5f, worldPosition.z);


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
            Debug.Log("another select");
            selectObj = m_NearestButterfly;
        }
        else
        {
            if(selectObj!= null)
            {
                if (numberOfEncounter > 50)
                {

                    HummingbirdAgent agent = selectObj.GetComponent<HummingbirdAgent>();
                    agent.mUserExist = true;
                    this.gameObject.SetActive(false);
                    //numberOfEncounter = 0;
                }
                else
                {
                    Debug.Log("select");
                    numberOfEncounter++;
                }


        
            }

        }
        float dist = Vector3.Distance(selectObj.transform.position, this.gameObject.transform.position);
        if (dist > 2)
        {
            Debug.Log("another selecting..");
            numberOfEncounter = 0;
            selectObj = null;
        }

    }
}