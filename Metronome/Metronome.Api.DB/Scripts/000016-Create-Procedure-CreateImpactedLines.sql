Create PROCEDURE MTN.sCreateImpactedLInes
(
	@DisruptionId int ,
	@LineId int ,
	@Id INT OUT
)
AS BEGIN
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
	BEGIN TRAN;
		IF EXISTS( SELECT * FROM MTN.ImpactedLines i WHERE  i.DisruptionId = @DisruptionId and i.LineId = @LineId )
		BEGIN
			ROLLBACK;
			RETURN 1;
		END;
		Insert into MTN.ImpactedLines (DisruptionId,LineId) values (@DisruptionId,@LineId)
		SELECT @Id = SCOPE_IDENTITY();
	COMMIT;
	RETURN 0;
END;