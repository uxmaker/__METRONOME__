Create PROCEDURE MTN.sCreateJointure
(
	@LigneCode NVARCHAR(32),
	@StopAreaName1 NVARCHAR(255) ,
	@StopAreaName2 NVARCHAR(255) ,
	@Id INT OUT
)
AS BEGIN
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
	BEGIN TRAN;
		BEGIN
			ROLLBACK;
			RETURN 1;
		END;
		Insert into MTN.Jointure(LigneId,StopAreaId1,StopAreaId2) values ((select Id from MTN.Line where code=@LigneCode),(select Id from MTN.StopArea s where s.[Name]=@StopAreaName1),(select Id from MTN.StopArea s where s.[Name]=@StopAreaName2))		
		SELECT @Id = SCOPE_IDENTITY();
	COMMIT;
	RETURN 0;
END;