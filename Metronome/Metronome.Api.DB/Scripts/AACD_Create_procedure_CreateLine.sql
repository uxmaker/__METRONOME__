CREATE PROCEDURE metromind.sCreateLine
(
	@api_id NVARCHAR(64),
	@name NVARCHAR(255),
	@color NVARCHAR(8),
	@code NVARCHAR(32),
	@cpening_time DATETIME2,
	@closing_time DATETIME2,
	@id INT OUT
)
AS BEGIN
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
	BEGIN TRAN;
		INSERT INTO metromind.Lines(api_id, [name], color, opening_time, closing_time, code) VALUES (@api_id, @name, @color, @opening_time, @closing_time, @code);
		SELECT @id = SCOPE_IDENTITY();
	COMMIT;
	RETURN 0;
END;