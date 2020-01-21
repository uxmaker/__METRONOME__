Create table MTN.ImpactedLines
(
	Id INT identity(0,1),
	DisruptionId int not null,
	LineId int not null,

	Constraint PK_ImpactedLinesId primary key (Id),
	constraint FK_ImpactedLines_Line foreign key (LineId) references MTN.Line (Id),
	constraint FK_ImpactedLines_Disruption foreign key (DisruptionId) references MTN.Disruption (Id),
)