using Invector.CharacterController;
using UnityEngine;

public class Clue : MonoBehaviour
{
    [SerializeField]
    private GM gm;
    [SerializeField]
    private ClueCard clueCard;
    public GameObject clueCardObj;
    public string clueName;
    public string description;
    public bool relevant;
    public bool discovered;
    public GameObject clueObj;
    public string inkKnotTitle;
    public int[] inkChoice;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        clueObj = this.gameObject;
        this.gameObject.name = clueName;
        gm = GameObject.FindWithTag("GameController").GetComponent<GM>();
        clueCard = ScriptableObject.CreateInstance<ClueCard>();
        clueCard.clueName = clueName;
        clueCard.description = description;
        clueCard.clueObj = clueObj;

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

}
