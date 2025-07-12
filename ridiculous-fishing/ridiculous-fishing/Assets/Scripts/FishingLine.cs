using UnityEngine;

[ExecuteInEditMode]
public class FishingLine : MonoBehaviour
{
    public Transform pivot1;
    public Transform pivot2;

    private LineRenderer lr;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        lr.SetPositions(new Vector3[]{
            pivot1.position, pivot2.position
        });
    }
}
