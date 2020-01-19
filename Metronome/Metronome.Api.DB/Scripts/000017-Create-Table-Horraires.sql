Create table MTN.Horraires
(
	Id INT identity(0,1),
	Arrival_time nvarchar(10) not null ,
	StopAreaId int not null,
	LineId int not null,
	Direction nvarchar(255) not null,

	CONSTRAINT PK_HorrairesId PRIMARY KEY (Id),
	CONSTRAINT FK_Horraires_StopArea foreign key (StopAreaId) references MTN.StopArea (Id),
	CONSTRAINT FK_Horraires_Line foreign key (LineId) references MTN.Line (Id)
)