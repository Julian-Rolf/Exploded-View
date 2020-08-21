using System;
using System.Collections.Generic;
using UnityEngine;

namespace Interactions
{
    public class ExplodedViewPart
    {
        public Vector3 StartPosition { get; }

        public Vector3 Destination;

        public float MoveDistance
        {
            set => Destination = m_meshCenter * value;
        }

        private readonly Vector3 m_meshCenter;

        public ExplodedViewPart(Vector3 startPosition, Vector3 meshCenter, float moveDistance)
        {
            m_meshCenter = meshCenter;

            StartPosition = startPosition;
            Destination = meshCenter * moveDistance;
        }
    }

    public class ExplodedView : MonoBehaviour
    {
        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_percentage;

        [SerializeField] private float m_moveDistance = 3f;

        private float m_oldMoveDistance = 3f;

        private readonly List<ExplodedViewPart> m_parts = new List<ExplodedViewPart>();
        private readonly List<Transform> m_partTransforms = new List<Transform>();

        private void Awake()
        {
            foreach (var component in GetComponentsInChildren<MeshRenderer>())
            {
                Transform partTransform = component.transform;

                m_partTransforms.Add(partTransform);
                m_parts.Add(new ExplodedViewPart(partTransform.position, component.bounds.center, m_moveDistance));
            }
        }

        private void ChangeMaxDistance(float value)
        {
            for (int i = 0; i < m_partTransforms.Count; i++)
            {
                m_parts[i].MoveDistance = value;
            }
        }

        private void OnValidate()
        {
            if (Math.Abs(m_oldMoveDistance - m_moveDistance) > float.Epsilon)
            {
                ChangeMaxDistance(m_moveDistance);

                m_oldMoveDistance = m_moveDistance;
            }

            Explode(m_percentage);
        }

        public void Explode(float percentage)
        {
            for (int i = 0; i < m_partTransforms.Count; i++)
            {
                m_partTransforms[i].position = (1 - percentage) * m_parts[i].StartPosition +
                                               percentage * m_parts[i].Destination;
            }
        }
    }
}