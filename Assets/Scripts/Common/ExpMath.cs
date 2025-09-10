using UnityEngine;

namespace Game.Common
{
    /// <summary>
    /// 게임 내 수학/밸런스 관련 헬퍼 함수 모음.
    /// 포트폴리오에서 "테스트 가능한 순수 로직" 예시로 사용합니다.
    /// </summary>
    public static class ExpMath
    {
        /// <summary>
        /// 경험치 배수 계산
        /// 10,20,30… 라운드마다 ×1.5씩 누적
        /// Wave 1~9 => 1.0
        /// Wave 10~19 => 1.5
        /// Wave 20~29 => 2.25
        /// ...
        /// </summary>
        public static float ComputeExpMultiplier(int waveIndex)
        {
            if (waveIndex < 1) return 1f;
            int steps = waveIndex / 10; // 10의 배수마다 1 증가
            return Mathf.Pow(1.5f, steps);
        }
    }
}
