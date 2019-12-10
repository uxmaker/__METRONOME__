create table MTN.Jointure
(
	Id int identity(0,1)not null,
	LigneId int not null,
	StopAreaId1 int not null ,
	StopAreaId2 int not null,

	Constraint PK_JointureId primary key (Id),
	Constraint FK_Jointure_Line foreign key (LigneId) references MTN.Line (Id),
	Constraint FK_Jointure_Station1 foreign key (StopAreaId1) references MTN.StopArea (Id),
	Constraint FK_Jointure_Station2 foreign key (StopAreaId2) references MTN.StopArea (Id),
);
