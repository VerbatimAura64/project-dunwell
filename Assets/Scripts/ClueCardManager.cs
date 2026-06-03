using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

//[CreateAssetMenu(fileName = "ClueCard", menuName = "ClueCard")]
public class ClueCardManager : MonoBehaviour
{
    public GM gm;

    [SerializeField] private int maxClueCards;

    [SerializeField] private GameObject clueCardPrefab;

    [SerializeField] private SplineContainer clueCardPath;

    [SerializeField] private Transform cardSpawnpoint;

    public List<GameObject> clueCards;

    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GM>();
    }

    public void UpdateCardPosition()
    {
        if (clueCards.Count == 0) return;
        float cardSpacing = 1f/clueCards.Count;
        float firstCardPosition = 0.5f - (clueCards.Count - 1) * cardSpacing/2;
        Spline spline = clueCardPath.Spline;

        for(int i = 0; i < clueCards.Count; i++)
        {
            float t = firstCardPosition + i * cardSpacing;
            Vector3 position = spline.EvaluatePosition(t);
            clueCards[i].transform.position = position;
            //clueCards[i].transform.rotation = Quaternion.LookRotation(spline.EvaluateTangent(t));
        }
    }

    void Update()
    {
        //UpdateCardPosition();
        /*for(int i = 0; i < gm.clueCards.Count; i++)
        {
            if (clueCards[i] == null)
            {
                clueCards.Add(gm.clueCards[i]);
            }
        }*/

    }


}
