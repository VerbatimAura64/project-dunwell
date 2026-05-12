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
    public string[] inkChoice;
    [SerializeField]
    private GM gm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        clueObj = this.gameObject;
        this.gameObject.name = clueName;
        gm = GameObject.FindWithTag("GameController").GetComponent<GM>();

    }

    [CreateAssetMenu(fileName = "ClueCard", menuName = "ClueCard")]
    public class ClueCard : ScriptableObject
    {
        public string clueName;
        public string description;
        public GameObject clueObj;
        public Sprite clueSprite;
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
