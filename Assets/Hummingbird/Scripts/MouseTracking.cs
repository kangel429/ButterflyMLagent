using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseTracking : MonoBehaviour
{
    public Canvas canvas;
    public int userId;
    public GameObject myButterfly;
    //public Agent
    public float scrollWheelSensitivity;
    public float pixelToMeterScale;

    private Text mousePositionDisplay;
    private GameObject selectedUser;
    private Vector3 lastMousePosition;
    private Vector3 mousePositionDelta;

    private int numberOfEncounters;

    // Start is called before the first frame update
    void Start()
    {
        mousePositionDisplay = canvas.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePositionDisplay.text = "Right click one";

        if (Input.GetMouseButtonDown(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                if (selectedUser == null) {
                    if (hit.transform.CompareTag("agent")) {
                        userId = int.Parse(hit.transform.gameObject.name.Substring(4));
                        selectedUser = hit.transform.gameObject;
                        lastMousePosition = Input.mousePosition;
                    }
                } else {
                    selectedUser = null;
                }
            }
        }

        // check movement if the user is selected
        if (selectedUser) {
            // mouseScrollDelta = (좌우, 상하)
            float deltaX = 0.0f;
            float deltaY = 0.0f;
            if (Input.GetMouseButtonDown(0)) {
                mousePositionDelta = Input.mousePosition - lastMousePosition;
                Debug.Log($"Input.mousePosition {Input.mousePosition}, lastMousePosition: {lastMousePosition}");

                deltaX = mousePositionDelta.x * pixelToMeterScale;
                deltaY = mousePositionDelta.y * pixelToMeterScale;

                lastMousePosition = Input.mousePosition;
            }

            float deltaZ = Input.mouseScrollDelta.y * scrollWheelSensitivity;

            //Debug.Log($"mouse delta: {deltaX}, {deltaY}, {deltaZ}");

            selectedUser.transform.position += new Vector3(deltaX, deltaY, deltaZ);
            mousePositionDisplay.text = selectedUser.transform.position.ToString();
        }
    }
}
