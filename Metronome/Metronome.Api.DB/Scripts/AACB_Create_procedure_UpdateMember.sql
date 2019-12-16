CREATE PROCEDURE metromind.SUpdateMembers
(
	@id INT,
	@password varbinary(255)
)
AS BEGIN
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
	BEGIN TRAN;
		UPDATE metromind.Members set [password] = @password where id = @id;
	COMMIT;
	RETURN 0;
END;