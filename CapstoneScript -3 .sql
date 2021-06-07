 --CREATE DATABASE CapstoneCustomerRelationsDB;
 --USE CapstoneCustomerRelationsDB;
USE CapstoneCustomerRelationsSystem;
DROP TABLE CapstoneInfo;

CREATE TABLE CapstoneInfo (
	CapstoneStoreName VARCHAR(25) NOT NULL,
	CapstoneSunday VARCHAR(17) NOT NULL,
	CapstoneMonday VARCHAR(17) NOT NULL,
	CapstoneTuesday VARCHAR(17) NOT NULL,
	CapstoneWednesday VARCHAR(17) NOT NULL,
	CapstoneThursday VARCHAR(17) NOT NULL,
	CapstoneFriday VARCHAR(17) NOT NULL,
	CapstoneSaturday VARCHAR(17) NOT NULL,
	CapstoneHoliday VARCHAR(17) NOT NULL,
	PhoneNumber VARCHAR(50) NOT NULL,
	Email VARCHAR(50) NOT NULL,
	Address VARCHAR(100) NOT NULL,
	AddressLink VARCHAR(200) NOT NULL,
	CONSTRAINT PK_CapstoneInfo
		PRIMARY KEY (CapstoneStoreName)
);
GO

DROP PROCEDURE GetCapstoneInfo;
DROP PROCEDURE AddCapstoneInfo;
DROP PROCEDURE ModifyCapstoneInfo;

GO

CREATE PROCEDURE GetCapstoneInfo (
	@CapstoneStoreName VARCHAR(50) NULL
)
AS
	DECLARE @ReturnCode INT;
	SET @ReturnCode = 1;

	IF @CapstoneStoreName IS NULL
		RAISERROR('GetCapstoneInfo - Required parameter: @GetCapstoneInfo', 16,1);
	ELSE
		BEGIN
			SELECT 
				CapstoneSunday,
				CapstoneMonday,
				CapstoneTuesday,
				CapstoneWednesday,
				CapstoneThursday,
				CapstoneFriday,
				CapstoneSaturday,
				CapstoneHoliday,
				PhoneNumber,
				Email,
				Address,
				AddressLink
			FROM CapstoneInfo
			WHERE CapstoneStoreName = @CapstoneStoreName

            IF @@ERROR = 0
                SET @ReturnCode = 0;
            ELSE
                RAISERROR('GetCapstoneInfo - SELECT error: CapstoneInfo', 16, 1);
		END
RETURN @ReturnCode
GO

CREATE PROCEDURE AddCapstoneInfo (
	@CapstoneStoreName VARCHAR(50) NULL,
	@CapstoneSunday VARCHAR(17) NULL,
	@CapstoneMonday VARCHAR(17) NULL,
	@CapstoneTuesday VARCHAR(17) NULL,
	@CapstoneWednesday VARCHAR(17) NULL,
	@CapstoneThursday VARCHAR(17) NULL,
	@CapstoneFriday VARCHAR(17) NULL,
	@CapstoneSaturday VARCHAR(17) NULL,
	@CapstoneHoliday VARCHAR(17) NULL,
	@PhoneNumber VARCHAR(50) NULL,
	@Email VARCHAR(50) NULL,
	@Address VARCHAR(100) NULL,
	@AddressLink VARCHAR(200) NULL
)
AS
	DECLARE @ReturnCode INT;
	SET @ReturnCode = 1;

	IF @CapstoneStoreName IS NULL
		RAISERROR('AddCapstoneInfo - Required parameter: @CapstoneStoreName', 16,1);
	ELSE
		IF @CapstoneMonday IS NULL
			RAISERROR('AddCapstoneInfo - Required parameter: @CapstoneMonday',16,1)
		ELSE
			IF @CapstoneTuesday IS NULL
				RAISERROR('AddCapstoneInfo - Required parameter: @CapstoneTuesday',16,1)
			ELSE
				IF @CapstoneWednesday IS NULL
					RAISERROR('AddCapstoneInfo - Required parameter: @CapstoneWednesday',16,1)
				ELSE				
					IF @CapstoneThursday IS NULL
						RAISERROR('AddCapstoneInfo - Required parameter: @CapstoneThursday',16,1)
					ELSE				
						IF @CapstoneFriday IS NULL
							RAISERROR('AddCapstoneInfo - Required parameter: @CapstoneFriday',16,1)
						ELSE				
							IF @CapstoneSaturday IS NULL
								RAISERROR('AddCapstoneInfo - Required parameter: @CapstoneSaturday',16,1)
							ELSE				
								IF @CapstoneHoliday IS NULL
									RAISERROR('AddCapstoneInfo - Required parameter: @CapstoneHoliday',16,1)
								ELSE				
									IF @PhoneNumber IS NULL
										RAISERROR('AddCapstoneInfo - Required parameter: @PhoneNumber',16,1)
									ELSE				
										IF @Email IS NULL
											RAISERROR('AddCapstoneInfo - Required parameter: @Email',16,1)
										ELSE				
											IF @Address IS NULL
												RAISERROR('AddCapstoneInfo - Required parameter: @Address',16,1)
											ELSE				
												IF @AddressLink IS NULL
													RAISERROR('AddCapstoneInfo - Required parameter: @AddressLink',16,1)
												ELSE
													BEGIN
														INSERT INTO CapstoneInfo (
															CapstoneStoreName,
															CapstoneSunday,
															CapstoneMonday,
															CapstoneTuesday,
															CapstoneWednesday,
															CapstoneThursday,
															CapstoneFriday,
															CapstoneSaturday,
															CapstoneHoliday,
															PhoneNumber,
															Email,
															Address,
															AddressLink)
														VALUES (
															@CapstoneStoreName,
															@CapstoneSunday,
															@CapstoneMonday,
															@CapstoneTuesday,
															@CapstoneWednesday,
															@CapstoneThursday,
															@CapstoneFriday,
															@CapstoneSaturday,
															@CapstoneHoliday,
															@PhoneNumber,
															@Email,
															@Address,
															@AddressLink)

														IF @@ERROR = 0
															SET @ReturnCode = 0;
														ELSE
															RAISERROR('GetCapstoneInfo - SELECT error: CapstoneInfo', 16, 1);
													END						

