using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePadController : MonoBehaviour
{
    public List<GameObject> linkedCells;

    private GameObject indicator;

    void Start()
    {
        indicator = transform.GetChild(0).gameObject;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            foreach(GameObject cell in linkedCells)
            {
                cell.transform.GetChild(1).gameObject.SetActive(false);
            }
            indicator.SetActive(true);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            foreach (GameObject cell in linkedCells)
            {
                cell.transform.GetChild(1).gameObject.SetActive(true);
            }
            indicator.SetActive(false);
        }
    }
}
