using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanHandAvatar : MonoBehaviour
{
    Vector3 worldPosition;


    private void Start()
    {
        //int random = (int)Random.Range(0, 2);
        //if (random == 0)
        //{
        //    gameObject.SetActive(true);
        //}
        //else
        //{
        //    gameObject.SetActive(false);
        //}

    }

    public void ResetHand()
    {
        int random = (int)Random.Range(0, 2);
        if (random == 0)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
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

       transform.position = new Vector3(worldPosition.x, 2, worldPosition.z);
    }
}
