create table metromind.Links
(
    id int identity(0, 1),
    stationAID int not null,
    stationBID int not null,
    Tmoy time,
    [state] int not null,

    constraint PK_links primary key(id),
    constraint FK_links_stations foreign key(stationAID) references metromind.Stations(id),
    constraint FK_links_stations foreign key(stationBID) references metromind.Stations(id)
);