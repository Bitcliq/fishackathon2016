
--/****** Object:  Table [dbo].[User_Type]    Script Date: 30-01-2015 17:32:09 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--CREATE TABLE [dbo].[User_Type](
--	[UserID] [int] NOT NULL,
--	[TypeID] [int] NOT NULL,
-- CONSTRAINT [PK_User_Type] PRIMARY KEY CLUSTERED 
--(
--	[UserID] ASC,
--	[TypeID] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
--) ON [PRIMARY]

--GO
--ALTER TABLE [dbo].[User_Type]  WITH CHECK ADD  CONSTRAINT [FK_User_Type_Type] FOREIGN KEY([TypeID])
--REFERENCES [dbo].[Type] ([ID])
--GO
--ALTER TABLE [dbo].[User_Type] CHECK CONSTRAINT [FK_User_Type_Type]
--GO
--ALTER TABLE [dbo].[User_Type]  WITH CHECK ADD  CONSTRAINT [FK_User_Type_User] FOREIGN KEY([UserID])
--REFERENCES [dbo].[User] ([ID])
--GO
--ALTER TABLE [dbo].[User_Type] CHECK CONSTRAINT [FK_User_Type_User]
--GO





--IF NOT EXISTS(SELECT * FROM sys.columns WHERE [object_id]=object_id('User') AND [name]='ISAdmin')
--BEGIN
	
--	ALTER TABLE [User] ADD ISAdmin BIT DEFAULT(0)

--END
--GO



--IF NOT EXISTS(SELECT * FROM sys.columns WHERE [object_id]=object_id('Issue') AND [name]='AssignedTo')
--BEGIN
	
--	ALTER TABLE [Issue] ADD AssignedTo INT

--END
--GO


--ALTER TABLE [dbo].[Issue]  WITH CHECK ADD  CONSTRAINT [FK_Issue_User] FOREIGN KEY([AssignedTo])
--REFERENCES [dbo].[User] ([ID])
--GO
--ALTER TABLE [dbo].[Issue] CHECK CONSTRAINT [FK_Issue_User]
--GO



--UPDATE [User] SET ISAdmin = 0

---- Monica Bitcliq AS USER
--UPDATE [User] SET ISAdmin = 1
--WHERE [Email] = 'monica@bitcliq.com'



--IF NOT EXISTS(SELECT * FROM sys.columns WHERE [object_id]=object_id('IssueStates') AND [name]='ReporterIDClosed')
--BEGIN
	
--	ALTER TABLE [IssueStates] ADD ReporterIDClosed INT

--END
--GO


--ALTER TABLE [dbo].[IssueStates]  WITH CHECK ADD  CONSTRAINT [FK_IssueStates_Reporter] FOREIGN KEY([ReporterIDClosed])
--REFERENCES [dbo].[Reporter] ([ID])
--GO
--ALTER TABLE [dbo].[IssueStates] CHECK CONSTRAINT [FK_IssueStates_Reporter]
--GO


IF NOT EXISTS(SELECT * FROM sys.columns WHERE [object_id]=object_id('Issue') AND [name]='Device')
BEGIN
	
	ALTER TABLE [Issue] ADD Device VARCHAR(255)

END
GO

--IF NOT EXISTS(SELECT * FROM sys.columns WHERE [object_id]=object_id('Issue') AND [name]='CameraType')
--BEGIN
	
--	ALTER TABLE [Issue] ADD CameraType INT

--END
--GO



IF NOT EXISTS(SELECT * FROM sys.columns WHERE [object_id]=object_id('User') AND [name]='ReceiveNotifications')
BEGIN
	
	ALTER TABLE [User] ADD ReceiveNotifications BIT DEFAULT(1)

END
GO

UPDATE [User] SET ReceiveNotifications = 1
GO



IF NOT EXISTS(SELECT * FROM sys.columns WHERE [object_id]=object_id('Type') AND [name]='ParentID')
BEGIN
	
	ALTER TABLE [Type] ADD ParentID INT 

END
GO


ALTER TABLE [dbo].[Type]  WITH CHECK ADD  CONSTRAINT [Type_ParentID_FK] FOREIGN KEY([ParentID])
REFERENCES [dbo].[Type] ([ID])
GO

ALTER TABLE [dbo].[Type] CHECK CONSTRAINT [Type_ParentID_FK]
GO

/****** Object:  StoredProcedure [dbo].[Type_ListToDataSet]    Script Date: 30-01-2015 12:26:17 ******/
DROP PROCEDURE [dbo].[Type_ListToDataSetWithParent]
GO

/****** Object:  StoredProcedure [dbo].[Type_ListToDataSet]    Script Date: 30-01-2015 12:26:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Type_ListToDataSetWithParent]
	@UserID AS INT,
	@p_Si INT, 
	@p_iPage INT, 
	@p_SortColumnName VARCHAR(255), 
	@p_Filters VARCHAR(255)
AS
BEGIN
	
	DECLARE @ProfileID AS INT
	DECLARE @AccountID AS INT
	DECLARE @SQLCont as varchar(4000)
	DECLARE @SQLStatement as varchar(4000)

	SET @AccountID = (SELECT AccountID FROM [User] WHERE ID = @UserID)

	SET @ProfileID = (SELECT ID FROM [User] WHERE ID = @UserID)


	
	if(@p_Si > 1)
	BEGIN
			SET @p_Si = ((@p_Si-1) * @p_iPage) + 1
			SET @p_iPage = (@p_Si + @p_iPage)-1
	END

		

	IF (@p_Filters IS NULL)
	BEGIN
		SET @SQLCont = 'select count(*) as cont from [Type]  as o
						INNER JOIN Account as a ON o.AccountID = a.ID
						INNER JOIN [Type] as ar ON o.parentID = ar.ID
						WHERE a.[Deleted] = 0 AND o.ParentID IS NOT NULL'		
	END
	ELSE
	BEGIN
		SET @SQLCont = 'select count(*) as cont from [Type] as o
						INNER JOIN Account as a ON o.AccountID = a.ID
						INNER JOIN [Type] as ar ON o.parentID = ar.ID
						WHERE a.[Deleted] = 0  AND o.ParentID IS NOT NULL AND o.Name like '  + '''%' +   @p_Filters +  '%'''		
	
	END
	
	

	IF (@p_Filters IS NULL)
	BEGIN	
			
			SET @SQLStatement = ' WITH OrderedNews AS
			(
			SELECT o.*,ar.Name as AreaName,
			row_number() over (order by o.name asc) rn
			FROM [Type] as o
			INNER JOIN Account as a ON o.AccountID = a.ID
			INNER JOIN [Type] as ar ON o.parentID = ar.ID
			WHERE  a.[Deleted] = 0 AND o.ParentID IS NOT NULL)
			SELECT * FROM OrderedNews
			WHERE rn BETWEEN ' +  cast(@p_Si as varchar(255))  + ' AND ' + cast(@p_IPage as varchar(255))
			+ 'ORDER BY '  + @p_SortColumnName

	END	
	ELSE
	BEGIN
			
			SET @SQLStatement = ' WITH OrderedNews AS
			(
			SELECT o.*,ar.Name as AreaName,

			row_number() over (order by o.name asc) rn
			FROM [Type] as o
			INNER JOIN Account as a ON o.AccountID = a.ID
			INNER JOIN [Type] as ar ON o.parentID = ar.ID
				WHERE o.Name like '  + '''%' +   @p_Filters +  '%''
			AND a.[Deleted] = 0 AND o.ParentID IS NOT NULL)
			SELECT *
				FROM OrderedNews
				
				WHERE rn BETWEEN ' +  cast(@p_Si as varchar(255))  + ' AND ' + cast(@p_IPage as varchar(255))
				+ 'ORDER BY '  + @p_SortColumnName
	END		

	exec (@SQLCont)

	exec (@SQLStatement)
	

END





GO





/****** Object:  StoredProcedure [dbo].[Type_ListToDataSet]    Script Date: 30-01-2015 12:26:17 ******/
DROP PROCEDURE [dbo].[Type_ListToDataSetWithoutParent]
GO

