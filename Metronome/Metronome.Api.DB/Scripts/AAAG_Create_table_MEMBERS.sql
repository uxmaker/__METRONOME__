CREATE TABLE metromind.Members
(
	id INT IDENTITY(0,1),
	email NVARCHAR(128) COLLATE Latin1_General_CI_AI NOT NULL,
	[password] VARBINARY(255) NOT NULL,

	CONSTRAINT PK_MemberId PRIMARY KEY (id),
	CONSTRAINT UK_MemberMail UNIQUE (email),
);