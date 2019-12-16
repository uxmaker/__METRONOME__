create table metromind.Stations
(
    id int,
    api_id nvarchar(64) not null,
    [name] nvarchar(32) not null,
    X float not null,
    Y float not null,

    constraint PK_stations primary key(id),
	constraint UK_StationsId UNIQUE (api_id),
	constraint UK_StationsName UNIQUE ([name])
);