using System.Collections.Generic;
using UnityEngine;

namespace SunBase.EraseCircles
{
    /// <summary>
    /// Line Drawer class attached on line renderer 
    /// draws line when mouse/fingure draw
    /// when mouse is up cast ray and check if the circles are present then destroy circles
    /// </summary>
    public class LineDrawer : MonoBehaviour, ILineDrawer
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private float lineWidth = 0.1f;
        [SerializeField] private LayerMask circleLayer; // Set this in the Inspector to the layer containing circles

        private List<Vector3> linePositions = new List<Vector3>();
        private bool isDrawing = false;


        private void Start()
        {
            // Configure Line Renderer settings
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
            lineRenderer.positionCount = 0;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartDrawing();
            }

            if (isDrawing)
            {
                UpdateDrawing();
            }

            if (Input.GetMouseButtonUp(0))
            {
                EndDrawing();
            }
        }

        //Calls when Ends Line Drawing
        public void EndDrawing()
        {
            isDrawing = false;

            // Cast a ray along the line and check for collisions with objects on the specified layer
            Vector2 startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (linePositions.Count >= 2)
            {
                startPoint = linePositions[0];
                endPoint = linePositions[linePositions.Count - 1];
            }

            RaycastHit2D[] hits = Physics2D.LinecastAll(startPoint, endPoint, circleLayer);

            foreach (RaycastHit2D hit in hits)
            {
                Circle circle = hit.collider.GetComponent<Circle>();
                if (circle != null)
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }

        //Calls when Start to draw line

        public void StartDrawing()
        {
            isDrawing = true;
            lineRenderer.positionCount = 0;
            linePositions.Clear();
        }

        //Update line position when dragging line
        public void UpdateDrawing()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            linePositions.Add(mousePosition);

            lineRenderer.positionCount = linePositions.Count;
            lineRenderer.SetPositions(linePositions.ToArray());
        }
    }
}