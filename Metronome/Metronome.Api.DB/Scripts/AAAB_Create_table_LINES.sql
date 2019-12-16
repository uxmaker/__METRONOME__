create table metromind.Lines
(
    id int not null,
    api_id nvarchar(64) not null,
    [name] nvarchar(255) not null,
    color nvarchar(8) not null,
    opening_time DATETIME2 not null,
    closing_time datetime2 not null,
    code nvarchar(32) not null,


    constraint PF_lines primary key(id),
    constraint UK_lines_Name unique([name]),
    constraint UK_lines unique(api_id)
);