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