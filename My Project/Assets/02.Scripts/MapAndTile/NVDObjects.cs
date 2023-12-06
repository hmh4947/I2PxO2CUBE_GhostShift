using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NVDObjects : MonoBehaviour
{
    private GameObject player;
    private PlayerGogglesController playerGogglesControllerScr;
    private TilemapRenderer tilemapRenderer;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerGogglesControllerScr = player.GetComponent<PlayerGogglesController>();
        tilemapRenderer = GetComponent<TilemapRenderer>();
        this.gameObject.GetComponent<Tilemap>().color = Color.green;
        tilemapRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerGogglesControllerScr.IsInNVDModes)
        {
            tilemapRenderer.enabled = true;
        }
        else
        {
            tilemapRenderer.enabled = false;
        }
    }
}
