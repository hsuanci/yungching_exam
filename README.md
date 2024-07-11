# 專案描述
> Yung Ching Exam 

# 框架技術
- .NET 8

# 建置
- dotnet restore
- dotnet run --project YungChingExam

# 開發 Style
> 此專案包含 1 個 API 專案和 3 個類別庫專案：

- YungChingExam (API 專案): 提供 API 端點，負責處理 HTTP 請求並調用服務層進行業務邏輯處理。
- YungChingExam.Data (DTOs & Model, Context): 包含資料傳輸對象 (DTOs)、資料模型和 Entity Framework 的 DbContext。
- YungChingExam.Repository (資料庫 Table 基礎設施): 提供與資料庫交互的基礎設施，包括 CRUD 操作。
- YungChingExam.Service (商業邏輯): 包含業務邏輯，負責處理來自 Repository 層的資料並執行相應的業務操作。


# 資料庫建置
- 請使用 ```instnwnd.sql``` 來建置範例資料庫。你可以使用以下方法來執行 SQL 檔案：

## 使用 SQL Server Management Studio (SSMS)
- 打開 SSMS。
- 連接到你的 SQL Server 執行個體。
- 打開 instnwnd.sql 檔案。
- 執行 SQL 腳本以建置範例資料庫。