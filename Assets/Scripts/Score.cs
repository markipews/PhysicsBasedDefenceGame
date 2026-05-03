using UnityEngine;

public class Score : MonoBehaviour
{
    public float score;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Add(int amount)
    {
        score += amount;
    }

    public void Subtract(int amount)
    {
        score -= amount;
    }
}
