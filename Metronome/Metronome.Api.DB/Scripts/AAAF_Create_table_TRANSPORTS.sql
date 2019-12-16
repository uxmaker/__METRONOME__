create table metromind.Transports
(
    id int identity(0, 1),
    [name] nvarchar(32) not null,
    X float not null,
    Y float not null,
    lineID int not null,

    constraint PK_transports primary key(id),
    constraint FK_transports_lines foreign key(lineID) references metromind.Lines(id)
);

