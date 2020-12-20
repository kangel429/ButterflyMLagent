using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanHandAvatar : MonoBehaviour
{
    Vector3 worldPosition;
    float circleSize = 1f;
    float circleSpeed = 1.5f;
    float timeValue;
    float circlePositionX;
    float circlePositionZ;

    public FlowerArea FloatingIsland;
    bool checkMouseButton = true;
    private void Start()
    {
        FloatingIsland = gameObject.GetComponentInParent<FlowerArea>();
//        Debug.Log(FloatingIsland.transform.name);

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

        if(FloatingIsland.transform.name == "FloatingIsland")
        {
            if (Input.GetMouseButtonDown(0))
            {
                checkMouseButton = !checkMouseButton;
            }

            if (checkMouseButton)
                transform.position = new Vector3(worldPosition.x, worldPosition.y + 1f, worldPosition.z);
            //transform.position = new Vector3(worldPosition.x, 2, worldPosition.z);

        }

    }


}
