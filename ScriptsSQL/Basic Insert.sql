use dbTodaHora

select * from Sexo
select * from Pessoa
select * from Usuario

insert into sexo values('Masculino', 1);
insert into sexo values('Feminino', 1);
insert into sexo values('Prefiro não declarar', 1);

insert into Pessoa values('Igor Henrique', 'Salvador', '21-06-2022', '39864297864', 1, '11980131219');

insert into Usuario values('igorsalvador0621@gmail.com', 'SWI3MjQ4MTk1MCE=', 'igorsalvador0621@gmail.com', 1, 1, GETDATE(), 1, GETDATE(), 'Igor Henrique Salvador', 1);