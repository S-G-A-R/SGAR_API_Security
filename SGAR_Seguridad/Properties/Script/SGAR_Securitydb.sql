CREATE DATABASE SGAR_SecurityDB;

USE SGAR_SecurityDB;

--Usuario
INSERT INTO Usuarios (Nombre, Apellido, Telefono, Email, DUI, Foto, Password, IdRol)
VALUES (
    'Marvin',                        
    'Antonio',                 
    '78901234',                    
    'Marvin@ejemplo.com',      
    '01234567-8',                  
    NULL,                          
    '12345',   
    4                              
);

--Roles 
--ID 1 
INSERT INTO Roles (NombreRol) VALUES ('Ciudadano');  
--ID 2
INSERT INTO Roles (NombreRol) VALUES ('Operador');   
--ID 3 
INSERT INTO Roles (NombreRol) VALUES ('Asociado');  
--ID 4
INSERT INTO Roles (NombreRol) VALUES ('Administrador');
--ID 5
INSERT INTO Roles (NombreRol) VALUES ('Organizacion');



-- 1.1. ROLES
CREATE TABLE Roles (
    Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    NombreRol VARCHAR(50) NOT NULL UNIQUE
);

-- 1.2. ORGANIZACION
CREATE TABLE Organizacion (
    Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    IdMunicipio INT NOT NULL, 
    NombreOrganizacion VARCHAR(250) NOT NULL,
    Telefono CHAR(9) NOT NULL,
    Email VARCHAR(255) UNIQUE,
    Password VARCHAR(255) NOT NULL,
    IdRol INT NOT NULL, 
    CONSTRAINT FK_Organizacion_Rol FOREIGN KEY (IdRol) REFERENCES Roles(Id)
);

-- 2.1. USUARIOS
CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    Nombre VARCHAR(120) NOT NULL,
    Apellido VARCHAR(120) NOT NULL,
    Telefono CHAR(9) UNIQUE NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    DUI VARCHAR(10) UNIQUE NOT NULL,  
    Foto VARBINARY(MAX) NULL,
    Password VARCHAR(255) NOT NULL, 
    IdRol INT NOT NULL, 
    CONSTRAINT FK_Usuario_Rol FOREIGN KEY (IdRol) REFERENCES Roles(Id)
);

-- 3.1. PUNTUACION
CREATE TABLE Puntuacion (
    Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    Puntos INT NOT NULL,
    NombreAnonimo VARCHAR(255) NULL,
    IdUser INT NULL,
    CONSTRAINT FK_Puntuacion_Usuario FOREIGN KEY (IdUser) REFERENCES Usuarios(Id),
);


-- 4.1. CIUDADANOS
CREATE TABLE Ciudadanos (
    Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    IdZona INT NOT NULL, -- Asumo FK a Zonas
    IdUser INT UNIQUE NOT NULL, 
    CONSTRAINT FK_Ciudadano_Usuario FOREIGN KEY (IdUser) REFERENCES Usuarios(Id)
);

-- 4.2. ADMINISTRADORES 
CREATE TABLE Administradores (
    Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    Codigo VARCHAR(25) UNIQUE NOT NULL,
    EmailLaboral VARCHAR(255) UNIQUE NOT NULL,
    TelefonoLaboral CHAR(9) UNIQUE NOT NULL,
    IdUser INT UNIQUE NOT NULL,
    CONSTRAINT FK_Administrador_Usuario FOREIGN KEY (IdUser) REFERENCES Usuarios(Id)
);

-- 4.3. OPERADORES
CREATE TABLE Operadores (
    Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    IdUser INT UNIQUE NOT NULL,
    CodigoOperador VARCHAR(20) UNIQUE NOT NULL,
    IdVehiculo INT NOT NULL, -- Asumo FK a Vehiculos
    LicenciaDoc VARBINARY(MAX) NULL,
    IdOrganizacion INT NOT NULL, -- Relación uno a muchos con Organizacion
    CONSTRAINT FK_Operador_Usuario FOREIGN KEY (IdUser) REFERENCES Usuarios(Id),
    CONSTRAINT FK_Operador_Organizacion FOREIGN KEY (IdOrganizacion) REFERENCES Organizacion(Id)
);

-- 4.4. SOLICITUDES DE OPERADOR
CREATE TABLE SolicitudesOperador (
    Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    IdCiudadano INT NOT NULL,
    IdOrganizacion INT NOT NULL,
    FechaSolicitud DATETIME NOT NULL,
    Estado VARCHAR(50) NOT NULL, 
    CONSTRAINT FK_Solicitud_Ciudadano FOREIGN KEY (IdCiudadano) REFERENCES Ciudadanos(Id),
    CONSTRAINT FK_Solicitud_Organizacion FOREIGN KEY (IdOrganizacion) REFERENCES Organizacion(Id)
);