RETURN @ReturnCode
GO

CREATE PROCEDURE ModifyCapstoneInfo (
	@CapstoneStoreName VARCHAR(50) NULL,
	@CapstoneSunday VARCHAR(17) NULL,
	@CapstoneMonday VARCHAR(17) NULL,
	@CapstoneTuesday VARCHAR(17) NULL,
	@CapstoneWednesday VARCHAR(17) NULL,
	@CapstoneThursday VARCHAR(17) NULL,
	@CapstoneFriday VARCHAR(17) NULL,
	@CapstoneSaturday VARCHAR(17) NULL,
	@CapstoneHoliday VARCHAR(17) NULL,
	@PhoneNumber VARCHAR(50) NULL,
	@Email VARCHAR(50) NULL,
	@Address VARCHAR(100) NULL,
	@AddressLink VARCHAR(200) NULL
)
AS
	DECLARE @ReturnCode INT;
	SET @ReturnCode = 1;
	
	IF @CapstoneStoreName IS NULL
		RAISERROR('AddCapstoneInfo - Required parameter: @CapstoneStoreName', 16,1);
	ELSE
		IF @CapstoneMonday IS NULL
			RAISERROR('ModifyCapstoneInfo - Required parameter: @CapstoneMonday',16,1)
		ELSE
			IF @CapstoneTuesday IS NULL
				RAISERROR('ModifyCapstoneInfo - Required parameter: @CapstoneTuesday',16,1)
			ELSE
				IF @CapstoneWednesday IS NULL
					RAISERROR('ModifyCapstoneInfo - Required parameter: @CapstoneWednesday',16,1)
				ELSE				
					IF @CapstoneThursday IS NULL
						RAISERROR('ModifyCapstoneInfo - Required parameter: @CapstoneThursday',16,1)
					ELSE				
						IF @CapstoneFriday IS NULL
							RAISERROR('ModifyCapstoneInfo - Required parameter: @CapstoneFriday',16,1)
						ELSE				
							IF @CapstoneSaturday IS NULL
								RAISERROR('ModifyCapstoneInfo - Required parameter: @CapstoneSaturday',16,1)
							ELSE				
								IF @CapstoneHoliday IS NULL
									RAISERROR('ModifyCapstoneInfo - Required parameter: @CapstoneHoliday',16,1)
								ELSE				
									IF @PhoneNumber IS NULL
										RAISERROR('ModifyCapstoneInfo - Required parameter: @PhoneNumber',16,1)
									ELSE				
										IF @Email IS NULL
											RAISERROR('ModifyCapstoneInfo - Required parameter: @Email',16,1)
										ELSE		
												IF @Address IS NULL
													RAISERROR('ModifyCapstoneInfo - Required parameter: @Address',16,1)
												ELSE				
													IF @AddressLink IS NULL
														RAISERROR('ModifyCapstoneInfo - Required parameter: @AddressLink',16,1)
													ELSE
														BEGIN
															UPDATE CapstoneInfo
															SET CapstoneSunday = @CapstoneSunday,
																CapstoneMonday = @CapstoneMonday,
																CapstoneTuesday = @CapstoneTuesday,
																CapstoneWednesday = @CapstoneWednesday,
																CapstoneThursday = @CapstoneThursday,
																CapstoneFriday = @CapstoneFriday,
																CapstoneSaturday = @CapstoneSaturday,
																CapstoneHoliday = @CapstoneHoliday,
																PhoneNumber = @PhoneNumber,
																Email = @Email,
																Address = @Address,
																AddressLink = @AddressLink
															WHERE CapstoneStoreName = @CapstoneStoreName
															IF @@ERROR = 0
																SET @ReturnCode = 0;
															ELSE
																RAISERROR('GetCapstoneInfo - SELECT error: CapstoneInfo', 16, 1);
														END
RETURN @ReturnCode
GO

