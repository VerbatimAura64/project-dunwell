using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool locked = true;
    private bool opening = false;
    public bool hinged = true;
    //public GameObject doorObj;
    public GameObject door;
    private float up = 3;
    private float down = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        door = this.transform.GetChild(0).gameObject;
    
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!locked && !hinged)
            {
                opening = true;
                SlideUp();
                //door.SetActive(false);
      //          doorObj.SetActive(false);
            }
            if(!locked && hinged)
            {
                //opening = true;
                //OpenDoor();
                //play locked sound
            }
        }
    }


    public void OpenDoor()
    {
        door.transform.Rotate(0, 90, 0); // Adjust the rotation angles as needed
        this.GetComponent<BoxCollider>().enabled = false; // Disable the collider to allow passage
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!locked)
            {
                opening = false;
                SlideUp();
                //          doorObj.SetActive(true);
            }
        }
    }

    void UnlockDoor()
    {
        locked = false;
        door.GetComponent<Clue>().enabled = true;
        door.GetComponent<BoxCollider>().enabled = true;
        if (!hinged)
        {
            opening = true;
            SlideUp();
        }
    }

    void SlideUp()
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
