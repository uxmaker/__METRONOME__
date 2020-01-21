Create PROCEDURE MTN.sCreateDisruption
(
	@Status NVARCHAR (10),
	@TextMessage NVARCHAR(255) ,
	@API_ID NVARCHAR(255) ,
	@Id INT OUT
)
AS BEGIN
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
	BEGIN TRAN;
		IF EXISTS( SELECT * FROM MTN.Disruption d WHERE  d.API_ID = @API_ID )
		BEGIN
			ROLLBACK;
			RETURN 1;
		END;
		Insert into MTN.Disruption([Status],TextMessage,API_ID) values (@Status,@TextMessage,@API_ID)
		SELECT @Id = SCOPE_IDENTITY();
	COMMIT;
	RETURN 0;
END;