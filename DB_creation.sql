CREATE DATABASE JOBPORTAL;

USE JOBPORTAL;

CREATE TABLE CONTACT(
	CONTACTID INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	NAME VARCHAR(50),
	EMAIL VARCHAR(50),
	SUBJECT VARCHAR(100),
	MESSAGE VARCHAR(MAX)
);

SELECT * FROM CONTACT;

USE JobPortal;

CREATE TABLE Country(
	CountryId INT PRIMARY KEY IDENTITY(1,1),
	CountryName VARCHAR(50)
)

SELECT * FROM Country;

INSERT INTO Country VALUES ('Vietnam');
INSERT INTO Country VALUES 
('France'),
('Brazil'),
('Canada'),
('South Africa'),
('Thailand'),
('Russia'),
('Mexico'),
('Italy'),
('Spain'),
('Japan');

CREATE TABLE [User](
	UserId INT PRIMARY KEY IDENTITY(1,1),
	Username VARCHAR(50),
	Password VARCHAR(50),
	Name VARCHAR(50),
	Email VARCHAR(50),
	Mobile VARCHAR(50),
	TenthGrade VARCHAR(50),
	TwelfthGrade VARCHAR(50),
	GraduationGrade VARCHAR(50),
	PostGraduationGrade VARCHAR(50),
	Phd VARCHAR(50),
	WorksOn VARCHAR(50),
	Experience VARCHAR(50),
	Resume VARCHAR(50),
	Address VARCHAR(MAX),
	Country VARCHAR(50)
)
ALTER TABLE [User] ALTER COLUMN Resume NVARCHAR(200);

ALTER TABLE [User] ADD UNIQUE (Username);

SELECT * FROM [User];

DELETE FROM [User];

-------Part 4----
CREATE TABLE Jobs(
	JobId INT PRIMARY KEY IDENTITY(1,1),
	Title VARCHAR(50),
	NoOfPost INT,
	Description VARCHAR(MAX),
	Qualification VARCHAR(50),
	Experience VARCHAR(50),
	Specialization VARCHAR(50),
	LastDateToApply DATE,
	Salary VARCHAR(50),
	JobType VARCHAR(50),
	CompanyName VARCHAR(200),
	CompanyImage VARCHAR(500),
	Website VARCHAR(100),
	Email VARCHAR(50),
	Address VARCHAR(MAX),
	Country VARCHAR(50),
	State VARCHAR(50),
	CreateDate DATETIME
)
