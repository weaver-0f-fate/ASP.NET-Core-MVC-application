USE [Task9DataBase]

INSERT INTO [dbo].[COURSES] 
			([CourseName],[CourseDescription])
	  VALUES('Linear Algebra', 'Studying Linear Algebra.'),
			('C# Programming', 'Studying C#'),
			('Hardware', 'Studying Hardware')

INSERT INTO [dbo].[GROUPS]
			([CourseId],[GroupName])
	  VALUES(1, 'Linear Algebra First Group'),
			(1, 'Linear Algebra Second Group'),
			(2, 'C# Programming First Group'),
			(2, 'C# Programmin Second Group'),
			(3, 'Hardware First Group'),
			(3, 'SR-01')

INSERT INTO [dbo].[STUDENTS]
			([GroupId],[FirstName],[LastName])
	  VALUES(1, 'Isabell', 'Farrington'),
			(1, 'Habib', 'Easton'),
			(1, 'Damian', 'Fraser'),
			(1, 'Sasha', 'Phillips'),
			(1, 'Gene', 'Dyer'),
			(1, 'Abu', 'Colon'),
			(1, 'Nikhil', 'Barlow'),
			(1, 'Jodi', 'Anthony'),
			(1, 'Grant', 'Warner'),
			(1, 'Maggie', 'Santos'),
			(1, 'Darius', 'Mccartney'),

			(2, 'Bethany', 'Conrad'),
			(2, 'Aaliyah', 'Tillman'),
			(2, 'Arjan', 'Byrne'),
			(2, 'Olli', 'Haney'),
			(2, 'Woody', 'Young'),
			(2, 'Konnor', 'Valencia'),
			(2, 'Marwa', 'Macdonald'),
			(2, 'Amelia', 'Weiss'),
			(2, 'Kaiden', 'Greenwood'),
			(2, 'Aniqa', 'Rooney'),
			(2, 'Awais', 'Deleon'),

			(3, 'Jude', 'Ryder'),
			(3, 'Anushka', 'Gomez'),
			(3, 'Jayde', 'Acevedo'),

			(4, 'Jacqueline', 'Driscoll'),
			(4, 'Lochlan', 'Acevedo'),
			
			(5, 'Jacqueline', 'Ellwood'),
			(5, 'Lochlan', 'Ryder'),
			(5, 'Greta', 'Wang'),
			(5, 'Jude', 'Emerson'),
			(5, 'Husna', 'Clayton'),
			(5, 'Anushka', 'Silva'),
			(5, 'Collette', 'Rosas'),
			(5, 'Sion', 'Driscoll'),
			(5, 'Jude', 'Goodwin'),
			(5, 'Allen', 'Carver'),
			(5, 'Lilian', 'Ryder'),
			(5, 'Lily-Rose', 'Wickens'),

			(6, 'Florrie', 'Wills'),
			(6, 'Marek', 'Brady')
			
GO




