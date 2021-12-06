if not exists(select * from sys.databases where name = 'Task_6')
	CREATE DATABASE Task_6

GO

USE Task_6

if not exists (	select * 
				from dbo.STUDENTS as S 
				where S.GROUP_ID = 1)
	CREATE TABLE STUDENTS(
		STUDENT_ID INT PRIMARY KEY NOT NULL,
		GROUP_ID INT NOT NULL,
		FIRST_NAME NVARCHAR(50) NOT NULL,
		LAST_NAME NVARCHAR(50) NOT NULL
	);


if not exists (	select * 
				from dbo.GROUPS as G 
				where G.COURSE_ID = 2)
	CREATE TABLE GROUPS(
		GROUP_ID INT PRIMARY KEY NOT NULL,
		COURSE_ID INT NOT NULL,
		NAME NVARCHAR(50) NOT NULL
	);


if not exists (	select * 
				from dbo.COURSES)
	CREATE TABLE COURSES(
		COURSE_ID INT PRIMARY KEY NOT NULL,
		NAME NVARCHAR(50) NOT NULL,
		DESCRIPTION TEXT NOT NULL
	);

GO


