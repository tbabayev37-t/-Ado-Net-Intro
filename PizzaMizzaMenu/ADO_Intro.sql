create database PizzaMizzaMenu
use PizzaMizzaMenu
create table Products(
Id int primary key identity,
Name nvarchar(50) not null,
Price decimal(18,2) check(Price>5)
);
insert into Products
values('Texas',11);
select *from Products