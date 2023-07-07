using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Tower towerPrefab;

    [SerializeField] bool isPlaceable;

    [SerializeField] Color defaultColor = new Color(0f, .57f, 1f, 0.5f);
    [SerializeField] Color blockedColor = new Color(1f, .57f, 0.1f, 0.5f);

    public bool IsPlaceable { get { return isPlaceable; } }


    GridManager gridManager;
    Pathfinder pathfinder;
    Vector2Int coordinates = new Vector2Int();

    Transform hover;

    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    void Start()
    {
        hover = transform.Find("Hover");
        hover?.gameObject.SetActive(false);

        if (gridManager != null)
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);

            if (!isPlaceable)
            {
                gridManager.BlockNode(coordinates);
            }
        }
    }

    void OnMouseDown()
    {
        if (gridManager.GetNode(coordinates).isWalkable && !pathfinder.WillBlockPath(coordinates))
        {
            bool isSuccessful = towerPrefab.CreateTower(towerPrefab, transform.position);
            if (isSuccessful)
            {
                gridManager.BlockNode(coordinates);
                pathfinder.NotifyReceivers();
                hover?.gameObject.SetActive(false);
            }
        }
    }

    void OnMouseEnter()
    {
        if ((!isPlaceable))
            return;
        if (gridManager.IsNodeBlocked(coordinates))
            return;
        if (pathfinder.WillBlockPath(coordinates))
            hover.gameObject.GetComponent<MeshRenderer>().material.color = blockedColor;
        else
            hover.gameObject.GetComponent<MeshRenderer>().material.color = defaultColor;
        hover?.gameObject.SetActive(true);
    }

    void OnMouseExit()
    {
        hover?.gameObject.SetActive(false);
    }
}