/****** Object:  StoredProcedure [dbo].[Type_ListToDataSet]    Script Date: 30-01-2015 12:26:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Type_ListToDataSetWithoutParent]
	@UserID AS INT,
	@p_Si INT, 
	@p_iPage INT, 
	@p_SortColumnName VARCHAR(255), 
	@p_Filters VARCHAR(255)
AS
BEGIN
	
	DECLARE @ProfileID AS INT
	DECLARE @AccountID AS INT
	DECLARE @SQLCont as varchar(4000)
	DECLARE @SQLStatement as varchar(4000)

	SET @AccountID = (SELECT AccountID FROM [User] WHERE ID = @UserID)

	SET @ProfileID = (SELECT ID FROM [User] WHERE ID = @UserID)


	
	if(@p_Si > 1)
	BEGIN
			SET @p_Si = ((@p_Si-1) * @p_iPage) + 1
			SET @p_iPage = (@p_Si + @p_iPage)-1
	END

		

	IF (@p_Filters IS NULL)
	BEGIN
		SET @SQLCont = 'select count(*) as cont from [Type]  as o
						INNER JOIN Account as a ON o.AccountID = a.ID
						WHERE a.[Deleted] = 0 AND o.ParentID IS  NULL'		
	END
	ELSE
	BEGIN
		SET @SQLCont = 'select count(*) as cont from [Type] as o
						INNER JOIN Account as a ON o.AccountID = a.ID
						WHERE a.[Deleted] = 0  AND o.ParentID IS  NULL AND o.Name like '  + '''%' +   @p_Filters +  '%'''		
	
	END
	
	

	IF (@p_Filters IS NULL)
	BEGIN	
			
			SET @SQLStatement = ' WITH OrderedNews AS
			(
			SELECT o.*,
			row_number() over (order by o.name asc) rn
			FROM [Type] as o
			INNER JOIN Account as a ON o.AccountID = a.ID
			WHERE  a.[Deleted] = 0 AND o.ParentID IS  NULL)
			SELECT * FROM OrderedNews
			WHERE rn BETWEEN ' +  cast(@p_Si as varchar(255))  + ' AND ' + cast(@p_IPage as varchar(255))
			+ 'ORDER BY '  + @p_SortColumnName

	END	
	ELSE
	BEGIN
			
			SET @SQLStatement = ' WITH OrderedNews AS
			(
			SELECT o.*,

			row_number() over (order by o.name asc) rn
			FROM [Type] as o
			INNER JOIN Account as a ON o.AccountID = a.ID
				WHERE o.Name like '  + '''%' +   @p_Filters +  '%''
			AND a.[Deleted] = 0 AND o.ParentID IS  NULL)
			SELECT *
				FROM OrderedNews
				
				WHERE rn BETWEEN ' +  cast(@p_Si as varchar(255))  + ' AND ' + cast(@p_IPage as varchar(255))
				+ 'ORDER BY '  + @p_SortColumnName
	END		

	exec (@SQLCont)

	exec (@SQLStatement)
	

END





GO






/****** Object:  StoredProcedure [dbo].[Type_List]    Script Date: 30-01-2015 12:45:26 ******/
DROP PROCEDURE [dbo].[Type_ListWithParent]
GO

/****** Object:  StoredProcedure [dbo].[Type_List]    Script Date: 30-01-2015 12:45:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[Type_ListWithParent]
	@Name as VARCHAR(255) = NULL,
	@AccountID AS INT = NULL
  
AS

BEGIN
    SELECT * 
    FROM [Type]
	WHERE (Name like '%' + @Name + '&' OR @Name IS NULL)
	AND AccountID = @AccountID AND ParentID IS NOT NULL
	ORDER BY Name
  
END


GO



/****** Object:  StoredProcedure [dbo].[Type_List]    Script Date: 30-01-2015 12:45:26 ******/
DROP PROCEDURE [dbo].[Type_ListByParentID]
GO

/****** Object:  StoredProcedure [dbo].[Type_List]    Script Date: 30-01-2015 12:45:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[Type_ListByParentID]
	@Name as VARCHAR(255) = NULL,
	@AccountID AS INT = NULL,
	@ParentID AS INT 
  
AS

BEGIN
    SELECT * 
    FROM [Type]
	WHERE (Name like '%' + @Name + '&' OR @Name IS NULL)
	AND AccountID = @AccountID AND ParentID = @ParentID
	ORDER BY Name
  
END


GO


DROP PROCEDURE [dbo].[Type_ListWithoutParent]
GO

/****** Object:  StoredProcedure [dbo].[Type_List]    Script Date: 30-01-2015 12:45:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[Type_ListWithoutParent]
	@Name as VARCHAR(255) = NULL,
	@AccountID AS INT = NULL
  
AS
BEGIN
    SELECT * 
    FROM [Type]
	WHERE (Name like '%' + @Name + '&' OR @Name IS NULL)
	AND AccountID = @AccountID AND ParentID IS NULL
	ORDER BY Name
  
END


GO



/****** Object:  StoredProcedure [dbo].[Type_CountByName]    Script Date: 30-01-2015 15:57:58 ******/
DROP PROCEDURE [dbo].[Type_CountByName]
GO

