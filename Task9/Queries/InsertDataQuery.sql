USE [Task9DataBase]

INSERT INTO [dbo].[COURSES] 
			([COURSE_ID],[NAME],[DESCRIPTION])
	  VALUES(1, 'Linear Algebra', 'Studying Linear Algebra.'),
			(2, 'C# Programming', 'Studying C#'),
			(3, 'Hardware', 'Studying Hardware')

INSERT INTO [dbo].[GROUPS]
			([GROUP_ID],[COURSE_ID],[NAME])
	  VALUES(1, 1, 'Linear Algebra First Group'),
			(2, 1, 'Linear Algebra Second Group'),
			(3, 2, 'C# Programming First Group'),
			(4, 2, 'C# Programmin Second Group'),
			(5, 3, 'Hardware First Group'),
			(6, 3, 'SR-01')

INSERT INTO [dbo].[STUDENTS]
			([STUDENT_ID],[GROUP_ID],[FIRST_NAME],[LAST_NAME])
	  VALUES(1, 1, 'Isabell', 'Farrington'),
			(2, 1, 'Habib', 'Easton'),
			(3, 1, 'Damian', 'Fraser'),
			(4, 1, 'Sasha', 'Phillips'),
			(5, 1, 'Gene', 'Dyer'),
			(6, 1, 'Abu', 'Colon'),
			(7, 1, 'Nikhil', 'Barlow'),
			(8, 1, 'Jodi', 'Anthony'),
			(9, 1, 'Grant', 'Warner'),
			(10, 1, 'Maggie', 'Santos'),
			(11, 1, 'Darius', 'Mccartney'),

			(12, 2, 'Bethany', 'Conrad'),
			(13, 2, 'Aaliyah', 'Tillman'),
			(14, 2, 'Arjan', 'Byrne'),
			(15, 2, 'Olli', 'Haney'),
			(16, 2, 'Woody', 'Young'),
			(17, 2, 'Konnor', 'Valencia'),
			(18, 2, 'Marwa', 'Macdonald'),
			(19, 2, 'Amelia', 'Weiss'),
			(20, 2, 'Kaiden', 'Greenwood'),
			(21, 2, 'Aniqa', 'Rooney'),
			(22, 2, 'Awais', 'Deleon'),

			(23, 3, 'Jude', 'Ryder'),
			(24, 3, 'Anushka', 'Gomez'),
			(25, 3, 'Jayde', 'Acevedo'),

			(26, 4, 'Jacqueline', 'Driscoll'),
			(27, 4, 'Lochlan', 'Acevedo'),
			
			(28, 5, 'Jacqueline', 'Ellwood'),
			(29, 5, 'Lochlan', 'Ryder'),
			(30, 5, 'Greta', 'Wang'),
			(31, 5, 'Jude', 'Emerson'),
			(32, 5, 'Husna', 'Clayton'),
			(33, 5, 'Anushka', 'Silva'),
			(34, 5, 'Collette', 'Rosas'),
			(35, 5, 'Sion', 'Driscoll'),
			(36, 5, 'Jude', 'Goodwin'),
			(37, 5, 'Allen', 'Carver'),
			(38, 5, 'Lilian', 'Ryder'),
			(39, 5, 'Lily-Rose', 'Wickens'),

			(40, 6, 'Florrie', 'Wills'),
			(41, 6, 'Marek', 'Brady')
			
GO




