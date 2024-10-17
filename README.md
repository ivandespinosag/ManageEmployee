DATABASE

USE [ManageEmployees_DB]
GO

/****** Object:  Table [dbo].[Employees]    Script Date: 17/10/2024 15:51:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Employees](
	[EmployeeId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[Position] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[HireDate] [date] NOT NULL,
	[PhotoUrl] [nvarchar](255) NULL,
	[ContractEndDate] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--########################## STORE PROCEDURES
--===========================================

USE [ManageEmployees_DB]
GO
/****** Object:  StoredProcedure [dbo].[p_Add_Employee]    Script Date: 17/10/2024 15:53:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[p_Add_Employee]
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @Position NVARCHAR(50),
    @HireDate DATE,
    @ContractEndDate DATE = NULL,
    @PhotoUrl NVARCHAR(255),
    @EmployeeId INT OUTPUT  -- Parámetro de salida para devolver el EmployeeId
AS
BEGIN
    SET NOCOUNT ON;

	IF @ContractEndDate IS NULL
    BEGIN
        SET @ContractEndDate = DATEADD(YEAR, 1, @HireDate);  -- Sumar un año a @HireDate
    END

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Insertar el nuevo registro del empleado
        INSERT INTO Employees (FirstName, LastName, Email, Position, HireDate, ContractEndDate, PhotoUrl)
        VALUES (@FirstName, @LastName, @Email, @Position, @HireDate, @ContractEndDate, @PhotoUrl);

        -- Obtener el EmployeeId generado
        SET @EmployeeId = SCOPE_IDENTITY();  -- Obtener el último EmployeeId insertado

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT @ErrorMessage = ERROR_MESSAGE(),
               @ErrorSeverity = ERROR_SEVERITY(),
               @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH;
END;

--===========================================

USE [ManageEmployees_DB]
GO
/****** Object:  StoredProcedure [dbo].[p_Delete_Employee]    Script Date: 17/10/2024 15:54:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[p_Delete_Employee]
    @EmployeeId INT
AS
BEGIN

    DELETE FROM Employees
    WHERE EmployeeId = @EmployeeId;

    -- Retornar el número de filas afectadas
    RETURN @@ROWCOUNT;

END

--===========================================

USE [ManageEmployees_DB]
GO
/****** Object:  StoredProcedure [dbo].[p_Get_Employees]    Script Date: 17/10/2024 15:54:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[p_Get_Employees] 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT [EmployeeId]
		, [FirstName]
		, [LastName]
        , [Position]
        , [Email]
        , [HireDate]
        , [PhotoUrl]
        , [ContractEndDate]
	FROM Employees


END

--===========================================

USE [ManageEmployees_DB]
GO
/****** Object:  StoredProcedure [dbo].[p_Update_Employee]    Script Date: 17/10/2024 15:55:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[p_Update_Employee]
    @EmployeeId INT,
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @Email NVARCHAR(255),
    @Position NVARCHAR(100),
    @HireDate DATETIME,
    @ContractEndDate DATETIME,
    @PhotoUrl NVARCHAR(255)
AS
BEGIN
    
    -- Verificar si el empleado existe
    IF NOT EXISTS (SELECT 1 FROM Employees WHERE EmployeeId = @EmployeeId)
    BEGIN
        RAISERROR('El empleado con ID %d no existe.', 16, 1, @EmployeeId);
        RETURN;
    END

    -- Actualizar los datos del empleado
    UPDATE Employees
    SET 
        FirstName = @FirstName,
        LastName = @LastName,
        Email = @Email,
        Position = @Position,
        HireDate = @HireDate,
        ContractEndDate = @ContractEndDate,
        PhotoUrl = @PhotoUrl
    WHERE EmployeeId = @EmployeeId;

    -- Opcional: devolver el número de filas afectadas
    RETURN @@ROWCOUNT;
END
