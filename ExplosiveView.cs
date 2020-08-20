using System.Collections.Generic;
using UnityEngine;

public class ExplosiveViewPart
{
    private Vector3 m_meshCenter;
    private float m_moveDistance;

    public Vector3 StartPosition { get; }

    public Vector3 Destination { get; }

    public ExplosiveViewPart(Vector3 startPosition, Vector3 meshCenter, float moveDistance)
    {
        StartPosition = startPosition;
        m_meshCenter = meshCenter;
        m_moveDistance = moveDistance;
        Destination = meshCenter * moveDistance;
    }
}

public class ExplosiveView : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    [SerializeField] private float m_percentage;

    [SerializeField] private float m_moveDistance = 3f;

    private readonly List<ExplosiveViewPart> m_parts = new List<ExplosiveViewPart>();
    private readonly List<Transform> m_partTransforms = new List<Transform>();

    private void Awake()
    {
        foreach (var component in GetComponentsInChildren<MeshRenderer>())
        {
            Transform partTransform = component.transform;

            m_partTransforms.Add(partTransform);
            m_parts.Add(new ExplosiveViewPart(partTransform.position, component.bounds.center, m_moveDistance));
        }
    }

    private void OnValidate()
    {
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
