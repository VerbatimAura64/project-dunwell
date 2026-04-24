using UnityEngine;

public class Door : MonoBehaviour
{
    public bool locked = true;
    private bool opening = false;
    //public GameObject doorObj;
    public GameObject door;
    private float up = 3;
    private float down = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!locked)
            {
                opening = true;
                SlideDoor();
                //door.SetActive(false);
      //          doorObj.SetActive(false);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!locked)
            {
                opening = false;
                SlideDoor();
                //          doorObj.SetActive(true);
            }
        }
    }

    void UnlockDoor()
    {
        locked = false;
    }

    void SlideDoor()
    {
        if (opening)
        {
            float step = .2f;
            if(door.transform.position.y <= up)
                door.transform.Translate(Vector3.up * step * Time.deltaTime); // Adjust the sliding direction and distance as needed
        }
        else
        {
            float step = 1f * Time.deltaTime;
            //while(door.transform.position.y >= down)
                door.transform.position =  Vector3.MoveTowards(door.transform.position, new Vector3(door.transform.position.x, down, door.transform.position.z), step); // Adjust the sliding direction and distance as needed
        }

    }





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