/****** Object:  StoredProcedure [dbo].[Type_CountByName]    Script Date: 30-01-2015 15:57:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Type_CountByName]
	@ID AS INT = NULL,
	@Name AS VARCHAR(255),
	@ParentID AS INT = NULL
AS
BEGIN
	IF(@ID IS NULL)
	BEGIN
		
		IF(@ParentID IS NULL)
		BEGIN
			SELECT COUNT(*) FROM Type 
			WHERE Name = @Name AND ParentID IS NULL
		END
		ELSE 
		BEGIN
			SELECT COUNT(*) FROM Type 
			WHERE Name = @Name AND ParentID =  @ParentID
		END
	END
	ELSE
	BEGIN
		
			
		IF(@ParentID IS NULL)
		BEGIN
			SELECT COUNT(*) FROM Type 
			WHERE Name = @Name AND ParentID IS NULL AND ID != @ID
		END
		ELSE 
		BEGIN
			SELECT COUNT(*) FROM Type 
			WHERE Name = @Name AND ParentID =  @ParentID AND ID != @ID
		END

		

	END 
END


GO



/****** Object:  StoredProcedure [dbo].[Type_Insert]    Script Date: 30-01-2015 17:34:39 ******/
DROP PROCEDURE [dbo].[Type_Insert]
GO

/****** Object:  StoredProcedure [dbo].[Type_Update]    Script Date: 30-01-2015 17:34:39 ******/
DROP PROCEDURE [dbo].[Type_Update]
GO

/****** Object:  StoredProcedure [dbo].[Type_Update]    Script Date: 30-01-2015 17:34:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Type_Update]
	@ID AS INT,
	@Name AS VARCHAR(255),
	@ModifiedBy AS INT,
	@ParentID AS INT = NULL
AS
BEGIN

UPDATE [dbo].[Type]
   SET [Name] = @Name
      ,[ModifiedBy] = @ModifiedBy
      ,[ModifiedDate] = GETDATE()
      ,[ParentID] = @ParentID
 WHERE ID = @ID

 SELECT @ID


END



GO

/****** Object:  StoredProcedure [dbo].[Type_Insert]    Script Date: 30-01-2015 17:34:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Type_Insert]
	@Name AS VARCHAR(255),
	@CreatedBy AS INT,
	@AccountID AS INT,
	@ParentID AS INT = NULL
AS
BEGIN

INSERT INTO [dbo].[Type]
           ([Name]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[AccountID]
		   ,[ParentID])
     VALUES
           (@Name
           ,@CreatedBy
           ,GETDATE()
           ,@AccountID
		   ,@ParentID)

	SELECT SCOPE_IDENTITY()
END



GO





/****** Object:  StoredProcedure [dbo].[Type_Insert]    Script Date: 30-01-2015 17:34:39 ******/
DROP PROCEDURE [dbo].[User_Type_Insert]
GO

/****** Object:  StoredProcedure [dbo].[Type_Update]    Script Date: 30-01-2015 17:34:39 ******/
DROP PROCEDURE [dbo].[User_Type_Update]
GO

/****** Object:  StoredProcedure [dbo].[Type_Update]    Script Date: 30-01-2015 17:34:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





---- =============================================
---- Author:		<Author,,Name>
---- Create date: <Create Date,,>
---- Description:	<Description,,>
---- =============================================
--CREATE PROCEDURE [dbo].[User_Type_Update]
--	@ID AS INT,
--	@Name AS VARCHAR(255),
--	@ModifiedBy AS INT,
--	@ParentID AS INT = NULL
--AS
--BEGIN

--UPDATE [dbo].[Type]
--   SET [Name] = @Name
--      ,[ModifiedBy] = @ModifiedBy
--      ,[ModifiedDate] = GETDATE()
--      ,[ParentID] = @ParentID
-- WHERE ID = @ID

-- SELECT @ID


--END



--GO

--/****** Object:  StoredProcedure [dbo].[Type_Insert]    Script Date: 30-01-2015 17:34:40 ******/
--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO



---- =============================================
---- Author:		<Author,,Name>
---- Create date: <Create Date,,>
---- Description:	<Description,,>
---- =============================================
--CREATE PROCEDURE [dbo].[User_Type_Insert]
--	@UserID AS INT,
--	@TypeID AS INT

--AS
--BEGIN

--	INSERT INTO [dbo].[User_Type]
--			   ([UserID]
--			   ,[TypeID])
--		 VALUES
--			   (@UserID 
--			   ,@TypeID)

--	SELECT SCOPE_IDENTITY()
--END



--GO

DROP PROCEDURE [dbo].[User_Type_Save]
GO


CREATE PROCEDURE [dbo].[User_Type_Save]
	@UserID AS INT,
	@TypeList AS VARCHAR(4000)

AS
BEGIN

	BEGIN TRAN
		DELETE FROM [User_Type] 
		WHERE [UserID] = @UserID

		IF @@ERROR<>0
		BEGIN
			ROLLBACK
			SELECT -1
		END


		IF(@TypeList != '')
			SET @TypeList = @TypeList + ','

		DECLARE @AuxType AS VARCHAR(512)
		DECLARE @NextIndexType AS INT
		DECLARE @StrIDType AS VARCHAR(20)

		SET @AuxType = @TypeList
		SET @NextIndexType = CHARINDEX(',',@AuxType)

		WHILE(@NextIndexType > 0)
		BEGIN
			SET @StrIDType = SUBSTRING(@AuxType, 1, @NextIndexType-1)

			IF (@StrIDType != '')
			BEGIN
				
				INSERT INTO [dbo].[User_Type]
						   ([UserID]
						   ,[TypeID])
					 VALUES
						   (@UserID 
						   ,CONVERT(INT, @StrIDType))

				IF @@ERROR<>0
				BEGIN
					ROLLBACK
					SELECT -2
				END

				SET @AuxType = SUBSTRING(@AuxType, @NextIndexType + 1, LEN(@AuxType)-@NextIndexType)
				SET @NextIndexType = CHARINDEX(',',@AuxType)
			END
		END

		IF @@ERROR<>0
		BEGIN
			ROLLBACK
			SELECT -3
		END
		ELSE
		BEGIN
			COMMIT
			SELECT 1
		END
		
END



GO




DROP PROCEDURE [dbo].[User_Type_ListByUserID]
GO


