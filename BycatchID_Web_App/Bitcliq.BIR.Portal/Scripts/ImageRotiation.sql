--23-01-2014
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [object_id]=object_id('Issue') AND [name]='ImageRotation')
BEGIN
	
	ALTER TABLE Issue ADD ImageRotation INT

END
GO

DROP PROCEDURE [dbo].[Issue_Insert]
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
	@ImageRotation AS INT = NULL
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
			ImageRotation
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



DROP PROCEDURE [dbo].[Issue_GetByAccountID]
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
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
			i.ImageRotation
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



DROP PROCEDURE [dbo].[Issue_GetArchivedByAccountID]
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
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
	t.Name as TypeName
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