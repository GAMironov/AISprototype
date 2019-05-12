create database People

use People

create table Persons
(
ID int IDENTITY(1001,1) not null constraint PK_People primary key,
Name nvarchar(30),
Family nvarchar(30),
)

create table Contacts
(
ID int  IDENTITY(1,1) not null constraint PK_Contacts primary key,
PersonID int not null,
Phone nvarchar(30),
Email nvarchar(30),
)

ALTER TABLE Contacts ADD  CONSTRAINT FK_Contacts_PersonID FOREIGN KEY(PersonID)
REFERENCES Persons (ID)

GO
CREATE PROCEDURE GetAll AS
BEGIN
    select p.ID, p.Name, p.Family, cnts.phone, cnts.email
from Persons as p
left join Contacts as cnts on cnts.PersonID=p.ID
END;

Go
CREATE PROCEDURE GetItem
	@family nvarchar(30)
AS
BEGIN
    select p.ID, p.Name, p.Family, cnts.phone, cnts.email
	from Persons as p
	left join Contacts as cnts on cnts.PersonID=p.ID where p.Family = @family;
END;

go
CREATE PROCEDURE AddItem
	@name nvarchar(30),
	@family nvarchar(30),
	@phone nvarchar(30) = null,
	@email nvarchar(30) = null
AS
BEGIN
    insert Persons (Name, Family) values (@name, @family);
	insert Contacts (PersonID) select top 1 p.ID from Persons as p ORDER BY p.ID DESC;
	update Contacts set phone = @phone, Email = @email where PersonID = (select top 1 p.ID from Persons as p where p.Family like @family ORDER BY p.ID DESC)
END;

go
CREATE PROCEDURE DeleteItem 
	@ID int
AS
BEGIN
    delete from Contacts where PersonID = @Id;
	delete from Persons where Id = @id;
END;

go
CREATE PROCEDURE UpdateItem 
	@ID int,
	@name nvarchar(30) = null,
	@family nvarchar(30) = null,
	@phone nvarchar(30) = null,
	@email nvarchar(30) = null
AS
BEGIN
    update Persons set name=@name, Family=@family where id=@id;
	update Contacts set phone=@phone, Email=@email where PersonID=@id;
END;

INSERT Persons(Name,Family) VALUES
('Иван', 'Иванов'),
('Петр', 'Петров'),
('Олег', 'Олегов'),
('Антон', 'Антонов')

Insert Contacts(PersonID) 
(
select ID from Persons
)

update Contacts set phone = '01234567890', Email = 'test@test.test' where PersonID = (select top 1 p.ID from Persons as p where p.Family like 'Иванов' ORDER BY p.ID DESC)
update Contacts set phone = '01234567890', Email = 'test@test.test' where PersonID = (select top 1 p.ID from Persons as p where p.Family like 'Петров' ORDER BY p.ID DESC)
update Contacts set phone = '01234567890', Email = 'test@test.test' where PersonID = (select top 1 p.ID from Persons as p where p.Family like 'Олегов' ORDER BY p.ID DESC)
update Contacts set phone = '01234567890', Email = 'test@test.test' where PersonID = (select top 1 p.ID from Persons as p where p.Family like 'Антонов' ORDER BY p.ID DESC)