CREATE PROCEDURE [dbo].[User_Type_ListByUserID]
	@UserID AS INT,
	@ISAdmin AS BIT

AS
BEGIN

	IF(@ISAdmin = 1) -- Vê tudo
	BEGIN
		SELECT ID AS [TypeID],  * FROM [Type] 
		--WHERE [UserID] = @UserID
	END
	ELSE
	BEGIN
		SELECT * FROM [User_Type] 
		WHERE [UserID] = @UserID
	END
END

GO







/****** Object:  StoredProcedure [dbo].[Issue_GetByAccountIDPaginated]    Script Date: 02-02-2015 17:18:44 ******/
DROP PROCEDURE [dbo].[Issue_GetByAccountIDPaginatedWithTable]
GO





CREATE TYPE dbo.TypesTable AS TABLE
(
    ID INT NOT NULL
);
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Issue_GetByAccountIDPaginatedWithTable]
	@AccountID AS INT,
	@Priority AS INT = NULL,
	@State AS INT = NULL,
	@StartIndex AS INT,
	@NumRecords AS INT,
	@TypeIDs TypesTable READONLY 


AS
BEGIN
WITH OrderedIssues AS
				(
		SELECT i.[ID]
			  ,[Subject]
			  ,[Message]
			  ,[GPS]
			  ,null as [Blob]
			  ,[BlobName]
			  ,[BlobType]
			  ,[BlobLen]
			  ,[Priority]
			  ,[DateReported]
			  ,[ReportedBy]
			  ,[TypeID]
			  ,i.[AccountID]
			  ,[DateTaken]
			  ,r.Name as Reporter
			  ,t.Name as [Type],
			   (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = i.ID ORDER BY StateDate DESC) AS State,
	(SELECT TOP(1) Name FROM IssueStates as ist INNER JOIN State as s ON s.ID = ist.StateID  WHERE IssueID = i.ID ORDER BY StateDate DESC) AS StateName,
	(SELECT TOP(1) StateDate FROM IssueStates as ist INNER JOIN State as s ON s.ID = ist.StateID  WHERE IssueID = i.ID ORDER BY StateDate DESC) AS StateDate,

	t.Name as TypeName,
	row_number() over ( ORDER BY SortIndex, Priority DESC, DateReported) rn
		  FROM [dbo].[Issue] as i
		  INNER JOIN Reporter as r ON i.ReportedBy = r.ID
		  LEFT OUTER JOIN Type as t ON i.TypeID = t.ID
		  WHERE i.Deleted = 0
		  AND i.AccountID = @AccountID 
		  
		  AND (TypeID  IN (SELECT ID FROM @TypeIDs))
		  AND (Priority  = @Priority OR @Priority IS NULL)
		  AND Archived = 0

		  --AND ( (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = i.ID ORDER BY StateDate DESC) = @State OR @State IS NULL ) 

		  AND (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = i.ID  ORDER BY StateDate DESC) != 4

		  AND ( (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = i.ID ORDER BY StateDate DESC) = @State OR @State IS NULL ) 

		  )
				SELECT * FROM OrderedIssues
				WHERE rn BETWEEN @StartIndex  AND @StartIndex + @NumRecords 
			
END



GO




/****** Object:  StoredProcedure [dbo].[Issue_GetByAccountID]    Script Date: 03-02-2015 09:44:51 ******/
DROP PROCEDURE [dbo].[Issue_GetByAccountIDWithTable]
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Issue_GetByAccountIDWithTable]
	@AccountID AS INT,
	@Priority AS INT = NULL,
	@State AS INT = NULL,
	@TypeIDs TypesTable READONLY 
AS
BEGIN

		SELECT i.[ID]
			  ,[Subject]
			  ,[Message]
			  ,[GPS]
			  ,null as [Blob]
			  ,[BlobName]
			  ,[BlobType]
			  ,[BlobLen]
			  ,[Priority]
			  ,[DateReported]
			  ,[ReportedBy]
			  ,[TypeID]
			  ,i.[AccountID]
			  ,[DateTaken]
			  ,r.Name as Reporter
			  ,t.Name as [Type],
			   (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = i.ID ORDER BY StateDate DESC) AS State,
	(SELECT TOP(1) Name FROM IssueStates as ist INNER JOIN State as s ON s.ID = ist.StateID  WHERE IssueID = i.ID ORDER BY StateDate DESC) AS StateName,
	(SELECT TOP(1) StateDate FROM IssueStates as ist INNER JOIN State as s ON s.ID = ist.StateID  WHERE IssueID = i.ID ORDER BY StateDate DESC) AS StateDate,

	t.Name as TypeName,
			i.ImageRotation
		  FROM [dbo].[Issue] as i
		  INNER JOIN Reporter as r ON i.ReportedBy = r.ID
		  LEFT OUTER JOIN Type as t ON i.TypeID = t.ID
		  WHERE i.Deleted = 0
		  AND i.AccountID = @AccountID 
		   AND (TypeID  IN (SELECT ID FROM @TypeIDs))
		  AND (Priority  = @Priority OR @Priority IS NULL)
		  AND Archived = 0


		  AND (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = i.ID  ORDER BY StateDate DESC) != 4
		  AND ( (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = i.ID ORDER BY StateDate DESC) = @State OR @State IS NULL ) 



		  --ORDER BY Priority DESC, DateReported ASC, SortIndex ASC 
		  ORDER BY SortIndex, Priority DESC, DateReported
END




GO


DROP PROCEDURE [dbo].[Type_ListByUserID]
GO


CREATE PROCEDURE [dbo].[Type_ListByUserID]
	@UserID AS INT,
	@TypeIDs TypesTable READONLY, 
	@Name as VARCHAR(255) = NULL,
	@AccountID AS INT = NULL,
	@ISAdmin AS BIT

AS
BEGIN

	
	
	IF(@ISAdmin = 1) -- Vê tudo
	BEGIN
		SELECT ID as TypeID, * FROM [Type] 
		WHERE (Name like '%' + @Name + '&' OR @Name IS NULL)
		AND AccountID = @AccountID
		AND ParentID IS NOT NULL
		ORDER BY Name
		--WHERE [UserID] = @UserID
	END
	ELSE
	BEGIN
		SELECT * FROM [Type] as t
		INNER JOIN [User_Type] as ut ON t.ID = ut.TypeID
		WHERE ut.[UserID] = @UserID
		
		AND (Name like '%' + @Name + '&' OR @Name IS NULL)
		AND AccountID = @AccountID
		AND ParentID IS NOT NULL
		AND (TypeID  IN (SELECT ID FROM @TypeIDs))
		ORDER BY Name
		

	END
