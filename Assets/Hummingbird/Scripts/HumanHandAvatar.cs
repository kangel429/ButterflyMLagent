using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanHandAvatar : MonoBehaviour
{
    Vector3 worldPosition;
    float circleSize = 0.1f;
    float circleSpeed = 10f;
    float timeValue;
    float circlePositionX;
    float circlePositionZ;
    float gapY;
    public FlowerArea FloatingIsland;
    bool checkMouseButton = true;
    bool checkWheelMouseButton = true;


    bool timsSet;
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

        if (Physics.Raycast(ray, out hitData, 1000, 1 << 11))
        {
            worldPosition = hitData.point;
        }

        if(FloatingIsland.transform.name == "FloatingIsland")
        {
            if (Input.GetMouseButtonDown(0))
            {
                checkWheelMouseButton = false;
                checkMouseButton = !checkMouseButton;
            }

            if (Input.GetMouseButtonDown(2))
            {
                checkWheelMouseButton = !checkWheelMouseButton;
                checkMouseButton = false;
            }

            if (checkMouseButton)
                transform.position = new Vector3(worldPosition.x, worldPosition.y + 1f, worldPosition.z);
            //transform.position = new Vector3(worldPosition.x, 2, worldPosition.z);

            if (checkWheelMouseButton)
            {
                if (timeValue > 999)
                {
                    timeValue = 0;
                }
                else
                {
                    timeValue += 0.1f;
                }
                if (circleSize > 2)
                {
                    timsSet = false;
                }
                if(circleSize < 0)
                {
                    timsSet = true;
                }

                if (timsSet)
                {
                    circleSize += 0.1f * Time.deltaTime;
                }
                else
                {
                    circleSize -= 0.1f * Time.deltaTime;
                }

                circlePositionX = worldPosition.x + Mathf.Sin(Time.deltaTime * timeValue * circleSpeed) * circleSize*0.8f;
                circlePositionZ = worldPosition.z + Mathf.Cos(Time.deltaTime * timeValue * circleSpeed) * circleSize * 0.8f;
                //zPos += forwardSpeed * Time.deltaTime;

                //circleSize += circleGrowSpeed;
                transform.position = new Vector3(circlePositionX, worldPosition.y + 1f + (circleSize), circlePositionZ);

            }

        }

    }


}
