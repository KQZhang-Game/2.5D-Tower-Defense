using UnityEngine;

public class LineWriter : Singleton<LineWriter>
{
    protected override bool IsPersistent => false;
    private LineRenderer line;
    private const int CIRCLE_VERTEX_COUNT = 32;

    protected override void Awake()
    {
        base.Awake();
        line = GetComponent<LineRenderer>();
        if (line == null)
        {
            line = gameObject.AddComponent<LineRenderer>();
        }
        line.loop = true;
        line.startWidth = 0.2f;
        line.endWidth = 0.2f;
        line.useWorldSpace = true;
        line.enabled = false;
    }

    /// <summary>
    /// 绘制攻击范围
    /// </summary>
    public void DrawLineInXZ(AttackRangeShape type, Vector3 centerPoint, float radius, int length = 0, int width = 0)
    {
        line.enabled = true;
        switch (type)
        {
            case AttackRangeShape.Rectangle:
                DrawRectangle(centerPoint, length, width);
                break;

            case AttackRangeShape.Sphere:
                DrawCircle(centerPoint, radius);
                break;
        }
    }
    public void HideLine()
    {
        line.enabled = false;
    }
    private void DrawRectangle(Vector3 center, int length, int width)
    {
        line.positionCount = 4;
        float halfW = width * 0.5f;
        float halfL = length * 0.5f;

        Vector3[] pos = new Vector3[4];
        pos[0] = new Vector3(center.x - halfW, center.y, center.z - halfL);
        pos[1] = new Vector3(center.x + halfW, center.y, center.z - halfL);
        pos[2] = new Vector3(center.x + halfW, center.y, center.z + halfL);
        pos[3] = new Vector3(center.x - halfW, center.y, center.z + halfL);

        line.SetPositions(pos);
    }
    private void DrawCircle(Vector3 center, float radius)
    {
        line.positionCount = CIRCLE_VERTEX_COUNT;
        Vector3[] pos = new Vector3[CIRCLE_VERTEX_COUNT];

        for (int i = 0; i < CIRCLE_VERTEX_COUNT; i++)
        {
            float angle = 360f * i / CIRCLE_VERTEX_COUNT;
            float x = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            float z = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            pos[i] = new Vector3(center.x + x, center.y, center.z + z);
        }

        line.SetPositions(pos);
    }
}