CREATE PROCEDURE metromind.SCreateMembers
(
	@email NVARCHAR(128),
	@password VARBINARY(255),
	@id INT OUT
)
AS BEGIN
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
	BEGIN TRAN;
		IF EXISTS( SELECT * FROM metromind.Members m WHERE m.email = @email )
		BEGIN
			ROLLBACK;
			RETURN 1;
		END;
		INSERT INTO metromind.Members(email, [password]) VALUES (@email, @password);
		SELECT @id = SCOPE_IDENTITY();
	COMMIT;
	RETURN 0;
END;