# Taxi Dispatch System

ASP.NET Core WebAPI と Console アプリを用いた、タクシー配車システムの学習プロジェクトです。

## 目的

以下の技術の習得を目的とします。

- ASP.NET Core WebAPI
- RESTful API設計
- SQL Server
- Dapper
- NLog
- async / await
- HttpClient

---

# システム構成

## 配車サーバー

ASP.NET Core WebAPI

### 役割

- 配車依頼（JOB）の管理
- タクシー情報の管理
- 配車処理
- 状態管理

### 使用技術

- ASP.NET Core WebAPI
- Dapper
- Microsoft.Data.SqlClient
- NLog
- Scalar

## 指令室UI

Console Application

### 役割

- JOB一覧監視
- タクシー一覧監視
- 配車指示

### 使用技術

- HttpClient
- async / await

---

# 操作範囲

本課題では利用者アプリおよびドライバーアプリは作成しません。

利用者およびドライバーの操作は Scalar から API を直接実行して模擬します。

## 利用者操作（Scalar）

### 配車依頼登録

`POST /api/jobs`

```json
{
  "pickupLocation": "松山駅",
  "destination": "松山空港"
}
```

### 行き先変更

`PATCH /api/jobs/{jobId}/destination`

```json
{
  "destination": "松山市駅"
}
```

## ドライバー操作（Scalar）

### JOB状態更新

`PATCH /api/jobs/{jobId}/status`

```json
{
  "status": "PickingUp"
}
```

```json
{
  "status": "OnBoard"
}
```

```json
{
  "status": "Completed"
}
```

### タクシー位置更新

`PATCH /api/taxis/{taxiId}/location`

```json
{
  "location": "松山駅"
}
```

---

# 機能要件

## JOB管理

### JOB登録

利用者からの配車依頼を登録する。

入力項目

- 呼び出し位置
- 行き先

初期状態

- Unassigned

### JOB一覧取得

受信日時順で一覧表示する。

表示項目

- JOBID
- JOBステータス
- 呼び出し位置
- 行き先
- 割り当て済みタクシー
- 受信日時
- 手配日時

### JOB状態

- Unassigned
- Assigned
- PickingUp
- OnBoard
- Completed
- Canceled

## Taxi管理

### Taxi一覧取得

表示項目

- TaxiID
- Taxiステータス
- 担当者
- 進行中JOB
- 現在地

### Taxi状態

- Available
- Assigned
- PickingUp
- OnBoard
- Break
- OffDuty

### Taxi位置更新

タクシーの現在地を更新する。

---

# 配車処理

未割当JOBに対してタクシーを割り当てる。

## JOB更新

- Status = Assigned
- AssignedTaxiId 更新
- AssignedAt 更新

## Taxi更新

- Status = Assigned
- CurrentJobId 更新

## トランザクション

配車処理は必ずトランザクションで実行する。

以下の更新は同時に成功または失敗しなければならない。

- JOB更新
- Taxi更新

---

# API仕様

## JOB

| Method | URL |
|----------|----------|
| GET | /api/jobs |
| GET | /api/jobs/{jobId} |
| POST | /api/jobs |
| POST | /api/jobs/{jobId}/assign/{taxiId} |
| PATCH | /api/jobs/{jobId}/status |
| PATCH | /api/jobs/{jobId}/destination |

## Taxi

| Method | URL |
|----------|----------|
| GET | /api/taxis |
| GET | /api/taxis/available |
| PATCH | /api/taxis/{taxiId}/status |
| PATCH | /api/taxis/{taxiId}/location |

---

# 指令室UI

指令室UIは監視および配車指示のみを担当する。

利用者情報の入力やドライバー操作は行わない。

最新状態は、ポーリングでコンソールが取得すること。

## JOB一覧

受信日時順で表示する。

未割当JOBは赤色表示する。

表示項目

- JOBID
- JOBステータス
- 呼び出し位置
- 行き先
- 割り当て済みタクシー
- 受信日時
- 手配日時

## Taxi一覧

表示項目

- TaxiID
- Taxiステータス
- 担当者
- 進行中JOB
- 現在地

## コマンド

### 一覧更新

```text
refresh
```

### 配車

```text
assign 1001 12
```

意味

```text
JobId=1001 を TaxiId=12 に割り当てる
```

内部的には以下の API を呼び出す。

```http
POST /api/jobs/1001/assign/12
```

### 終了

```text
exit
```

---

# データベース設計

## DispatchJobs

| Column | Type |
|----------|----------|
| JobId | bigint |
| Status | int |
| PickupLocation | nvarchar(100) |
| Destination | nvarchar(100) |
| AssignedTaxiId | bigint NULL |
| ReceivedAt | datetime2 |
| AssignedAt | datetime2 NULL |

## Taxis

| Column | Type |
|----------|----------|
| TaxiId | bigint |
| Status | int |
| DriverName | nvarchar(50) |
| CurrentJobId | bigint NULL |
| CurrentLocation | nvarchar(100) |

---

# 列挙型

## JobStatus

```csharp
public enum JobStatus
{
    Unassigned = 0,
    Assigned = 1,
    PickingUp = 2,
    OnBoard = 3,
    Completed = 4,
    Canceled = 5
}
```

## TaxiStatus

```csharp
public enum TaxiStatus
{
    Available = 0,
    Assigned = 1,
    PickingUp = 2,
    OnBoard = 3,
    Break = 4,
    OffDuty = 5
}
```

---

# ログ要件

NLogを使用して以下を記録する。

- JOB受付
- 配車実行
- JOB状態変更
- Taxi状態変更
- Taxi位置更新
- システムエラー

---

# 非同期処理

以下は async / await を使用する。

- Controller
- Service
- Repository
- Dapper
- HttpClient
- Console更新処理

---

# 学習ポイント

- RESTful API設計
- CRUD操作
- DapperによるSQL実行
- SQL Serverとの接続
- トランザクション管理
- 状態遷移管理
- HttpClientによるAPI呼び出し
- 非同期処理
- ログ出力
- Console UI構築

- インターロック
  - Taxi が Available であること
  - Job が Unassigned であること
  - 両方を満たした場合のみ配車可能
- トランザクション
  - Job を Assigned に更新
  - Taxi を Assigned に更新
  - 両方成功なら COMMIT
  - どちらか失敗なら ROLLBACK