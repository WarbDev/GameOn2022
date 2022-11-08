using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalClickListener : MonoBehaviour
{
    [SerializeField] private ClickListener mapTileListener;
    [SerializeField] private ClickListener playerListener;
    [SerializeField] private ClickListener enemyListener;
    public ClickListener MapTileListener { get => mapTileListener; }
    public ClickListener PlayerListener { get => playerListener; }
    public ClickListener EnemyListener { get => enemyListener; }

    public static GlobalClickListener Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }
}