END

GO





--DROP PROCEDURE [dbo].[User_Type_listUsersInType]
--GO


--CREATE PROCEDURE [dbo].[User_Type_listUsersInType]
--	@TypeID AS INT
	
--AS
--BEGIN

--		SELECT * FROM [User] as u
		
--		INNER JOIN [User_Type] as ut ON u.ID = ut.UserID
--		WHERE ut.[TypeID] = @TypeID
--		AND u.Deleted = 0 AND u.Active = 1
--		ORDER BY Name
		
--END

--GO


DROP PROCEDURE [dbo].[User_ListUsersByTypes]
GO


CREATE PROCEDURE [dbo].[User_ListUsersByTypes]
	@TypeIDs TypesTable READONLY
	
AS
BEGIN

		SELECT DISTINCT u.* FROM [User] as u
		
		INNER JOIN [User_Type] as ut ON u.ID = ut.UserID
		WHERE ut.[TypeID] IN  (SELECT ID FROM @TypeIDs)
		AND u.Deleted = 0 AND u.Active = 1
		ORDER BY Name
		
END

GO


DROP PROCEDURE [dbo].[User_ListUsersByTypeID]
GO


CREATE PROCEDURE [dbo].[User_ListUsersByTypeID]
	@TypeID AS INT
	
AS
BEGIN

		SELECT DISTINCT u.* FROM [User] as u
		
		LEFT OUTER JOIN [User_Type] as ut ON u.ID = ut.UserID
		WHERE (ut.[TypeID]  = @TypeID OR u.IsAdmin = 1)
		AND u.Deleted = 0 AND u.Active = 1
		ORDER BY Name
		
END

GO


/****** Object:  StoredProcedure [dbo].[Issue_UpdatePriorityAndType]    Script Date: 03-02-2015 16:13:35 ******/
DROP PROCEDURE [dbo].[Issue_UpdatePriorityAndType]
GO

