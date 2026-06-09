using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviour
{
    private Clue _firstSelected = null;
    private GM GM;

    private void Awake()
    {
        GM = GameObject.Find("GameController").GetComponent<GM>();
    }

    private void OnCardClicked(Clue clueCard)
    {
        if (_firstSelected == null)
        {
            _firstSelected = clueCard;
            clueCard.SetSelected(true);
        }
        else if (_firstSelected == clueCard)
        {
            _firstSelected.SetSelected(false);
            _firstSelected = null;
        }
        else
        {
            TryConnect(_firstSelected, clueCard);
            _firstSelected.SetSelected(false);
            _firstSelected = null;
        }

    }

    private void TryConnect(Clue a, Clue b)
    {
        string connectionKey = GetConnectionKey(a.name, b.name);

        if (validConnections.ContainsKey(connectionKey))
        {
            GM.TriggerClueKnot(validConnections[connectionKey]);
            DrawConnection(a, b);
        }

    }

    private string GetConnectionKey(string clueA, string clueB)
    {
        string[] sorted = new string[] { clueA, clueB };
        System.Array.Sort(sorted);
        return sorted[0] + "_" + sorted[1];
    }

    private Dictionary<string, string> validConnections = new Dictionary<string, string>()
    {

    };

    private void DrawConnection(Clue a, Clue b)
    {
        GameObject lineObj = new GameObject("ConnectionLine");
        //lineObj.transform.SetParent(connectContainer, false);



        Image line = lineObj.AddComponent<Image>();
        //line.color = connectionColor;

        RectTransform lineRect = lineObj.GetComponent<RectTransform>();

        Vector2 posA = a.GetComponent<RectTransform>().anchoredPosition;
        Vector2 posB = b.GetComponent<RectTransform>().anchoredPosition;


        /*
        line.positionCount = 2;
        line.startWidth = 2f;
        line.endWidth = 2f;
        //line.material = connectionLineMaterial;

        line.SetPosition(0, a.transform.position);
        line.SetPosition(1, b.transform.position);
    */}
}
