using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkFollowing : MonoBehaviour
{
    [SerializeField] private string link;

    private void Awake()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(URLFollow);
    }

    private void URLFollow()
    {
        Application.OpenURL(link);
    }
    
}
