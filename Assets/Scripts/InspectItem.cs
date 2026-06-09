using UnityEngine;

public class InspectItem : MonoBehaviour
{
    public Transform objToInspect;
    public float rotateSpeed = 1f;
    private Vector3 previousMousePosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objToInspect = GetComponent<Clue>().clueObj.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previousMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0)) 
        { 
            Vector3 deltaMousePosition = Input.mousePosition - previousMousePosition;
            float rotationX = (deltaMousePosition.y * rotateSpeed * Time.deltaTime);
            float rotationY = -(deltaMousePosition.x * rotateSpeed * Time.deltaTime);

            Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
            objToInspect.rotation = rotation * objToInspect.rotation;

            previousMousePosition = Input.mousePosition;
        }
    }
}
