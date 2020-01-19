Create PROCEDURE MTN.sCreateHorraires
(
	@Arrival_time  NVARCHAR (10),
	@StopName NVARCHAR(255) ,
	@LineId int ,
	@Direction nvarchar(255),
	@Id INT OUT
)
AS BEGIN
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
	BEGIN TRAN;
		IF EXISTS( SELECT * FROM MTN.Horraires h WHERE  h.Arrival_time =@Arrival_time and h.StopAreaId =( select id from MTN.StopArea where [name]=@StopName) and h.LineId = @LineId and h.Direction = @Direction)
		BEGIN
			ROLLBACK;
			RETURN 1;
		END;
		Insert into MTN.Horraires(Arrival_time,StopAreaId,LineId,Direction) values (@Arrival_time,( select id from MTN.StopArea where [name]=@StopName),@LineId,@Direction)
		SELECT @Id = SCOPE_IDENTITY();
	COMMIT;
	RETURN 0;
END;