SELECT * FROM CapstoneInfo
--Select * From CapstoneInfo;
--DELETE FROM CapstoneInfo;
----Capstone 1
--EXECUTE GetCapstoneInfo 'Capstone I';
----Capstone 2
--EXECUTE GetCapstoneInfo 'Capstone II'
----Capstone 3
--EXECUTE GetCapstoneInfo 'Capstone III'

--Capstone 1
EXECUTE AddCapstoneInfo 
						'Capstone I',				--Store
						'11:00am - 5:00pm',		--Sunday
						'11:00am - 8:00pm',		--Monday
						'11:00am - 8:00pm',		--Tuesday
						'11:00am - 8:00pm',		--Wednesday
						'11:00am - 8:00pm',		--Thursday
						'11:00am - 8:00pm',		--Friday
						'11:00am - 5:00pm',		--Saturday
						'Hours may vary',		--Holidays
						'(780) 471-6248',		--Phone
						'Capstone1@Capstone.ca',		--Email
						'10504 Princess Elizabeth Ave NW building w',		--Address
						'https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d1408.8524039294252!2d-113.5017878386252!3d53.568515416116554!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x53a023a4f87763b5%3A0x41d622a4700403c0!2sNAIT%20HP%20Centre%20for%20Information%20and%20Communications%20Technology!5e0!3m2!1sen!2sca!4v1623032622333!5m2!1sen!2sca';		--AddressLink
--Capstone 2
EXECUTE AddCapstoneInfo 
						'Capstone II',				--Store
						'11:00am - 5:00pm',		--Sunday
						'11:00am - 8:00pm',		--Monday
						'11:00am - 8:00pm',		--Tuesday
						'11:00am - 8:00pm',		--Wednesday
						'11:00am - 8:00pm',		--Thursday
						'11:00am - 8:00pm',		--Friday
						'11:00am - 5:00pm',		--Saturday
						'Hours may vary',		--Holidays
						'(780) 471-6248',		--Phone
						'Capstone2@Capstone.ca',		--Email
						'10504 Princess Elizabeth Ave NW building w',		--Address
						'https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d1408.8524039294252!2d-113.5017878386252!3d53.568515416116554!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x53a023a4f87763b5%3A0x41d622a4700403c0!2sNAIT%20HP%20Centre%20for%20Information%20and%20Communications%20Technology!5e0!3m2!1sen!2sca!4v1623032622333!5m2!1sen!2sca';		--AddressLink
--Capstone 3
EXECUTE AddCapstoneInfo 
						'Capstone III',			--Store
						'12:00am - 5:00pm',		--Sunday
						'12:00am - 6:00pm',		--Monday
						'12:00am - 6:00pm',		--Tuesday
						'12:00am - 6:00pm',		--Wednesday
						'12:00am - 7:00pm',		--Thursday
						'12:00am - 7:00pm',		--Friday
						'12:00am - 5:00pm',		--Saturday
						'12:00am - 5:00pm',		--Holidays
						'(780) 471-6248',		--Phone
						'Capstone3@Capstone.ca',		--Email
						'10504 Princess Elizabeth Ave NW building w',		--Address
						'https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d1408.8524039294252!2d-113.5017878386252!3d53.568515416116554!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x53a023a4f87763b5%3A0x41d622a4700403c0!2sNAIT%20HP%20Centre%20for%20Information%20and%20Communications%20Technology!5e0!3m2!1sen!2sca!4v1623032622333!5m2!1sen!2sca';		--AddressLink

----Capstone 1
--EXECUTE ModifyCapstoneInfo
--						'11:00am - 5:00pm',		--Sunday
--						'11:00am - 8:00pm',		--Monday
--						'11:00am - 8:00pm',		--Tuesday
--						'11:00am - 8:00pm',		--Wednesday
--						'11:00am - 8:00pm',		--Thursday
--						'11:00am - 8:00pm',		--Friday
--						'11:00am - 5:00pm',		--Saturday
--						'Hours may vary',		--Holidays
--						'(780) 433-7119',		--Phone
--						'Capstone1@Capstone.ca',		--Email
--						'',
--						'';

----Capstone 2
--EXECUTE ModifyCapstoneInfo
--						'11:00am - 5:00pm',		--Sunday
--						'11:00am - 8:00pm',		--Monday
--						'11:00am - 8:00pm',		--Tuesday
--						'11:00am - 8:00pm',		--Wednesday
--						'11:00am - 8:00pm',		--Thursday
--						'11:00am - 8:00pm',		--Friday
--						'11:00am - 5:00pm',		--Saturday
--						'Hours may vary',		--Holidays
--						'(780) 478-7767',		--Phone
--						'Capstone2@Capstone.ca';		--Email

----Capstone 3
--EXECUTE ModifyCapstoneInfo
--						'12:00am - 5:00pm',		--Sunday
--						'12:00am - 6:00pm',		--Monday
--						'12:00am - 6:00pm',		--Tuesday
--						'12:00am - 6:00pm',		--Wednesday
--						'12:00am - 7:00pm',		--Thursday
--						'12:00am - 7:00pm',		--Friday
--						'12:00am - 5:00pm',		--Saturday
--						'12:00am - 5:00pm',		--Holidays
--						'(780) 462-5767',		--Phone
--						'Capstone3@Capstone.ca';		--Email