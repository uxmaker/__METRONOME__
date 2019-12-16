CREATE PROCEDURE metromind.SCreateStations
(
	@api_id NVARCHAR(64),
	@name NVARCHAR(255),
	@x FLOAT,
	@y FLOAT,
	@id INT OUT
)
AS BEGIN
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
	BEGIN TRAN;
		IF EXISTS( SELECT * FROM metromind.Stations s WHERE s.x = @x AND s.y = @y )
		BEGIN
			ROLLBACK;
			RETURN 1;
		END;
		INSERT INTO metromind.Stations(api_id, [name], x, y) VALUES (@api_id, @name, @x, @y);
		SELECT @id = SCOPE_IDENTITY();
	COMMIT;
	RETURN 0;
END;