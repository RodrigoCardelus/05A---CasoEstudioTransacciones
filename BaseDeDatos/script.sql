--creo la base de datos de ventas
use master
go

if exists(Select * FROM SysDataBases WHERE name='TransaccionesLogicas')
BEGIN
	DROP DATABASE TransaccionesLogicas
END
go

CREATE DATABASE TransaccionesLogicas
go

--pone en uso la bd
USE TransaccionesLogicas
go

---------------------------------------------TABLAS-----------------------------------------------------------------
--creo la tabla de los clientes
Create Table Clientes(
	CodCli int Primary Key Identity,
	NomCli varchar(30) not null,
	DirCli varchar(20)
)
go

--creo la tabla de telefonos
Create Table Telefonos(
	CodCli int not null Foreign Key References Clientes(CodCli),
	NumTel varchar(12) not null,
	Primary Key (CodCli, NumTel)
)
go

-----------------------------------------------SP------------------------------------------------------------------

Create Procedure AltaCliente @NomCli varchar(30), @DirCli varchar(20) AS
Begin
		If (Exists(select * from Clientes where NomCli = @NomCli))
			return -1
		
		Insert Clientes (NomCli, DirCli) Values (@NomCli, @DirCli)
		
		If @@ERROR = 0
			return @@Identity
		else
			return 0
End
go

Create Proc ModificarCliente @cod int, @Nom varchar(30), @Dir varchar(20) AS
Begin
	If Not (Exists(select * from Clientes where CodCli = @cod))
			return -1
	
	update Clientes
	set NomCli= @Nom, DirCli=@Dir
	where CodCli = @cod
End
go 

Create Procedure AltaTelefono @CodCli int, @NumTel varchar(12) As
Begin
		If Not (Exists(select * from Clientes where CodCli = @CodCli))
			return -1

        if exists(Select * from Telefonos
                  where CodCli = @CodCli AND NumTel= @NumTel)
           return -2
        
		Insert Telefonos (CodCli, NumTel) Values (@CodCli, @NumTel)
		
		If @@ERROR = 0
			return 1
		else
			return 0

End
go


Create Procedure ListoClientes As
Begin
	Select * From Clientes 
End
go


Create Procedure BuscoCliente @CodCLi int As
Begin
	Select * From Clientes Where CodCli = @CodCLi
End
go


Create Procedure TelefonoDeUnCliente @CodCLi int As
Begin
	Select * From Telefonos Where CodCli = @CodCLi
End
go


Create Procedure EliminoTelsDeCliente  @CodCLi int As
Begin
	Delete From Telefonos Where CodCli = @CodCLi
End
go


--------------------------------Datos de Prueba´----------------------------------------------------------
Insert Clientes (NomCli, DirCli) Values ('Andrea Muñoz','Salto')
Insert Clientes (NomCli, DirCli) Values ('Juan Lopez','Paysandu')
Insert Clientes (NomCli, DirCli) Values ('Carolina Vazques','Pueblo Blanco')
go

Insert Telefonos (CodCli, NumTel) Values (1, '099554422')
Insert Telefonos (CodCli, NumTel) Values (1, '44225689')
go

Insert Telefonos (CodCli, NumTel) Values (2, '099112255')
Insert Telefonos (CodCli, NumTel) Values (2, '47562314')
Insert Telefonos (CodCli, NumTel) Values (2, '27072233')
Insert Telefonos (CodCli, NumTel) Values (2, '097656467')
go

Insert Telefonos (CodCli, NumTel) Values (3, '095778899')
go

