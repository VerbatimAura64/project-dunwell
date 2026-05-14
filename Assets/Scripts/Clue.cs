using Invector.CharacterController;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Clue : MonoBehaviour
{
    [SerializeField]
    private GM gm;
    public GameObject clueCardObj;
    public string clueName;
    public string description;
    public bool relevant;
    public bool discovered;
    public GameObject clueObj;
    public string inkKnotTitle;
    public int[] inkChoice;

    #region ClueCard Variables
    [Header("Clue Card Variables")]
    public TextMeshProUGUI cardClueName;
    public Image cardClueImage;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        clueObj = this.gameObject;
        this.gameObject.name = clueName;
        gm = GameObject.FindWithTag("GameController").GetComponent<GM>();
        if (clueCardObj != null)
        {
            cardClueName = GameObject.Find("ClueName").GetComponent<TextMeshProUGUI>();
            cardClueName.text = clueName;
            clueCardObj.GetComponent<Clue>().cardClueName = cardClueName;
            clueCardObj.GetComponent<Clue>().cardClueName.text = clueName;
            //clueCardObj = GameObject.FindWithTag("ClueCard");
        } else
        {
            
        }

        

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
