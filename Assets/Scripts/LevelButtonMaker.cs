using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Security.Cryptography;

public class LevelButtonMaker : MonoBehaviour
{
    public GameObject levelButton;
    public GameObject prevButton;
    public GameObject nextButton;

    private int totalLevels;
    private int start_i = 0;
    private int end_i = 0;
    private int page_i = 0;
    private GameObject currentPage;

    private int current_i;

    void OnEnable()
    {
        MakeThePages() ;
    }

    void MakeThePages()
    {
        totalLevels = PlayerPrefs.GetInt("latestLevel");
        //Make the pages
        while (start_i < totalLevels)
        {
            currentPage = new GameObject("Page" + ++page_i);
            currentPage.transform.parent = this.transform;
            if (totalLevels - start_i - 1 > 10)
            {
                end_i = start_i + 10;
            }
            else
            {
                end_i = totalLevels;
            }
            PopulatePage();
            start_i = end_i;
            if (page_i > 1)
            {
                currentPage.SetActive(false);
            }
        }
        prevButton.SetActive(false);
        if (page_i < 2)
        {
            nextButton.SetActive(false);
        }
    }

    void PopulatePage()
    {
        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < 5; i++)
            {
                if (start_i + i + 5 * j >= end_i)
                {
                    return;
                }
                GameObject button = Instantiate(levelButton, currentPage.transform);
                int levelNum = start_i + i + 5 * j + 1;
                button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Level" + levelNum;
                button.transform.position =  new Vector3(600 + i * 200, 600 - j * 150, 0);
            }
        }
    }

    public void NextPage()
    {
        transform.GetChild(current_i++).gameObject.SetActive(false);
        transform.GetChild(current_i).gameObject.SetActive(true);
        if (current_i + 1 >= page_i)
        {
            nextButton.SetActive(false);
        }
        prevButton.SetActive(true);
    }

    public void PrevPage()
    {
        transform.GetChild(current_i--).gameObject.SetActive(false);
        transform.GetChild(current_i).gameObject.SetActive(true);
        if (current_i <= 0)
        {
            prevButton.SetActive(false);
        }
        nextButton.SetActive(true);
    }
}
