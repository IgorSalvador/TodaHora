create database dbTodaHora
go
use dbTodaHora
go
create table Sexo
(
	Sexo_Id int identity(1,1) primary key,
	Descricao varchar(50),
	blnAtivo bit
);

create table Pessoa
(
	Pessoa_Id int identity(1,1) primary key,
	Nome varchar(100),
	Sobrenome varchar(100),
	DataNascimento datetime,
	Cpf varchar(11),
	Sexo_Id int,
	Telefone varchar(11),
	constraint fk_Pessoa_Sexo foreign key (Sexo_Id) references Sexo (Sexo_Id)
);

create table Usuario
(
	Usuario_Id int identity(1,1) primary key,
	Username varchar(100),
	Senha varchar(200),
	Email varchar(100),
	blnAtivo bit,
	blnAdmin bit,
	DataAlteracao datetime,
	Pessoa_Id int,
	Created_On datetime,
	Created_By_UserName varchar(100),
	Created_By_Id int,
	constraint fk_Usuario_Pessoa foreign key (Pessoa_Id) references Pessoa (Pessoa_Id)
);