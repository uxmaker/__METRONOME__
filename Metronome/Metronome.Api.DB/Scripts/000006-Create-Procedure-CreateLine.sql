CREATE PROCEDURE MTN.sCreateLine
(
	@API_ID NVARCHAR(64),
	@Name NVARCHAR(255),
	@Color NVARCHAR(8),
	@Code NVARCHAR(32),
	@Opening_time DATETIME2,
	@Closing_time DATETIME2,
	@Id INT OUT
)
AS BEGIN
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
	BEGIN TRAN;
		INSERT INTO MTN.Line(API_ID, [Name], Color, Opening_time, Closing_time, Code) VALUES (@API_ID, @Name, @Color, @Opening_time, @Closing_time, @Code);
		SELECT @Id = SCOPE_IDENTITY();
	COMMIT;
	RETURN 0;
END;