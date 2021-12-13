using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainBuilderScript : MonoBehaviour
{
    public enum ChainEnd { Nothing, Mace }
    public GameObject chainHub;
    public GameObject chainType1;
    public GameObject chainType2;
    public int chainLength;

    private ChainEnd ending;
    private Rigidbody2D prevRB;

    public void BuildChain()
    {
        ending = ChainEnd.Nothing;
        GameObject cap = Instantiate(chainHub, transform.position, Quaternion.identity);
        cap.transform.parent = gameObject.transform;
        prevRB = cap.GetComponent<Rigidbody2D>();

        for (int i = 0; i < chainLength; i++)
        {
            Vector3 chain1Pos = transform.position + new Vector3(0, -0.8f * i - 0.25f, 0);
            Vector3 chain2Pos = transform.position + new Vector3(0, -0.8f * i - 0.65f, 0);

            GameObject chain1 = Instantiate(chainType1, chain1Pos, Quaternion.identity);
            GameObject chain2 = Instantiate(chainType2, chain2Pos, Quaternion.identity);
            chain1.transform.parent = gameObject.transform;
            chain2.transform.parent = gameObject.transform;

            HingeJoint2D hinge1 = chain1.GetComponent<HingeJoint2D>();
            HingeJoint2D hinge2 = chain2.GetComponent<HingeJoint2D>();
            hinge1.connectedBody = prevRB;
            hinge2.connectedBody = hinge1.attachedRigidbody;
            prevRB = hinge2.attachedRigidbody;
        }

        switch (ending)
        {
            case ChainEnd.Mace:
                Debug.Log("Mace");
                break;
        }
    }
}
