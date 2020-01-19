Create table MTN.Disruption
(
	Id INT identity(0,1),
	[Status] nvarchar(10) not null ,
	TextMessage nvarchar(255) not null, 
	API_ID nvarchar(255) not null ,

	CONSTRAINT PK_DisrutpionsId PRIMARY KEY (Id),
	CONSTRAINT UK_DisruptionsApiId UNIQUE (API_ID)
)