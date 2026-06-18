using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool locked = true;
    private bool opening = false;
    private bool opened = false;
    public bool hinged = true;
    public bool forced = false;
    public bool knocked;
    public bool unlockable;
    //public GameObject doorObj;
    public GameObject door;
    private float left = 0;
    private float down = 02.4f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BldManager"))
        {
            OpenDoor();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        //door = this.transform.GetChild(0).gameObject;
    
    }

    void Update()
    {
        Slide();
    }

    public void OpenDoor()
    {
        if (!locked && !opened)
        {
            //if(door.transform.rotation == Vector3(0f,90f,0f))
            door.transform.Rotate(0, 90, 0); // Adjust the rotation angles as needed
            opened = true;
            this.GetComponent<BoxCollider>().enabled = false; // Disable the collider to allow passage
            this.GetComponent<Door>().enabled = false;
            //GetComponent<Door>().door = null;
        }
    }

    public void UnlockDoor()
    {
        if (Hacked())
        {
            //play success sound
            locked = false;
            OpenDoor();
            this.GetComponent<Door>().enabled = false;
            door.GetComponent<Clue>().enabled = true;
            door.GetComponent<BoxCollider>().enabled = true;
        }
        else
        {
            locked = false;
            //this.GetComponent<Door>().enabled = false;
            door.GetComponent<Clue>().enabled = true;
            door.GetComponent<BoxCollider>().enabled = true;

        }
        if (!hinged)
        {
            opening = true;
            Slide();
        }
    }

    bool Hacked()
    {
        if (forced)
            return true;
        else
            return false;

    }

    void Slide()
    {
        if (!locked && !hinged)
        {
            if (!door.name.Equals("ServiceElevator"))
            {
                float step = .35f;
                if (left >= door.transform.localPosition.x)
                    door.transform.Translate(step * Time.deltaTime * Vector3.right); // Adjust the sliding direction and distance as needed
            }
            else
            {
                if (GameObject.Find("GM").GetComponent<GM>().gameOver)
                {
                    float step = .35f;
                    if (down >= door.transform.localPosition.y)
                        door.transform.Translate(step * Time.deltaTime * Vector3.up);
                }
            }
        }
           

    }
}
