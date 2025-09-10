# 🧟 Zombie Survivor 2D (Unity Portfolio)

> **Unity 2022.3 LTS / URP(2D Renderer)**  
> 싱글플레이 2D 탑다운 **좀비 서바이벌 웨이브 게임**  
> 개인 포트폴리오 프로젝트 (클라이언트 중심 개발)

---

## 🎮 게임 개요
- **장르**: 2D Top-Down Wave Survival
- **플랫폼**: Android, WebGL
- **플레이 방식**: 
  - 조이스틱 이동 + 자동 공격
  - 웨이브마다 적 수 증가
  - 10웨이브마다 경험치 획득량 1.5배 상승
- **목표**: 가능한 오래 살아남고 성장하기

---

## ✨ 핵심 기능

### ⚔️ 전투 시스템
- **무기 (WeaponController + WeaponData)**
  - `SingleShot`: 고데미지 단일 발사, 후반에 관통/폭발 추가
  - `Shotgun`: 다발 산탄, 레벨업마다 발 수 증가 + 폭발 보조
  - `Orbit`: 플레이어 궤도 회전, 탄 수와 반경 확장
- **총알 (BulletHitPierce)**
  - 데미지 / 관통 / 범위 피해 지원
  - `BulletLifetime`, `BulletOutOfView` → 풀링 반환 관리
- **자동 공격**
  - 버튼 입력 불필요, 무기 쿨다운 기반 자동 사격

### 🧩 패시브 시스템 (PassiveInventory + PassiveData)
- `MoveSpeed`: 이동속도 증가 (최대 5레벨)  
- `Damage`: 무기 데미지 배수 증가 (최대 5레벨)  
- `ExpGain`: 경험치 획득 배수 증가 (최대 5레벨)  
- `Magnet`: 경험치 흡수 반경 확장 (최대 10레벨)  

### 🌊 웨이브 시스템 (WaveManager + SpawnTable)
- 웨이브마다 스폰 수 증가
- 러너/탱커 단계적 등장
- 10웨이브마다 **Exp Multiplier ×1.5**
- 경험치 드랍 오브젝트(`ExpOrb`)는 `PlayerExpCollector`에 의해 자석처럼 흡수됨

### 🎛️ 풀링 시스템 (ObjectPool / PooledObject / PoolUtil)
- Instantiate/Destroy 금지, 모든 오브젝트 풀링 기반 관리
- 관리 대상:
  - **총알**
  - **이펙트 (히트, 텍스트)**
  - **경험치 오브젝트**
- `SafeRelease()` 유틸로 안정적인 반환

### 📊 HUD (PerfHUD)
- FPS / Memory / Wave / Alive / Exp / Exp Multiplier 표시
- Exp Multiplier는 UI에서 실시간 반영 (`x1.0`, `x1.5`, `x2.25`…)

---

## ⚙️ 아키텍처

### 🔹 프로젝트 구조
```
Assets/
 ├─ Scripts/
 │   ├─ Combat/ (무기, 총알, 히트 이펙트)
 │   ├─ Enemies/ (Enemy, EnemyAI, WaveManager)
 │   ├─ Systems/ (Pooling, HUD, Level, Settings)
 │   └─ UI/ (DamageText, WorldCanvasManager)
 ├─ ScriptableObjects/
 │   ├─ Weapons (SingleShot, Shotgun, Orbit)
 │   ├─ Passives (MoveSpeed, Damage, ExpGain, Magnet)
 │   └─ WaveConfig / SpawnTable
 ├─ Prefabs/
 │   ├─ Player, Enemy, ExpOrb
 │   ├─ Bullet, HitEffect, DamageText
 │   └─ HUD
 └─ Scenes/
     ├─ MainScene
     └─ TestScene
```

### 🔹 흐름도
- **WaveManager** → Enemy 스폰  
- **Enemy.Die()** → ExpOrb 풀에서 드랍  
- **PlayerExpCollector** → ExpOrb 흡수 → LevelSystem 경험치 추가  
- **LevelSystem** → 레벨업 시 무기/패시브 선택 UI 표시  
- **WeaponController** → 선택된 무기 인스턴스 자동 공격  
- **HUD (PerfHUD)** → 실시간 게임 정보 표시  

---

## 🖼️ 스크린샷
| HUD & 전투 | 웨이브 진행 | 레벨업 |
|------------|------------|--------|
| ![hud](./docs/screenshot_hud.png) | ![wave](./docs/screenshot_wave.png) | ![levelup](./docs/screenshot_levelup.png) |

---

## 📂 데이터 예시 (WeaponData)

### SingleShot
```yaml
Max Level: 5
Base Damage: 10
Base Speed: 12
Range: 10
Base Fire Rate: 2
Kind: SingleShot
Pierce Per Level: [0,0,1,1,2]
Splash Radius Per Level: [0,0,0,0,1.5]
Splash Ratio Per Level: [0,0,0,0,0.5]
```

### Shotgun
```yaml
Max Level: 5
Base Damage: 5
Base Speed: 10
Range: 8
Base Fire Rate: 1.2
Kind: Shotgun
Pierce Per Level: [0,0,1,1,1]
Splash Radius Per Level: [0,0,0,0.5,1.0]
Splash Ratio Per Level: [0,0,0,0.3,0.5]
```

### Orbit
```yaml
Max Level: 5
Base Damage: 6
Base Speed: 8
Range: 4
Base Fire Rate: 0.5
Kind: Orbit
Pierce Per Level: [999,999,999,999,999]
Splash Radius Per Level: [1.0,1.2,1.5,1.8,2.0]
Splash Ratio Per Level: [0,0,0,0,0]
```

---

## 📈 최적화
- `Application.targetFrameRate = 60`, `vSync = 0`
- Incremental GC ON
- URP 2D Renderer + 최소 옵션(MSAA/HDR/PP)
- 모든 오브젝트 풀링(ObjectPool) 기반 관리

---

## 🚀 개선 여지
- 아트/사운드 퀄리티 보강
- 추가 무기/적 패턴
- UI 개선 (메인 메뉴, 게임오버)
- 온라인 빌드 (itch.io / GitHub Pages WebGL)

---

## 👤 개발자
- **개인 프로젝트 (100%)**
- 게임 기획 / 코드 / 시스템 / 최적화 직접 구현
- 아트/사운드: 무료 에셋 사용
- 📧 Contact: [Your Email]  
- 🔗 GitHub: [Your GitHub Profile]
