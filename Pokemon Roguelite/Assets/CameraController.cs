using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;

    [SerializeField] float movementSpeed;
    [SerializeField] float movementTime;
    [SerializeField] float rotationAmount;
    [SerializeField] Vector3 zoomAmount;

    [SerializeField] Vector3 newPosition;
    [SerializeField] Quaternion newRotation;
    [SerializeField] Vector3 newZoom;

    [SerializeField] Vector2 startPanLimit;
    [SerializeField] Vector2 endPanLimit;
    [SerializeField] float maxZoomAmount;
    Vector3 minZoomLimit;
    Vector3 maxZoomLimit;

    //[SerializeField] float panSpeed = 20f;
    //[SerializeField] float panBorderThickness = 10f;
    //[SerializeField] float scrollSpeed = 2000f;
    //[SerializeField] Vector2 heightLimit = new Vector2(20, 120);

    void Start()
    {
        cameraTransform = transform.GetChild(0).GetComponent<Transform>();
        maxZoomLimit = new Vector3(0, cameraTransform.localPosition.y - maxZoomAmount, cameraTransform.localPosition.z + maxZoomAmount);/*cameraTransform.localPosition + cameraTransform.forward * maxZoomAmount;*/
        minZoomLimit = new Vector3(0, cameraTransform.localPosition.y + maxZoomAmount, cameraTransform.localPosition.z - maxZoomAmount); /*cameraTransform.localPosition + (cameraTransform.forward * -1) * maxZoomAmount;*/

        Debug.Log(maxZoomLimit);
        Debug.Log(minZoomLimit);

        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    void Update()
    {
        HandleMovementInput();

        //Vector3 pos = transform.position;

        //if(Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        //{
        //    pos.z += panSpeed * Time.deltaTime;
        //}
        //if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        //{
        //    pos.z -= panSpeed * Time.deltaTime;
        //}
        //if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        //{
        //    pos += transform.right * panSpeed * Time.deltaTime;
        //}
        //if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        //{
        //    pos -= transform.right * panSpeed * Time.deltaTime;
        //}

        //float scroll = Input.GetAxis("Mouse ScrollWheel");
        //pos -= transform.forward * scroll * scrollSpeed * Time.deltaTime;

        //pos.x = Mathf.Clamp(pos.x, 0, panLimit.x);
        //pos.y = Mathf.Clamp(pos.y, heightLimit.x, heightLimit.y);
        //pos.z = Mathf.Clamp(pos.z, 0, panLimit.y);

        //transform.position = pos;
    }

    void HandleMovementInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -movementSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * movementSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -movementSpeed);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            newZoom += Input.GetAxis("Mouse ScrollWheel") * zoomAmount;
        }

        newPosition.x = Mathf.Clamp(newPosition.x, startPanLimit.x, endPanLimit.x);
        newPosition.z = Mathf.Clamp(newPosition.z, startPanLimit.y, endPanLimit.y);

        newZoom.z = Mathf.Clamp(newZoom.z, minZoomLimit.z, maxZoomLimit.z);
        newZoom.y = Mathf.Clamp(newZoom.y, maxZoomLimit.y, minZoomLimit.y);

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }
}