/****** Object:  StoredProcedure [dbo].[Issue_UpdatePriorityAndType]    Script Date: 03-02-2015 16:13:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[Issue_UpdatePriorityAndType]
    @ID AS INT, 
    @Priority AS INT, 
    @TypeID AS INT = NULL,
	@State AS INT = NULL,
	@UserID AS INT,
	@Message as VARCHAR(MAX)  = NULL,
	@Notes as VARCHAR(MAX)  = NULL,
	@AssignedTo AS INT = NULL

-- WITH ENCRYPTION
AS
BEGIN
    BEGIN TRAN

    UPDATE [Issue] SET 
            [Priority] = @Priority,
            [TypeID] = @TypeID,
			[AssignedTo] = @AssignedTo
			
			
            
    WHERE  [ID] = @ID  



    IF @@ERROR<>0
    BEGIN
        ROLLBACK
        SELECT 0
    END
    ELSE
    BEGIN
		DECLARE @dt AS DATETIME = GETDATE()

		--DECLARE @LastState AS INT

		--SET @LastState = (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = @ID ORDER BY StateDate DESC)

		--IF(@LastState IS NOT NULL)
		--BEGIN
		--	IF(@LastState != @State)
		--	BEGIN
		--		-- colocar estado novo
		--		EXEC [IssueStates_Insert]  @id,  @dt,  @UserID,  @State,@Message,@Notes
		--	END

		--END
		--ELSE
		--BEGIN
		--	-- colocar estado novo
		--	EXEC [IssueStates_Insert]  @id,  @dt,  @UserID,  @State,@Message,@Notes
		--END


		-- colocar estado novo
		EXEC [IssueStates_Insert]  @id,  @dt,  @UserID,  @State,@Message,@Notes


		 IF @@ERROR<>0
		BEGIN
			ROLLBACK
			SELECT 0
		END

		ELSE
		BEGIN
			COMMIT
			SELECT @ID
		END
    END
END



GO





DROP PROCEDURE [dbo].[Issue_GetByAccountIDWithTable]
GO


CREATE PROCEDURE [dbo].[Issue_GetByAccountIDWithTable]
	@AccountID AS INT,
	@Priority AS INT = NULL,
	@State AS INT = NULL,
	@TypeIDs TypesTable READONLY 
AS
BEGIN

		SELECT i.[ID]
			  ,[Subject]
			  ,[Message]
			  ,[GPS]
			  ,null as [Blob]
			  ,[BlobName]
			  ,[BlobType]
			  ,[BlobLen]
			  ,[Priority]
			  ,[DateReported]
			  ,[ReportedBy]
			  ,[TypeID]
			  ,i.[AccountID]
			  ,[DateTaken]
			  ,r.Name as Reporter
			  ,t.Name as [Type],
			   (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = i.ID ORDER BY StateDate DESC) AS State,
	(SELECT TOP(1) Name FROM IssueStates as ist INNER JOIN State as s ON s.ID = ist.StateID  WHERE IssueID = i.ID ORDER BY StateDate DESC) AS StateName,
	(SELECT TOP(1) StateDate FROM IssueStates as ist INNER JOIN State as s ON s.ID = ist.StateID  WHERE IssueID = i.ID ORDER BY StateDate DESC) AS StateDate,

	t.Name as TypeName,i.ImageRotation, i.AssignedTo
			
		  FROM [dbo].[Issue] as i
		  INNER JOIN Reporter as r ON i.ReportedBy = r.ID
		  LEFT OUTER JOIN Type as t ON i.TypeID = t.ID
		  WHERE i.Deleted = 0
		  AND i.AccountID = @AccountID 
		   AND (TypeID  IN (SELECT ID FROM @TypeIDs))
		  AND (Priority  = @Priority OR @Priority IS NULL)
		  AND Archived = 0


		  AND (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = i.ID  ORDER BY StateDate DESC) != 4
		  AND ( (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = i.ID ORDER BY StateDate DESC) = @State OR @State IS NULL ) 



		  --ORDER BY Priority DESC, DateReported ASC, SortIndex ASC 
		  ORDER BY SortIndex, Priority DESC, DateReported
END





GO

DROP PROCEDURE [dbo].[Issue_GetByAccountIDPaginatedWithTable]
GO

CREATE PROCEDURE [dbo].[Issue_GetByAccountIDPaginatedWithTable]
	@AccountID AS INT,
	@Priority AS INT = NULL,
	@State AS INT = NULL,
	@StartIndex AS INT,
	@NumRecords AS INT,
	@TypeIDs TypesTable READONLY 


AS
BEGIN
WITH OrderedIssues AS
				(
		SELECT i.[ID]
			  ,[Subject]
			  ,[Message]
			  ,[GPS]
			  ,null as [Blob]
			  ,[BlobName]
			  ,[BlobType]
			  ,[BlobLen]
			  ,[Priority]
			  ,[DateReported]
			  ,[ReportedBy]
			  ,[TypeID]
			  ,i.[AccountID]
			  ,[DateTaken]
			  ,r.Name as Reporter
			  ,t.Name as [Type],
			   (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = i.ID ORDER BY StateDate DESC) AS State,
	(SELECT TOP(1) Name FROM IssueStates as ist INNER JOIN State as s ON s.ID = ist.StateID  WHERE IssueID = i.ID ORDER BY StateDate DESC) AS StateName,
	(SELECT TOP(1) StateDate FROM IssueStates as ist INNER JOIN State as s ON s.ID = ist.StateID  WHERE IssueID = i.ID ORDER BY StateDate DESC) AS StateDate,

	t.Name as TypeName, i.AssignedTo,
	row_number() over ( ORDER BY SortIndex, Priority DESC, DateReported) rn
		  FROM [dbo].[Issue] as i
		  INNER JOIN Reporter as r ON i.ReportedBy = r.ID
		  LEFT OUTER JOIN Type as t ON i.TypeID = t.ID
		  WHERE i.Deleted = 0
		  AND i.AccountID = @AccountID 
		  
		  AND (TypeID  IN (SELECT ID FROM @TypeIDs))
		  AND (Priority  = @Priority OR @Priority IS NULL)
		  AND Archived = 0

		  --AND ( (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = i.ID ORDER BY StateDate DESC) = @State OR @State IS NULL ) 

		  AND (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = i.ID  ORDER BY StateDate DESC) != 4

		  AND ( (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = i.ID ORDER BY StateDate DESC) = @State OR @State IS NULL ) 

		  )
				SELECT * FROM OrderedIssues
				WHERE rn BETWEEN @StartIndex  AND @StartIndex + @NumRecords 
			
END




GO

DROP PROCEDURE [dbo].[Issue_GetArchivedByAccountID]
GO


CREATE PROCEDURE [dbo].[Issue_GetArchivedByAccountID]
	@AccountID AS INT,

	@Priority AS INT = NULL
AS
BEGIN

		SELECT i.[ID]
			  ,[Subject]
			  ,[Message]
			  ,[GPS]
			  ,null as [Blob]
			  ,[BlobName]
			  ,[BlobType]
			  ,[BlobLen]
			  ,[Priority]
			  ,[DateReported]
			  ,[ReportedBy]
			  ,[TypeID]
			  ,i.[AccountID]
			  ,[DateTaken]
			  ,r.Name as Reporter
			  ,t.Name as [Type],
			   (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = i.ID ORDER BY StateDate DESC) AS State,
	(SELECT TOP(1) Name FROM IssueStates as ist INNER JOIN State as s ON s.ID = ist.StateID  WHERE IssueID = i.ID ORDER BY StateDate DESC) AS StateName,
	(SELECT TOP(1) StateDate FROM IssueStates as ist INNER JOIN State as s ON s.ID = ist.StateID  WHERE IssueID = i.ID ORDER BY StateDate DESC) AS StateDate,
	t.Name as TypeName, i.AssignedTo
		  FROM [dbo].[Issue] as i
		  INNER JOIN Reporter as r ON i.ReportedBy = r.ID
		  LEFT OUTER JOIN Type as t ON i.TypeID = t.ID
		  WHERE i.Deleted = 0
		  AND i.AccountID = @AccountID 
		  
		  AND (Priority  = @Priority OR @Priority IS NULL)
		  --AND Archived = 1

		  AND (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = i.ID  ORDER BY StateDate DESC) = 4

		  


		  --ORDER BY Priority DESC, DateReported ASC, SortIndex ASC 
		  ORDER BY SortIndex, Priority DESC, DateReported
END





GO

DROP PROCEDURE [dbo].[Issue_GetByAccountID]
GO
CREATE PROCEDURE [dbo].[Issue_GetByAccountID]
	@AccountID AS INT,
	@TypeID AS INT = NULL,
	@Priority AS INT = NULL,
	@State AS INT = NULL
AS
BEGIN

		SELECT i.[ID]
			  ,[Subject]
			  ,[Message]
			  ,[GPS]
			  ,null as [Blob]
			  ,[BlobName]
			  ,[BlobType]
			  ,[BlobLen]
			  ,[Priority]
			  ,[DateReported]
			  ,[ReportedBy]
			  ,[TypeID]
			  ,i.[AccountID]
			  ,[DateTaken]
			  ,r.Name as Reporter
			  ,t.Name as [Type],
			   (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = i.ID ORDER BY StateDate DESC) AS State,
	(SELECT TOP(1) Name FROM IssueStates as ist INNER JOIN State as s ON s.ID = ist.StateID  WHERE IssueID = i.ID ORDER BY StateDate DESC) AS StateName,
	(SELECT TOP(1) StateDate FROM IssueStates as ist INNER JOIN State as s ON s.ID = ist.StateID  WHERE IssueID = i.ID ORDER BY StateDate DESC) AS StateDate,

	t.Name as TypeName,
			i.ImageRotation, i.AssignedTo
		  FROM [dbo].[Issue] as i
		  INNER JOIN Reporter as r ON i.ReportedBy = r.ID
		  LEFT OUTER JOIN Type as t ON i.TypeID = t.ID
		  WHERE i.Deleted = 0
		  AND i.AccountID = @AccountID 
		  AND (TypeID  = @TypeID OR @TypeID IS NULL)
		  AND (Priority  = @Priority OR @Priority IS NULL)
		  AND Archived = 0

		  --AND ( (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = i.ID ORDER BY StateDate DESC) = @State OR @State IS NULL ) 

		  AND (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = i.ID  ORDER BY StateDate DESC) != 4

		  AND ( (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = i.ID ORDER BY StateDate DESC) = @State OR @State IS NULL ) 



		  --ORDER BY Priority DESC, DateReported ASC, SortIndex ASC 
		  ORDER BY SortIndex, Priority DESC, DateReported
END




GO
DROP PROCEDURE [dbo].[Issue_List]
GO

CREATE PROCEDURE [dbo].[Issue_List] 
    @Priority AS INT, 
    @DateReported AS DATETIME = NULL, 
    @ReportedBy AS INT = NULL, 
    @TypeID AS INT = NULL, 
    @AccountID AS INT
-- WITH ENCRYPTION
AS
BEGIN
    SELECT
		 [Issue].[ID],
		 [Subject],
		 [Message],
		 [GPS],
		 [Blob],
		 [BlobName],
		 [BlobType],
		 [BlobLen],
		 [Priority],
		 [DateReported],
		 [ReportedBy],
		 [TypeID],
		[Issue].[AccountID],
		 geography::STGeomFromText(cast(gps as varchar(255)), 4326).Lat AS Latitude,
		 geography::STGeomFromText(cast(gps as varchar(255)), 4326).Long AS Longitude,

		 (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = [Issue].ID ORDER BY StateDate DESC) AS State,
	
	(SELECT TOP(1) Name FROM IssueStates as ist INNER JOIN State as s ON s.ID = ist.StateID  WHERE IssueID = [Issue].ID ORDER BY StateDate DESC) AS StateName,
	t.Name as TypeName,
			ImageRotation, AssignedTo
    FROM [Issue] 
	LEFT OUTER JOIN Type as t ON t.ID = [Issue].TypeID 
    WHERE  ([Issue].[AccountID] = @AccountID) AND Deleted = 0 AND 
		   ([Priority] = @Priority OR  @Priority IS NULL) AND 
		   ([ReportedBy] = @ReportedBy OR @ReportedBy IS NULL) AND 
		   ([TypeID] = @TypeID OR @TypeID IS NULL) 

	--ORDER BY Priority DESC, DateReported ASC 
	ORDER BY SortIndex, Priority DESC, DateReported
END




GO



DROP PROCEDURE [dbo].[Issue_GetByReporterID]
GO

CREATE PROCEDURE [dbo].[Issue_GetByReporterID]
	@ReporterID AS INT,
	@TypeID AS INT = NULL,
	@Priority AS INT = NULL,
	@AccountID AS INT = NULL
AS
BEGIN

		SELECT i.[ID]
			  ,[Subject]
			  ,[Message]
			  ,[GPS]
			  , [Blob]
			  ,[BlobName]
			  ,[BlobType]
			  ,[BlobLen]
			  ,[Priority]
			  ,[DateReported]
			  ,[ReportedBy]
			  ,[TypeID]
			  ,i.[AccountID]
			  ,[DateTaken]
			  ,r.Name as Reporter
			  ,t.Name as [Type]

			  ,geography::STGeomFromText(cast(gps as varchar(255)), 4326).Lat AS Latitude,
			geography::STGeomFromText(cast(gps as varchar(255)), 4326).Long AS Longitude ,SortIndex
			,(SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = i.ID ORDER BY StateDate DESC) AS State,
			(SELECT TOP(1) Name FROM IssueStates as ist INNER JOIN State as s ON s.ID = ist.StateID  WHERE IssueID = i.ID ORDER BY StateDate DESC) AS StateName,
			t.Name as TypeName,
			i.ImageRotation
		  FROM [dbo].[Issue] as i
		  INNER JOIN Reporter as r ON i.ReportedBy = r.ID
		  LEFT OUTER JOIN Type as t ON i.TypeID = t.ID
		  WHERE i.Deleted = 0
		  AND ReportedBy = @ReporterID
		  AND (TypeID  = @TypeID OR @TypeID IS NULL)
		  AND (Priority  = @Priority OR @Priority IS NULL)
		  AND (i.AccountID  = @AccountID OR @AccountID IS NULL)
		  --ORDER BY Priority DESC, DateReported ASC, SortIndex ASC 
		  ORDER BY  DateReported desc
END





GO




USE [BIRT]
GO

/****** Object:  StoredProcedure [dbo].[User_Update]    Script Date: 04-02-2015 10:29:40 ******/
DROP PROCEDURE [dbo].[User_Update]
GO

