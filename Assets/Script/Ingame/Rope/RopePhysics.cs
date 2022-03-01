using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using camera;
using Manager;
namespace Pinata {
    public class RopePhysics : MonoBehaviour
    {
        public LineRenderer lineRenderer;
        public int segmentCount;
        public int constraintLoop;
        public float segmentLength;
        public float ropeWidth;
        public Vector2 gravity;
        [Space(10f)]
        public Transform startTransform;
        public Transform EndTransform;
        public float pivot;

        private List<Segment> segments = new List<Segment>();

        [SerializeField] private pinata_down pd;
        private void Reset()
        {
            TryGetComponent(out lineRenderer);
        }

        public void Add_Component()
        {
            segmentCount += 10;
            Vector2 segmentPos = segments[segments.Count - 1].position;
            for (int i = 0; i < 10; i++)
            {
                segments.Add(new Segment(segmentPos));
                segmentPos.y -= segmentLength;
            }
        }

        private void Awake()
        {
            Vector2 segmentPos = startTransform.position;
            for (int i = 0; i < segmentCount; i++)
            {
                segments.Add(new Segment(segmentPos));
                segmentPos.y -= segmentLength;
            }
        }

        private void UpdateSegments()
        {
            for (int i = 1; i < segments.Count - 1; i++)
            {
                segments[i].velocity = segments[i].position - segments[i].previousPos;
                segments[i].previousPos = segments[i].position;
                segments[i].position += gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;
                segments[i].position += segments[i].velocity;
            }
        }

        private void ApplyConstraint()
        {
            segments[0].position = startTransform.position;
            segments[segments.Count - 1].position = new Vector2(EndTransform.position.x , EndTransform.position.y+ pivot);
            for (int i = 0; i < segments.Count - 1; i++)
            {
                float distance = (segments[i].position - segments[i + 1].position).magnitude;
                float difference = segmentLength - distance;
                Vector2 Dir = (segments[i + 1].position - segments[i].position).normalized;
                Vector2 movement = Dir * difference;

                if (i == 0)
                    segments[i + 1].position += movement;

                else if (i == segments.Count - 2)
                    segments[i].position -= movement;

                else
                {
                    segments[i].position -= movement * 0.5f;
                    segments[i + 1].position += movement * 0.5f;

                }

            }


        }

        private void Update()
        {
            UpdateSegments();
            for (int i = 0; i < constraintLoop; i++)
            {
                ApplyConstraint();
            }


            DrawRope();
        }

        private void DrawRope()
        {

            lineRenderer.startWidth = ropeWidth;
            lineRenderer.endWidth = ropeWidth;
            Vector3[] segmentPosition = new Vector3[segments.Count];

            for (int i = 0; i < segments.Count; i++)
            {
                segmentPosition[i] = segments[i].position;
            }
            lineRenderer.positionCount = segmentPosition.Length;
            lineRenderer.SetPositions(segmentPosition);

        }
        

        public class Segment
        {
            public Vector2 previousPos;
            public Vector2 position;
            public Vector2 velocity;

            public Segment(Vector2 _position)
            {
                previousPos = _position;
                position = _position;
                velocity = Vector2.zero;

            }

        }
    }
}
