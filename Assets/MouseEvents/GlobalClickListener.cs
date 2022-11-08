using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapTileClickChecker), typeof(PlayerClickChecker), typeof(EnemyClickChecker))]
public class GlobalClickListener : MonoBehaviour
{
    private IClickChecker mapTileListener;
    private IClickChecker playerListener;
    private IClickChecker enemyListener;
    public IClickChecker MapTileListener { get => mapTileListener; }
    public IClickChecker PlayerListener { get => playerListener; }
    public IClickChecker EnemyListener { get => enemyListener; }

    public static GlobalClickListener Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        mapTileListener = GetComponent<MapTileClickChecker>();
        playerListener = GetComponent<PlayerClickChecker>();
        enemyListener = GetComponent<EnemyClickChecker>();
    }
}