/****** Object:  StoredProcedure [dbo].[User_Update]    Script Date: 04-02-2015 10:29:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[User_Update] 
	 @ID AS INT, 
	 @Name AS VARCHAR(512) , 
	 @Email AS VARCHAR(255) , 
	 @ModifiedBy AS INT,
	 @ProfileID AS INT
		 

AS
BEGIN
	UPDATE [User] SET 
	 [Name] = @Name
	 ,[Email] = @Email
	 ,[ProfileID] = @ProfileID
	 ,[ModifiedBy] = @ModifiedBy
	 ,[ModifiedDate] = GETDATE()
	WHERE [ID] = @ID
	
	SELECT @ID

END




GO






DROP PROCEDURE [dbo].[Issue_GetArchivedByAccountIDPaginatedWithTable]
GO






-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Issue_GetArchivedByAccountIDPaginatedWithTable]
	@AccountID AS INT,
	@Priority AS INT = NULL,
	@State AS INT = NULL,
	@StartIndex AS INT,
	@NumRecords AS INT,
	@TypeIDs TypesTable READONLY 


AS
BEGIN
WITH OrderedIssues AS
				(
		SELECT i.[ID]
			  ,[Subject]
			  ,[Message]
			  ,[GPS]
			  ,null as [Blob]
			  ,[BlobName]
			  ,[BlobType]
			  ,[BlobLen]
			  ,[Priority]
			  ,[DateReported]
			  ,[ReportedBy]
			  ,[TypeID]
			  ,i.[AccountID]
			  ,[DateTaken]
			  ,r.Name as Reporter
			  ,t.Name as [Type],
			   (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = i.ID ORDER BY StateDate DESC) AS State,
	(SELECT TOP(1) Name FROM IssueStates as ist INNER JOIN State as s ON s.ID = ist.StateID  WHERE IssueID = i.ID ORDER BY StateDate DESC) AS StateName,
	(SELECT TOP(1) StateDate FROM IssueStates as ist INNER JOIN State as s ON s.ID = ist.StateID  WHERE IssueID = i.ID ORDER BY StateDate DESC) AS StateDate,

	t.Name as TypeName,
	row_number() over ( ORDER BY SortIndex, Priority DESC, DateReported) rn
		  FROM [dbo].[Issue] as i
		  INNER JOIN Reporter as r ON i.ReportedBy = r.ID
		  LEFT OUTER JOIN Type as t ON i.TypeID = t.ID
		  WHERE i.Deleted = 0
		  AND i.AccountID = @AccountID 
		  
		  AND (TypeID  IN (SELECT ID FROM @TypeIDs))
		  AND (Priority  = @Priority OR @Priority IS NULL)
		 

		  --AND ( (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = i.ID ORDER BY StateDate DESC) = @State OR @State IS NULL ) 

		  AND (SELECT TOP(1) StateID FROM IssueStates WHERE IssueID = i.ID  ORDER BY StateDate DESC) = 4


		  )
				SELECT * FROM OrderedIssues
				WHERE rn BETWEEN @StartIndex  AND @StartIndex + @NumRecords 
			
END



GO


DROP PROCEDURE [dbo].[Issue_Fix]
GO


CREATE PROCEDURE Issue_Fix
	@IssueID AS INT, 
    @UserID AS INT = NULL, 
    @StateID AS INT,
	@InternalNotes as VARCHAR(MAX)  = NULL,
	@GPS AS GEOGRAPHY, 
    @Blob AS VARBINARY(MAX) , 
    @BlobName AS VARCHAR(255) , 
    @BlobType AS VARCHAR(50) , 
    @BlobLen AS INT,
	@ReporterIDClosed AS INT = NULL 
AS
BEGIN
	 INSERT INTO [IssueStates] (
             [IssueID]
            ,[StateDate]
            ,[UserID]
            ,[StateID]
			,InternalNotes
			,[GPS]
			,[Blob]
            ,[BlobName]
            ,[BlobType]
            ,[BlobLen]
			,[ReporterIDClosed]
            )
            VALUES (
            @IssueID
            ,GETDATE()
            ,@UserID
            ,@StateID
			,@InternalNotes
			,@GPS
            ,@Blob
            ,@BlobName
            ,@BlobType
            ,@BlobLen
			,@ReporterIDClosed
            )

  SELECT SCOPE_IDENTITY()
END
GO






DROP PROCEDURE [dbo].[User_UpdateMyProfile]
GO


CREATE PROCEDURE [User_UpdateMyProfile]
	 @ID AS INT, 
	 @Name AS VARCHAR(512) , 
	 @Email AS VARCHAR(255) , 
	 @ReceiveNotifications AS BIT 
AS
BEGIN
	 UPDATE [User] SET 
		 [Name] = @Name
		 ,[Email] = @Email
		 ,[ModifiedBy] = @ID
		 ,[ModifiedDate] = GETDATE()
		 ,[ReceiveNotifications] =@ReceiveNotifications
	WHERE [ID] = @ID
	
	SELECT @ID
END
GO



DROP PROCEDURE [dbo].[User_ChangeMyPassword]
GO


CREATE PROCEDURE [User_ChangeMyPassword]
	 @UserID AS INT, 
	 @Password AS VARCHAR(512) 
AS
BEGIN
	 UPDATE [User] SET 
		 [Password] = @Password
		 ,[ModifiedBy] = @UserID
		 ,[ModifiedDate] = GETDATE()
		 
	WHERE [ID] = @UserID
	
	SELECT @UserID
END
GO


DROP PROCEDURE [dbo].[IssueStates_GetIssueFix]
GO


CREATE PROCEDURE [IssueStates_GetIssueFix]
	 @IssueID AS INT
AS
BEGIN
	 
	-- SELECT ist.*, s.Name as StateName, u.Name as [By] FROM IssueStates as ist

	--INNER JOIN Issue as i ON ist.IssueID = i.ID 
	--INNER JOIN State as s ON ist.StateID = s.ID
	--LEFT OUTER JOIN [User] as u ON ist.UserID = u.ID
	--WHERE i.ID = @ID

	 SELECT TOP(1) ist.*,  s.Name as StateName, u.Name as [By] FROM IssueStates as ist 
	 LEFT OUTER JOIN [User] as u ON u.ID = ist.UserID
	 LEFT OUTER JOIN [reporter] as r ON r.ID = ist.ReporterIDClosed
	 INNER JOIN Issue as i ON ist.IssueID = i.ID 
	INNER JOIN State as s ON ist.StateID = s.ID
		 
	WHERE [IssueID] = @IssueID
	And StateID = 4
	ORDER BY StateDate DESC
END
GO



USE [BIRT]
GO

/****** Object:  StoredProcedure [dbo].[Issue_Insert]    Script Date: 05-02-2015 11:35:11 ******/
DROP PROCEDURE [dbo].[Issue_Insert]
GO

