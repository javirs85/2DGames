using Assets.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameController : MonoBehaviour
{
    public GameObject HerizoPrefab;
    private GameObject HerizoGO;
    private herizo Herizo;

    float minx, miny, maxx, maxy;

    // Start is called before the first frame update
    void Start()
    {
        minx = -5;
        maxx = 0;
        miny = -2;
        maxy = -1.5f;

        SpawnHerizo();
    }

    private void Herizo_IDied(object sender, System.EventArgs e)
    {
        SpawnHerizo();
    }

    private void SpawnHerizo()
    {
        var x = UnityEngine.Random.Range(minx, maxx);
        var y = UnityEngine.Random.Range(miny, maxy);
        HerizoGO = Instantiate(HerizoPrefab, new Vector3(x, y), Quaternion.identity);
        Herizo = HerizoGO.GetComponent<herizo>();
        Herizo.IDied += Herizo_IDied;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
