using UnityEngine;

public class BossTelegrapher : MonoBehaviour
{
    [SerializeField] private float telegraphDistance;
    [SerializeField] private LineRenderer line;


    public Vector2 Direction { get; set; }
    public Transform TrackingTarget { get; set; }

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    public void ToggleLine(bool value)
    {
        line.enabled = value;
    }

    private void Update()
    {
        line.SetPosition(0, transform.position);
        if (TrackingTarget != null )
        {
            Direction = TrackingTarget.transform.position - transform.position;
        }
        line.SetPosition(1, (Vector2)transform.position + Direction.normalized * telegraphDistance);
    }
}
