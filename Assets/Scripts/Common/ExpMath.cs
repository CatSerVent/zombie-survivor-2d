using UnityEngine;

namespace Game.Common
{
    /// <summary>
    /// ���� �� ����/�뷱�� ���� ���� �Լ� ����.
    /// ��Ʈ���������� "�׽�Ʈ ������ ���� ����" ���÷� ����մϴ�.
    /// </summary>
    public static class ExpMath
    {
        /// <summary>
        /// ����ġ ��� ���
        /// 10,20,30�� ���帶�� ��1.5�� ����
        /// Wave 1~9 => 1.0
        /// Wave 10~19 => 1.5
        /// Wave 20~29 => 2.25
        /// ...
        /// </summary>
        public static float ComputeExpMultiplier(int waveIndex)
        {
            if (waveIndex < 1) return 1f;
            int steps = waveIndex / 10; // 10�� ������� 1 ����
            return Mathf.Pow(1.5f, steps);
        }
    }
}
