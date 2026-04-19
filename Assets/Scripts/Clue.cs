using Invector.CharacterController;
using UnityEngine;

public class Clue : MonoBehaviour
{

    public string clueName;
    public string description;
    public bool relevant;
    public bool discovered;
    public GameObject clueObj;
    public string inkKnotTitle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

   /* private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject Player = other.gameObject;
            if (Player.GetComponent<vThirdPersonInput>().focused)
            {
                this.discovered = true;
            }
            
        }
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }
}
