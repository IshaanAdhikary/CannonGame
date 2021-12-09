using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainBuilderScript : MonoBehaviour
{
    public GameObject chainHub;
    public GameObject chainType1;
    public GameObject chainType2;
    public int chainLength;

    public void BuildChain()
    {
        GameObject cap = Instantiate(chainHub, transform.position, Quaternion.identity);
        cap.transform.parent = gameObject.transform;

        for (int i = 0; i < chainLength; i++)
        {
            float chain1Pos = transform.position.y - (0.25f + (0.4f * i-1));

            Instantiate(chainType1, transform.position, Quaternion.identity);
            Instantiate(chainType2, transform.position, Quaternion.identity);
        }
    }
}
