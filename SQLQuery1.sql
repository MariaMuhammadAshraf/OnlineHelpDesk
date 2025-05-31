create database helpdesk;

use helpdesk;

CREATE TABLE USER_REGISTRATION(
USER_ID INT PRIMARY KEY IDENTITY(1,1),
FIRST_NAME VARCHAR(255) NOT NULL,
LAST_NAME VARCHAR(255) NOT NULL,
USER_NAME VARCHAR(255) NOT NULL,
USER_EMAIL VARCHAR(255) NOT NULL,
USER_PASSWORD VARCHAR(255) NOT NULL,
USER_ROLE VARCHAR(255)
);


CREATE TABLE ContactMessage(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Mobile NVARCHAR(20),
    Subject NVARCHAR(150),
    Message NVARCHAR(MAX),
    SubmittedAt DATETIME DEFAULT GETDATE()
);


select * from USER_REGISTRATION
select * from ContactMessage

drop table USER_REGISTRATION
drop table ContactMessage


insert into USER_REGISTRATION
values('admin','user','admin','admin786@gmail.com' , 'admin786', 'ADMIN');




CREATE TABLE Facilities
(
    FacilityId INT IDENTITY(1,1) PRIMARY KEY,
    FacilityName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255) NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    IsActive BIT NOT NULL DEFAULT 1
);

select * from Facilities;
drop table Facilities

CREATE TABLE aboutus (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Content NVARCHAR(MAX) NOT NULL,
    ImagePath NVARCHAR(255) NULL
);

select * from aboutus;
drop table aboutus

CREATE TABLE Reviews (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClientName NVARCHAR(255) NOT NULL,
    Profession NVARCHAR(255) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    Rating INT CHECK (Rating >= 1 AND Rating <= 5) NOT NULL,
    ImagePath NVARCHAR(255) NULL
);
select * from Reviews;
drop table Reviews












