using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsMoves : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> starsTargetPoints;
    [SerializeField]
    public GameObject targetCube;

    [SerializeField]
    public bool move = true;
    bool revers;
    [SerializeField]
    public bool setPause;

    [SerializeField]
    public static event System.Action<GameObject> OnClickToStar;

    [SerializeField]
    private int randomSpeed = 12;
    
    void Start()
    {
        StartCoroutine(RundomTimeForSpeed());
        StartCoroutine(RundomTimeForDirection());
    }

    private void OnTriggerEnter(Collider other)
    {
        for(int i = 0; i < starsTargetPoints.Count; i++)
        {
            if (other.gameObject.Equals(starsTargetPoints[i]))
            {
                if(i != (starsTargetPoints.Count - 1) && revers == false)
                {
                    targetCube = starsTargetPoints[i + 1];
                }

                if (i == (starsTargetPoints.Count - 1) && revers == false)
                {
                    targetCube = starsTargetPoints[0];
                }

                if (i != 0 && revers)
                {
                    targetCube = starsTargetPoints[i - 1];
                }

                if (i == 0 && revers)
                {
                    targetCube = starsTargetPoints[starsTargetPoints.Count - 1];
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (move)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetCube.transform.position, Time.deltaTime / randomSpeed);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            move = !move;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            revers = !revers;
        }
    }

    private void OnMouseDown()
    {
        if(setPause == false)
        {
            OnClickToStar?.Invoke(gameObject);
        }
    }


    IEnumerator RundomTimeForSpeed()
    {
        yield return new WaitForSeconds(Random.Range(2, 6));
        StartCoroutine(RundomSpeed());
    }

    IEnumerator RundomTimeForDirection()
    {
        yield return new WaitForSeconds(Random.Range(2, 6));
        StartCoroutine(RundomDirection());
    }

    IEnumerator RundomSpeed()
    {
        randomSpeed = Random.Range(12, 25);
        yield return new WaitForSeconds(Random.Range(2, 6));
        StartCoroutine(RundomTimeForSpeed());
    }

    IEnumerator RundomDirection()
    {
        float myFloat = Random.Range(0f, 1f);
        if(myFloat <= 0.5f)
        {
            revers = false;
        }
        if (myFloat > 0.5f)
        {
            revers = true;
        }
        yield return new WaitForSeconds(Random.Range(2, 6));
        StartCoroutine(RundomTimeForDirection());
    }
}
