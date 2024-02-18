using UnityEngine;

public class MoveToClick : MonoBehaviour {

	public bool rightClick = false;
    public ParticleSystem moveParticles;
    private int buttonNumber;
    private SpriteRenderer sp;
    private SpriteRenderer sp2;
    public Rect allowedClickArea = new Rect(100, 100, 200, 200);

    void Start ()
    {
        sp = GetComponent<SpriteRenderer>();
        sp2 = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        if (rightClick) buttonNumber = 1;
        else buttonNumber = 0;
    }


	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(buttonNumber))
        {
            Vector3 click = Input.mousePosition;

            if (IsClickAllowed(click))
            {
                Vector3 wantedPosition = Camera.main.ScreenToWorldPoint(new Vector3(click.x, click.y, 1f));
                wantedPosition.z = transform.position.z;
                transform.position = wantedPosition;
                moveParticles.Play();
                PinIsReached(true);
            }
        }
    }

    public void PinIsReached(bool on)
    {
        string tag;
        if (on) tag = "PIN";
        else tag = "Untagged";

        this.gameObject.tag = tag;
        sp.enabled = on;
        sp2.enabled = on;
    }

    private bool IsClickAllowed(Vector3 clickPosition)
    {
        return allowedClickArea.Contains(clickPosition);
    }

    void OnDrawGizmos()
    {
        Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(allowedClickArea.x, allowedClickArea.y, 1f));
        Vector3 bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(allowedClickArea.x + allowedClickArea.width, allowedClickArea.y, 1f));
        Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(allowedClickArea.x, allowedClickArea.y + allowedClickArea.height, 1f));
        Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(allowedClickArea.x + allowedClickArea.width, allowedClickArea.y + allowedClickArea.height, 1f));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);
    }
}

