create table metromind.Ways
(
    id int identity(0, 1),
    departureID int not null,
    stopID int not null,
    lineID int not null,
    Tmoy time,

    constraint PK_ways primary key(id),
    constraint FK_stations_lines foreign key(lineID) references metromind.Lines(id),
    constraint FK_stations_stations foreign key(departureID) references metromind.Stations(id),
    constraint FK_stations_stations foreign key(stopID) references metromind.Stations(id)
)