/****** Object:  StoredProcedure [dbo].[Issue_Insert]    Script Date: 05-02-2015 11:35:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[Issue_Insert]
    @Subject AS VARCHAR(255) , 
    @Message AS VARCHAR(MAX) , 
    @GPS AS GEOGRAPHY, 
    @Blob AS VARBINARY(MAX) , 
    @BlobName AS VARCHAR(255) , 
    @BlobType AS VARCHAR(50) , 
    @BlobLen AS INT, 
    @Priority AS INT, 
   
    @ReportedBy AS INT, 
    @TypeID AS INT, 
    @AccountID AS INT,
	@DateTaken as datetime,
	@ImageRotation AS INT = NULL,
	@Device AS VARCHAR(255) = NULL,
	@CameraType AS INT = NULL
-- WITH ENCRYPTION
AS
BEGIN


	DECLARE @SortIndex AS INT

	SET @SortIndex = (SELECT TOP(1) SortIndex FROM Issue  ORDER BY SortIndex DESC)

	IF(@SortIndex IS NULL)
		SET @SortIndex = 1
	ELSE
		SET @SortIndex = @SortIndex + 1
    DECLARE @id AS INT

    BEGIN TRAN

    INSERT INTO [Issue] (
            [Subject]
            ,[Message]
            ,[GPS]
            ,[Blob]
            ,[BlobName]
            ,[BlobType]
            ,[BlobLen]
            ,[Priority]
            ,[DateReported]
            ,[ReportedBy]
            ,[TypeID]
            ,[AccountID]
			,[DateTaken]
			,[SortIndex]
			,[ImageRotation]
			,[Device]
			
            )
            VALUES (
            @Subject
            ,@Message
            ,@GPS
            ,@Blob
            ,@BlobName
            ,@BlobType
            ,@BlobLen
            ,@Priority
            ,GetDate()
            ,@ReportedBy
            ,@TypeID
            ,@AccountID
			,@DateTaken
			,@SortIndex
			,@ImageRotation
			,@Device
			
            )

    SET @id=SCOPE_IDENTITY()

    IF @@ERROR<>0
    BEGIN
        ROLLBACK
        SELECT 0
    END
    ELSE
    BEGIN
		-- colocar estado novo
		DECLARE @dt AS DATETIME = GETDATE()
		EXEC [IssueStates_Insert]  @id,  @dt,  NULL,  1


        COMMIT
        SELECT @id
    END
END


GO


