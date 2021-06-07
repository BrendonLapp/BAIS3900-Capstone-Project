-------------------------------------
-- Create & Alter Tables
-- CREATE DATABASE CapstoneCustomerRelationsDB;
 --USE CapstoneCustomerRelationsDB;
-- Bottom one is for Dev 1 server
 USE CapstoneCustomerRelationsSystem;
-- GO
----------------
DROP TABLE UserAccountRoles;
DROP TABLE Role;
DROP TABLE NewsItem;
DROP TABLE Product;
DROP TABLE UserAccount;

CREATE TABLE UserAccount (
	UserAccountNumber INT IDENTITY (1,1) NOT NULL,
	FirstName    VARCHAR(25) NOT NULL,
    LastName     VARCHAR(25) NOT NULL,
    PhoneNumber  VARCHAR(50) NOT NULL,
    UserName VARCHAR(50) NOT NULL,
    Email VARCHAR(50) NOT NULL,
	DCINumber INT NULL, 
    Password VARCHAR(50) NOT NULL,
    Salt VARCHAR(50) NOT NULL,
	CONSTRAINT PK_UserAccount_UserAccountNumber PRIMARY KEY (UserAccountNumber),
	CONSTRAINT UNIQUE_UserName UNIQUE(UserName)
);
GO

CREATE TABLE Role 
(
    RoleNumber INT IDENTITY(1,1) NOT NULL,
    RoleName VARCHAR(50) NOT NULL,
    RoleDescription VARCHAR(100) NULL,
    CONSTRAINT PK_Role_RoleNumber
        PRIMARY KEY (RoleNumber)
)
GO

CREATE TABLE UserAccountRoles
(
    UserAccountNumber INT NOT NULL,
    RoleNumber INT NOT NULL,
    CONSTRAINT PK_UserAccountRoles
        PRIMARY KEY (UserAccountNumber, RoleNumber),
    CONSTRAINT FK_UserAccountRoles_UserAccount
        FOREIGN KEY (UserAccountNumber) REFERENCES UserAccount(UserAccountNumber),
    CONSTRAINT FK_UserAccountRoles_Role
        FOREIGN KEY (RoleNumber) REFERENCES Role (RoleNumber)
);
GO

CREATE TABLE NewsItem (
    NewsItemNumber INT IDENTITY(1,1) NOT NULL,
    ImageType VARCHAR(25) NOT NULL,
    ImageURL VARCHAR(2000) NOT NULL,
    NewsItemLink VARCHAR(2000) NOT NULL,
    OptionalNewsName VARCHAR(1000) NULL,
    OptionalNewsDescription VARCHAR(1000) NULL,
    OptionalNewsPrice VARCHAR (100) NULL
    CONSTRAINT PK_NewsItem
        PRIMARY KEY (NewsItemNumber)
);
GO

CREATE TABLE Product (
    ProductNumber INT IDENTITY(1,1) NOT NULL,
    ImageURL VARCHAR(2000) NOT NULL,
    --ProductLink VARCHAR(200) NOT NULL,
    CompanyName VARCHAR(50) NOT NULL,
    ProductName VARCHAR(1000) NOT NULL,
    ProductDescription VARCHAR(2000) NOT NULL,
    ProductPrice MONEY NOT NULL
    CONSTRAINT PK_Product
        PRIMARY KEY (ProductNumber)
);
GO

-------------------------------------
-- Stored Procedures
-------------------------------------
DROP PROCEDURE AddUserAccountAndAssignRole;
DROP PROCEDURE AddUserAccount;
DROP PROCEDURE GetUserAccount;
DROP PROCEDURE AssignRole;
DROP PROCEDURE UnAssignRole;
DROP PROCEDURE CreateRole;
DROP PROCEDURE GetRoles;
DROP PROCEDURE GetUserAccounts;
DROP PROCEDURE Login;
DROP PROCEDURE GetUserSalt;
DROP PROCEDURE GetUserAccountByUserName;
DROP PROCEDURE UpdateUserAccount;
DROP PROCEDURE UpdateUserAccountPassword;

DROP PROCEDURE GetNewsItems;
DROP PROCEDURE GetNewsItem;
DROP PROCEDURE AddNewsItem;
DROP PROCEDURE UpdateNewsItem;
DROP PROCEDURE DeleteNewsItem;

DROP PROCEDURE GetProducts;
DROP PROCEDURE GetProduct;
DROP PROCEDURE AddProduct;
DROP PROCEDURE UpdateProduct;
DROP PROCEDURE DeleteProduct;
GO

CREATE PROCEDURE UpdateUserAccountPassword(
	@Username VARCHAR(50) NULL,
	@Password VARCHAR(50) NULL,
	@Salt VARCHAR(50)NULL
)
AS
DECLARE @ReturnCode INT;
SET @ReturnCode = 1;

	IF @Username IS NULL
		RAISERROR('UpdateUserAccountPassword - Required parameter: @UserName', 16, 1);
	ELSE
		IF @Password IS NULL
			RAISERROR('UpdateUserAccountPassword - Required parameter: @Password', 16, 1);
		ELSE
			IF @Salt IS NULL
				RAISERROR('UpdateUserAccountPassword - Required parameter: @Salt', 16, 1);
			ELSE
			BEGIN
				-- Update the User Account table
				UPDATE UserAccount
				SET 
					Password = @Password,
					Salt = @Salt
				WHERE UserName = @Username

				IF @@ERROR = 0
					SET @ReturnCode = 0
				ELSE
					RAISERROR('UpdateUserAccountPassword - UPDATE error: UserAccount', 16, 1)
			END

RETURN @ReturnCode;
GO

CREATE PROCEDURE UpdateUserAccount(
    @FirstName VARCHAR(25) NULL,
    @LastName VARCHAR(25) NULL,
    @PhoneNumber VARCHAR(50) NULL,
    @UserName VARCHAR(50) = NULL,
	@PreviousUserName VARCHAR(50) = NULL,
    @Email VARCHAR(50) = NULL,
	@DCINumber INT NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 1

	IF @FirstName IS NULL
		RAISERROR('UpdateUserAccount - Required parameter: @FirstName', 16, 1);
	ELSE
		IF @LastName IS NULL
			RAISERROR('UpdateUserAccount - Required parameter: @LastName', 16, 1);
		ELSE
			IF @PhoneNumber IS NULL
				RAISERROR('UpdateUserAccount - Required parameter: @PhoneNumber', 16, 1);
			ELSE
				IF @PreviousUserName IS NULL
					RAISERROR('UpdateUserAccount - Required parameter: @PreviousUserName', 16, 1);
				ELSE
					IF @UserName IS NULL
						RAISERROR('UpdateUserAccount - Required parameter: @UserName', 16, 1);
					ELSE
						IF @Email IS NULL
							RAISERROR('UpdateUserAccount - Required parameter: @Email', 16, 1);
						ELSE
							BEGIN
								-- Update the User Account table
								UPDATE UserAccount
								SET 
									FirstName = @FirstName,
									LastName = @LastName,
									UserName = @UserName,
									Email = @Email,
									DCINumber = @DCINumber,
									PhoneNumber = @PhoneNumber
								WHERE UserName = @PreviousUserName

								IF @@ERROR = 0
									SET @ReturnCode = 0
								ELSE
									RAISERROR('UpdateUserAccount - UPDATE error: UserAccount', 16, 1)
							END

RETURN @ReturnCode;
GO

CREATE PROCEDURE AddUserAccount(
    @FirstName VARCHAR(25) NULL,
    @LastName VARCHAR(25) NULL,
    @PhoneNumber VARCHAR(50) NULL,
    @UserName VARCHAR(50) = NULL,
    @Email VARCHAR(50) = NULL,
	@DCINumber INT = NULL,
    @Password VARCHAR(1000) = NULL,
    @Salt VARCHAR(1000) = NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 1

    IF @FirstName IS NULL
        RAISERROR('AddUserAccount - Required parameter: @FirstName', 16, 1);
    ELSE
        IF @LastName IS NULL
            RAISERROR('AddUserAccount - Required parameter: @LastName', 16, 1);
        ELSE
            IF @PhoneNumber IS NULL
                RAISERROR('AddUserAccount - Required parameter: @PhoneNumber', 16, 1);
            ELSE
                IF @UserName IS NULL
                    RAISERROR('AddUserAccount - Required parameter: @UserName', 16, 1);
                ELSE
                    IF @Email IS NULL
                        RAISERROR('AddUserAccount - Required parameter: @Email', 16, 1);
                    ELSE
						IF @Password IS NULL
							RAISERROR('AddUserAccount - Required parameter: @Password', 16, 1);
						ELSE
							IF @Salt IS NULL
								RAISERROR('AddUserAccount - Required parameter: @Salt', 16, 1);
							ELSE
								BEGIN
									INSERT INTO UserAccount (
										FirstName,
										LastName,
										PhoneNumber,
										UserName,
										Email,
										DCINumber,
										Password,
										Salt)
									VALUES (
										@FirstName,
										@LastName,
										@PhoneNumber,
										@UserName,
										@Email,
										@DCINumber,
										@Password,
										@Salt)

									IF @@ERROR = 0
										SET @ReturnCode = 0
									ELSE
										RAISERROR('AddUserAccount - INSERT error: UserAccount', 16, 1)
								END
RETURN @ReturnCode
GO

CREATE PROCEDURE AddUserAccountAndAssignRole(
    @FirstName VARCHAR(25) NULL,
    @LastName VARCHAR(25) NULL,
    @PhoneNumber VARCHAR(50) NULL,
    @UserName VARCHAR(50) = NULL,
    @Email VARCHAR(50) = NULL,
	@DCINumber INT = NULL,
    @Password VARCHAR(1000) = NULL,
    @Salt VARCHAR(1000) = NULL,
	@RoleName VARCHAR(50) = NULL
)
AS
DECLARE @ReturnCode INT
DECLARE @UserAccountNumber INT
DECLARE @RoleNumber INT
SET @ReturnCode = 1

    IF @FirstName IS NULL
        RAISERROR('AddUserAccountAndAssignRole - Required parameter: @FirstName', 16, 1);
    ELSE
        IF @LastName IS NULL
            RAISERROR('AddUserAccountAndAssignRole - Required parameter: @LastName', 16, 1);
        ELSE
            IF @PhoneNumber IS NULL
                RAISERROR('AddUserAccountAndAssignRole - Required parameter: @PhoneNumber', 16, 1);
            ELSE
                IF @UserName IS NULL
                    RAISERROR('AddUserAccountAndAssignRole - Required parameter: @UserName', 16, 1);
                ELSE
                    IF @Email IS NULL
                        RAISERROR('AddUserAccountAndAssignRole - Required parameter: @Email', 16, 1);
                    ELSE
                        IF @Password IS NULL
                            RAISERROR('AddUserAccountAndAssignRole - Required parameter: @Password', 16, 1);
                        ELSE
                            IF @Salt IS NULL
                                RAISERROR('AddUserAccountAndAssignRole - Required parameter: @Salt', 16, 1);
                            ELSE
								IF @RoleName IS NULL
									RAISERROR('AddUserAccountAndAssignRole - Required parameter: @RoleName', 16, 1);
								ELSE
									BEGIN
									BEGIN TRANSACTION
									-- Add the User Account
										INSERT INTO UserAccount (
											FirstName,
											LastName,
											PhoneNumber,
											UserName,
											Email,
											DCINumber,
											Password,
											Salt)
										VALUES (
											@FirstName,
											@LastName,
											@PhoneNumber,
											@UserName,
											@Email,
											@DCINumber,
											@Password,
											@Salt)

										-- If that fails, rollback the transaction
                                        IF @@ERROR = 1
                                        BEGIN
                                            RAISERROR('AddUserAccountAndAssignRole - INSERT error: UserAccount', 16, 1);
                                            ROLLBACK TRANSACTION;
                                        END
										ELSE
											--Set the User Account Number
											SET @UserAccountNumber = (
												SELECT TOP 1
													UserAccountNumber 
												FROM UserAccount
												WHERE UserName = @UserName
											);

											--Set the Role Number
											SET @RoleNumber = (
												SELECT TOP 1
													RoleNumber
												FROM Role
												WHERE RoleName = @RoleName
											);

											IF @@ERROR = 1
											BEGIN
												RAISERROR('AddUserAccountAndAssignRole - SELECT error: UserAccount', 16, 1);
												ROLLBACK TRANSACTION;
											END
											ELSE
												--Assign The User Account the chosen Role
												INSERT INTO UserAccountRoles (
													UserAccountNumber,
													RoleNumber
												) VALUES (
													@UserAccountNumber,
													@RoleNumber
												);

												IF @@ERROR = 1
												BEGIN
													RAISERROR('AddUserAccountAndAssignRole - INSERT error: UserAccountRoles', 16, 1);
													ROLLBACK TRANSACTION;
												END
												ELSE
											
													IF @@ERROR = 0
													BEGIN
														SET @ReturnCode = 0;
														COMMIT TRANSACTION;
													END
													ELSE
													BEGIN
														RAISERROR('AddUserAccountAndAssignRole - INSERT error: UserAccount', 16, 1)
														ROLLBACK TRANSACTION;
													END
												END
RETURN @ReturnCode
GO

CREATE PROCEDURE GetUserAccount(
    @UserAccountNumber INT NULL
)
AS
    DECLARE @ReturnCode INT
    SET @ReturnCode = 1

    BEGIN
        SELECT
            UserAccountNumber,
            FirstName,
            LastName,
            PhoneNumber,
			DCINumber
        FROM
            UserAccount
        WHERE
            UserAccountNumber = @UserAccountNumber

        IF @@ERROR = 0
                SET @ReturnCode = 0
            ELSE
                RAISERROR('GetUserAccount - SELECT error: UserAccount', 16, 1);
    END
RETURN @ReturnCode
GO

CREATE PROCEDURE CreateRole (
    @RoleName VARCHAR(50) NULL,
    @RoleDescription VARCHAR(100) NULL
)
AS
    DECLARE @ReturnCode INT;
    SET @ReturnCode = 1;

    IF @RoleName IS NULL
        RAISERROR ('CreateRole - Required Parameter: @RoleName', 16, 1);
    ELSE
        BEGIN

        INSERT INTO Role (
            RoleName,
            RoleDescription)
        VALUES (
            @RoleName,
            @RoleDescription
        )

        IF @@ERROR = 0
            SET @ReturnCode = 0;
        ELSE
            RAISERROR ('CreateRole - INSERT error: Role', 16, 1);
        END

RETURN @ReturnCode;
GO

CREATE PROCEDURE GetRoles
AS
    DECLARE @ReturnCode INT;
    SET @ReturnCode = 1;

    BEGIN
        SELECT
            RoleNumber,
            RoleName,
            RoleDescription
        FROM Role

        IF @@ERROR = 0
            SET @ReturnCode = 0;
        ELSE
            RAISERROR ('GetRoles - SELECT Error: Role', 16, 1);
    END
RETURN @ReturnCode;
GO

CREATE PROCEDURE UnAssignRole (
	@UserAccountNumber INT NULL,
    @RoleNumber INT NULL
)
AS
	DECLARE @ReturnCode INT;
	SET @ReturnCode = 0;

	IF @UserAccountNumber IS NULL
        RAISERROR ('UnAssignRole - Required Parameter: @UserAccountNumber', 16, 1);
    ELSE
        IF @RoleNumber IS NULL
            RAISERROR ('UnAssignRole - Required Parameter: @RoleNumber', 16, 1);
        ELSE
		BEGIN 
		    DELETE
				FROM UserAccountRoles
            WHERE 
				UserAccountNumber = @UserAccountNumber AND
				RoleNumber = @RoleNumber;		

			IF @@ERROR = 0
				SET @ReturnCode = 0;
			ELSE
				RAISERROR ('UnAssignRole - DELETE error: UserAccountRoles', 16, 1);
		END

RETURN @ReturnCode;
GO


CREATE PROCEDURE AssignRole (
    @UserAccountNumber INT NULL,
    @RoleNumber INT NULL
)
AS
    DECLARE @ReturnCode INT;
    SET @ReturnCode = 0;

    IF @UserAccountNumber IS NULL
        RAISERROR ('AssignRole - Required Parameter: @UserAccountNumber', 16, 1);
    ELSE
        IF @RoleNumber IS NULL
            RAISERROR ('AssignRole - Required Parameter: @RoleNumber', 16, 1);
        ELSE
            BEGIN
                INSERT INTO UserAccountRoles (
                    UserAccountNumber,
                    RoleNumber
                )
                VALUES (
                    @UserAccountNumber,
                    @RoleNumber
                );

                IF @@ERROR = 0
                    SET @ReturnCode = 0;
                ELSE
                    RAISERROR ('AssignRole - INSERT error: UserAccountRoles', 16, 1);
            END
RETURN @ReturnCode;
GO

CREATE PROCEDURE GetUserAccounts
AS
    DECLARE @ReturnCode INT
    SET @ReturnCode = 1

    BEGIN

        SELECT 
            UserAccount.FirstName,
            UserAccount.LastName,
            UserAccount.UserName,
            UserAccount.PhoneNumber,
            UserAccount.Email,
			UserAccount.DCINumber,
            Role.RoleName,
            Role.RoleDescription,
            Role.RoleNumber,
            UserAccount.UserAccountNumber
            FROM UserAccountRoles
                INNER JOIN Role ON UserAccountRoles.RoleNumber = Role.RoleNumber
                INNER JOIN UserAccount ON UserAccountRoles.UserAccountNumber = UserAccount.UserAccountNumber;
        IF @@ERROR = 0
                SET @ReturnCode = 0
            ELSE
                RAISERROR('GetUserAccounts - SELECT error: UserAccount', 16, 1);

    END
RETURN @ReturnCode
GO

CREATE PROCEDURE GetUserAccountByUserName (
    @UserName VARCHAR(50) NULL
)
AS
    DECLARE @ReturnCode INT;
    SET @ReturnCode = 1;

    IF @UserName IS NULL
        RAISERROR('GetUserAccountByUserName - SELECT error: UserName', 16, 1);
    ELSE
        BEGIN

                SELECT 
                    UserAccount.FirstName,
                    UserAccount.LastName,
                    UserAccount.UserName,
                    UserAccount.PhoneNumber,
                    UserAccount.Email,
					UserAccount.DCINumber,
                    Role.RoleName,
                    Role.RoleDescription,
                    Role.RoleNumber,
                    UserAccount.UserAccountNumber
                FROM UserAccountRoles
                    INNER JOIN Role ON UserAccountRoles.RoleNumber = Role.RoleNumber
                    INNER JOIN UserAccount ON UserAccountRoles.UserAccountNumber = UserAccount.UserAccountNumber
                        WHERE UserAccount.UserName = @UserName;

            IF @@ERROR = 0
                SET @ReturnCode = 0;
            ELSE
                RAISERROR('GetUserAccountByUsername - SELECT error: UserAccount', 16, 1);

        END
RETURN @ReturnCode;
GO

CREATE PROCEDURE Login (
    @UserName VARCHAR(50) NULL,
    @Password VARCHAR(1000) NULL
)
AS
    DECLARE @ReturnCode INT;
    SET @ReturnCode = 1;

    BEGIN
        SELECT
            UserName,
            Password,
            Role.RoleName
        FROM UserAccount
            INNER JOIN UserAccountRoles ON UserAccount.UserAccountNumber = UserAccountRoles.UserAccountNumber 
            INNER JOIN Role ON UserAccountRoles.RoleNumber = Role.RoleNumber
                WHERE(
                    UserName = @UserName 
                    AND Password = @Password
                )

        IF @@ERROR = 0
            SET @ReturnCode = 0;
        ELSE
            RAISERROR('Login - SELECT error: UserAccount TABLE', 16, 1);
    End
RETURN @ReturnCode;
GO

CREATE PROCEDURE GetUserSalt (
    @UserName VARCHAR(50) NULL
)
AS
    DECLARE @ReturnCode INT;
    SET @ReturnCode = 1;
    IF @UserName IS NULL
        RAISERROR('GetUserSalt - Required parameter: @UserName', 16, 1);
    ELSE
        BEGIN
            SELECT 
                Salt 
            FROM UserAccount
                WHERE (UserName = @UserName);

            IF @@ERROR = 0
                SET @ReturnCode = 0;
            ELSE
                RAISERROR('GetUserSalt - SELECT error: UserAccount TABLE', 16, 1);
        END
RETURN @ReturnCode;
GO

CREATE PROCEDURE GetNewsItems
AS
    DECLARE @ReturnCode INT;
    SET @ReturnCode = 1;

    BEGIN

    SELECT
        NewsItemNumber,
        ImageType,
        ImageURL,
        NewsItemLink,
        --IndexPosition,
        OptionalNewsName,
        OptionalNewsDescription,
        OptionalNewsPrice
    FROM NewsItem

    IF @@ERROR = 0
        SET @ReturnCode = 0;
    ELSE
        RAISERROR('GetNewsItems - SELECT error: NewsItem TABLE', 16, 1);
    END

RETURN @ReturnCode;
GO

CREATE PROCEDURE GetNewsItem
(
    @NewsItemNumber INT NULL
)
AS
    DECLARE @ReturnCode INT;
    SET @ReturnCode = 1;

    IF @NewsItemNumber IS NULL
        RAISERROR ('GetNewsItem - SELECT error: NewsItem TABLE', 16, 1);
    ELSE

        BEGIN

        SELECT
            NewsItemNumber,
            ImageType,
            ImageURL,
            NewsItemLink,
            --IndexPosition,
            OptionalNewsName,
            OptionalNewsDescription,
            OptionalNewsPrice
        FROM NewsItem
            WHERE NewsItemNumber = @NewsItemNumber

        IF @@ERROR = 0
            SET @ReturnCode = 0;
        ELSE
            RAISERROR('GetNewsItem - SELECT error: NewsItem TABLE', 16, 1);
        END

RETURN @ReturnCode;
GO

CREATE PROCEDURE AddNewsItem (
    @ImageType VARCHAR(25) NULL,
    @ImageURL VARCHAR(2000) NULL,
    @NewsItemLink VARCHAR(2000) NULL,
    --@IndexPosition INT NULL,
    @OptionalNewsName VARCHAR(1000) NULL,
    @OptionalNewsDescription VARCHAR(1000) NULL,
    @OptionalNewsPrice VARCHAR (100) NULL
)
AS
    DECLARE @ReturnCode INT;
    SET @ReturnCode = 1;

    IF @ImageType IS NULL
        RAISERROR ('AddNewsItem - Required Parameter: @ImageType', 16, 1);
    ELSE
        IF @ImageURL IS NULL
            RAISERROR ('AddNewsItem - Required Parameter: @ImageURL', 16, 1);
        ELSE
            IF @NewsItemLink IS NULL
                RAISERROR ('AddNewsItem - Required Parameter: @NewsItemLink', 16, 1);
            ELSE
                --IF @IndexPosition IS NULL
                --    RAISERROR ('AddNewsItem - Required Parameter: @IndexPosition', 16, 1);
                --ELSE
                    BEGIN
                        INSERT INTO NewsItem (
                            ImageType,
                            ImageURL,
                            NewsItemLink,
                            --IndexPosition,
                            OptionalNewsName,
                            OptionalNewsDescription,
                            OptionalNewsPrice
                        ) VALUES (
                            @ImageType,
                            @ImageURL,
                            @NewsItemLink,
                            --@IndexPosition,
                            @OptionalNewsName,
                            @OptionalNewsDescription,
                            @OptionalNewsPrice
                        );

                        IF @@ERROR = 0
                            SET @ReturnCode = 0;
                        ELSE
                            RAISERROR('AddNewsItem - INSERT error: NewsItem TABLE', 16, 1);
                    END

RETURN @ReturnCode;
GO

CREATE PROCEDURE UpdateNewsItem (
    @NewsItemNumber INT NULL,
    @ImageType VARCHAR(25) NULL,
    @ImageURL VARCHAR(2000) NULL,
    @NewsItemLink VARCHAR(2000) NULL,
    --@IndexPosition INT NULL,
    @OptionalNewsName VARCHAR(1000) NULL,
    @OptionalNewsDescription VARCHAR(1000) NULL,
    @OptionalNewsPrice VARCHAR (100) NULL
)
AS
    DECLARE @ReturnCode INT;
    SET @ReturnCode = 1;

    IF @NewsItemNumber IS NULL
        RAISERROR ('UpdateNewsItem - Required Parameter: @NewsItemNumber', 16, 1);
    ELSE
        IF @ImageType IS NULL
            RAISERROR ('UpdateNewsItem - Required Parameter: @ImageType', 16, 1);
        ELSE
            IF @ImageURL IS NULL
                RAISERROR ('UpdateNewsItem - Required Parameter: @ImageURL', 16, 1);
            ELSE
                IF @NewsItemLink IS NULL
                    RAISERROR ('UpdateNewsItem - Required Parameter: @NewsItemLink', 16, 1);
                ELSE
                    --IF @IndexPosition IS NULL
                    --    RAISERROR ('UpdateNewsItem - Required Parameter: @IndexPosition', 16, 1);
                    --ELSE
                        BEGIN

                            -- Update the News Item table
                            UPDATE NewsItem
                                SET ImageType = @ImageType,
                                        ImageURL = @ImageURL,
                                        NewsItemLink = @NewsItemLink,
                                        --IndexPosition = @IndexPosition,
                                        OptionalNewsName = @OptionalNewsName,
                                        OptionalNewsDescription = @OptionalNewsDescription,
                                        OptionalNewsPrice = @OptionalNewsPrice
                                WHERE NewsItemNumber = @NewsItemNumber 

                            -- Check for errors
                            IF @@ERROR = 0
                                SET @ReturnCode = 0;
                            ELSE
                                RAISERROR('UpdateNewsItem - UPDATE error: NewsItem TABLE', 16, 1);
                        END

RETURN @ReturnCode;
GO

CREATE PROCEDURE DeleteNewsItem (
    @NewsItemNumber INT NULL
)
AS
    DECLARE @ReturnCode INT;
    SET @ReturnCode = 1;

    IF @NewsItemNumber IS NULL
        RAISERROR('DeleteNewsItem - Required Parameter: @NewsItemNumber',16, 1);
    ELSE

        DELETE
        FROM NewsItem
            WHERE NewsItemNumber = @NewsItemNumber;

        IF @@ERROR = 0
            SET @ReturnCode = 0;
        ELSE
            RAISERROR('DeleteNewsItem - DELETE error: NewsItem TABLE',16, 1);

RETURN @ReturnCode;
GO

CREATE PROCEDURE GetProducts
AS
    DECLARE @ReturnCode INT;
    SET @ReturnCode = 1;

    SELECT 
        ProductNumber,
        ImageURL,
        --ProductLink,
        CompanyName,
        ProductName,
        ProductDescription,
        ProductPrice
    FROM Product

    IF @@ERROR = 0
        SET @ReturnCode = 0;
    ELSE
        RAISERROR('GetProducts - SELECT error: Product TABLE', 16, 1);

RETURN @ReturnCode;
GO

CREATE PROCEDURE GetProduct (
    @ProductNumber INT NULL
)
AS
    DECLARE @ReturnCode INT;
    SET @ReturnCode = 1;

    IF @ProductNumber IS NULL
        RAISERROR('GetProduct - Required parameter - @ProductNumber', 16, 1);
    ELSE
    
        SELECT 
            ProductNumber,
            ImageURL,
            --ProductLink,
            CompanyName,
            ProductName,
            ProductDescription,
            ProductPrice
        FROM Product
        WHERE ProductNumber = @ProductNumber

        IF @@ERROR = 0
            SET @ReturnCode = 0;
        ELSE
            RAISERROR('GetProduct - SELECT error: Product TABLE', 16, 1);

RETURN @ReturnCode;
GO

CREATE PROCEDURE AddProduct (
    @ImageURL VARCHAR(2000) NULL,
    --@ProductLink VARCHAR(200) NULL,
    @CompanyName VARCHAR(50) NULL,
    @ProductName VARCHAR(1000) NULL,
    @ProductDescription VARCHAR(2000) NULL,
    @ProductPrice VARCHAR (100) NULL
)
AS
    DECLARE @ReturnCode INT;
    SET @ReturnCode = 1;

    IF @ImageURL IS NULL
        RAISERROR ('AddProduct - Required Parameter: @ImageURL', 16, 1);
    ELSE
        --IF @ProductLink IS NULL
        --    RAISERROR ('AddProduct - Required Parameter: @ProductLink', 16, 1);
        --ELSE
            IF @CompanyName IS NULL
                RAISERROR ('AddProduct - Required Parameter: @CompanyName', 16, 1);
            ELSE
                IF @ProductName IS NULL
                    RAISERROR ('AddProduct - Required Parameter: @ProductName', 16, 1);
                ELSE
                    IF @ProductDescription IS NULL
                        RAISERROR ('AddProduct - Required Parameter: @ProductDescription', 16, 1);
                    ELSE
                        IF @ProductPrice IS NULL
                            RAISERROR ('AddProduct - Required Parameter: @ProductPrice', 16, 1);
                        ELSE
                            BEGIN
                                INSERT INTO Product (
                                    ImageURL,
                                    --ProductLink,
                                    CompanyName,
                                    ProductName,
                                    ProductDescription,
                                    ProductPrice
                                ) VALUES (
                                    @ImageURL,
                                    --@ProductLink,
                                    @CompanyName,
                                    @ProductName,
                                    @ProductDescription,
                                    @ProductPrice
                                );

                                IF @@ERROR = 0
                                    SET @ReturnCode = 0;
                                ELSE
                                    RAISERROR('AddProduct - INSERT error: Product TABLE', 16, 1);
                            END

RETURN @ReturnCode;
GO

CREATE PROCEDURE UpdateProduct (
    @ProductNumber INT NULL,
    @ImageURL VARCHAR(2000) NULL,
    --@ProductLink VARCHAR(200) NULL,
    @CompanyName VARCHAR(50) NULL,
    @ProductName VARCHAR(1000) NULL,
    @ProductDescription VARCHAR(2000) NULL,
    @ProductPrice VARCHAR (100) NULL
)
AS
    DECLARE @ReturnCode INT;
    SET @ReturnCode = 1;

    IF @ProductNumber IS NULL
        RAISERROR ('UpdateProduct - Required Parameter: @ProductNumber', 16, 1);
    ELSE
        IF @ImageURL IS NULL
            RAISERROR ('UpdateProduct - Required Parameter: @ImageURL', 16, 1);
        ELSE
            --IF @ProductLink IS NULL
            --    RAISERROR ('UpdateProduct - Required Parameter: @ProductLink', 16, 1);
            --ELSE
                IF @CompanyName IS NULL
                    RAISERROR ('UpdateProduct - Required Parameter: @CompanyName', 16, 1);
                ELSE
                    IF @ProductName IS NULL
                        RAISERROR ('UpdateProduct - Required Parameter: @ProductName', 16, 1);
                    ELSE
                        IF @ProductDescription IS NULL
                            RAISERROR ('UpdateProduct - Required Parameter: @ProductDescription', 16, 1);
                        ELSE
                            IF @ProductPrice IS NULL
                                RAISERROR ('UpdateProduct - Required Parameter: @ProductPrice', 16, 1);
                            ELSE
                    BEGIN

                    -- Update the Product Item table
                    UPDATE Product
                    SET 
                        ImageURL = @ImageURL,
                        --ProductLink = @ProductLink,
                        CompanyName = @CompanyName,
                        ProductName = @ProductName,
                        ProductDescription = @ProductDescription,
                        ProductPrice = @ProductPrice
                    WHERE ProductNumber = @ProductNumber;

                        IF @@ERROR = 0
                            SET @ReturnCode = 0;
                        ELSE
                            RAISERROR('UpdateProduct - UPDATE error: Product TABLE', 16, 1);
                    END

RETURN @ReturnCode;
GO

CREATE PROCEDURE DeleteProduct (
    @ProductNumber INT NULL
)
AS
    DECLARE @ReturnCode INT;
    SET @ReturnCode = 1;

    IF @ProductNumber IS NULL
        RAISERROR('DeleteProduct - Required Parameter: @ProductNumber',16, 1);
    ELSE

        DELETE
        FROM Product
            WHERE ProductNumber = @ProductNumber;

        IF @@ERROR = 0
            SET @ReturnCode = 0;
        ELSE
            RAISERROR('DeleteProduct - DELETE error: Product TABLE',16, 1);

RETURN @ReturnCode;
GO

-------------------------------------
-- Testing
-------------------------------------
DELETE FROM UserAccountRoles;
DELETE FROM UserAccount;
DELETE FROM Role;
DBCC CHECKIDENT ('Role',RESEED,0);

DELETE FROM NewsItem;

-- Create some Roles
EXECUTE CreateRole 'Admin', 'An Administrator of the Capstone Customer Relations System.';
EXECUTE CreateRole 'Manager', 'A Manager of Capstone.';
EXECUTE CreateRole 'Employee', 'An Employee of Capstone.';
EXECUTE CreateRole 'Customer', 'A Customer of Capstone.';

-- Get all the Roles
EXECUTE GetRoles;

SELECT * FROM UserAccountRoles

--Add some User Accounts
EXECUTE AddUserAccountAndAssignRole 
	'Brendon',--First Name
	'Lapp',--Last name
	'7805555555',--Phone
	'BLapp', --UserName
	'Blapp@Capstone.com',--Email
	1000000001, --DCI Number
	--Password as text: 123
	'qfSVNu7ByreBCBxF3d2Z6yErrs3wPX1dyBw41FdoIK0=',--Password (hash)
	'i8Zaxr9Kx6wzryqbyjz/jQ==',--Salt
	'Admin';

	
EXECUTE AddUserAccountAndAssignRole 
	'Brendon',--First Name
	'Lapp',--Last name
	'7805555555',--Phone
	'BLap', --UserName
	'Casptone@Capstone.com',--Email
	1000000007, --DCI Number
	--Password as text: 123
	'qfSVNu7ByreBCBxF3d2Z6yErrs3wPX1dyBw41FdoIK0=',--Password (hash)
	'i8Zaxr9Kx6wzryqbyjz/jQ==',--Salt
	'Employee';
-- Get the User Account Number
--EXECUTE GetUserAccount 1

---- Assign the Admin Role to User Account Brendon Desmarais
--EXECUTE AssignRole 1, 1
---- Assign the Manager Role to User Account Brendon Desmarais
--EXECUTE AssignRole 1, 2
---- Assign the Employee Role to User Account Brendon Desmarais
--EXECUTE AssignRole 1, 3
---- Assign the Customer Role to User Account Brendon Desmarais
--EXECUTE AssignRole 1, 4

-- Get a User Account by their User Name
EXECUTE GetUserAccountByUserName 'BLapp'

-- Get all the User Accounts
EXECUTE GetUserAccounts;

-- Get all the News Items
EXECUTE GetNewsItems;

-- Create a news Item
EXECUTE AddNewsItem 'Deal', -- Image Type
                                            'https://images.ultrapro.com/products/medium/85717.jpg', -- Image URL
                                            'https://elderreign.com', -- Image Link
                                            --0, -- Index Position
                                            null, -- Optional Name
                                            null, -- Optional Description
                                            null; -- Optional Price

EXECUTE AddNewsItem 'Deal', -- Image Type
                                            'https://www.dccomics.com/sites/default/files/styles/comics320x485/public/comic-covers/2021/02/BMUL_01_60344263c43fb9.86685174.jpg', -- Image URL
                                            'https://elderreign.com', -- News Item Link
                                            --1, -- Index Position
                                            'DC Comics - Batman', -- Optional Name
                                            'Packed with action, bats, and bats making actions!', -- Optional Description
                                            '$8.99'; -- Optional Price

EXECUTE AddNewsItem 'Deal', -- Image Type
                                            'https://www.pokemoncenter.com/products/images/P6379/174-81712/P6379_174-81712_01.jpg', -- Image URL
                                            'https://elderreign.com', -- News Item Link
                                            --2, -- Index Position
                                            'Pokémon Sword & Shield Darkness Ablaze', -- Optional Name
                                            null, -- Optional Description
                                            null; -- Optional Price

EXECUTE AddNewsItem 'Deal', -- Image Type
                                            'https://images.ultrapro.com/products/medium/85717.jpg', -- Image URL
                                            'https://elderreign.com', -- News Item Link
                                            --3, -- Index Position
                                            null, -- Optional Name
                                            null, -- Optional Description
                                            null; -- Optional Price

EXECUTE AddNewsItem 'Deal', -- Image Type
                                            'https://images-na.ssl-images-amazon.com/images/I/91xJv0hdG8L._AC_SL1500_.jpg', -- Image URL
                                            'https://elderreign.com', -- News Item Link
                                            --3, -- Index Position
                                            null, -- Optional Name
                                            null, -- Optional Description
                                            null; -- Optional Price

EXECUTE AddNewsItem 'Deal', -- Image Type
                                            'https://images-na.ssl-images-amazon.com/images/I/91xJv0hdG8L._AC_SL1500_.jpg', -- Image URL
                                            'https://elderreign.com', -- News Item Link
                                            --3, -- Index Position
                                            null, -- Optional Name
                                            null, -- Optional Description
                                            null; -- Optional Price

-- Get all the News Items
EXECUTE GetNewsItems;

EXECUTE UpdateNewsItem 
    6, -- News Item Number
    'New', -- Image Type
    'https://images-na.ssl-images-amazon.com/images/I/618Ndx6j%2BSL._AC_SY355_.jpg', -- Image URL
    'https://elderreign.com', -- News Item Link
    --3, --Index Position
    'Haxtec 7PCS Zinc Alloy DND Metal Dice', -- Optional name
    '', -- Optional Description
    '$21.99'; -- Optional Price

-- Add Products
EXECUTE AddProduct
                                            'https://images.ultrapro.com/products/medium/85717.jpg', -- Image URL
                                            --'https://elderreign.com', -- Product Link
                                            'Ultra-Pro', -- Company Name
                                            'Deck Box', -- Optional Name
                                            'I very nice deck box.', -- Optional Description
                                            '14.99'; -- Optional Price

EXECUTE AddProduct
                                            'https://www.dccomics.com/sites/default/files/styles/comics320x485/public/comic-covers/2021/02/BMUL_01_60344263c43fb9.86685174.jpg', -- Image URL
                                            --'https://elderreign.com', -- Product Link
                                            'Ultra-Pro', -- Company Name
                                            'DC Comics - Batman', -- Optional Name
                                            'Packed with action, bats, and bats making actions!', -- Optional Description
                                            '$8.99'; -- Optional Price

EXECUTE AddProduct
                                            'https://www.pokemoncenter.com/products/images/P6379/174-81712/P6379_174-81712_01.jpg', -- Image URL
                                            --'https://elderreign.com', -- Product Link
                                            'Nintendo', -- Company Name
                                            'Pokémon Sword & Shield Darkness Ablaze', -- Optional Name
                                            'Cards!', -- Optional Description
                                            10.99; -- Optional Price

EXECUTE AddProduct
                                            'https://images-na.ssl-images-amazon.com/images/I/91xJv0hdG8L._AC_SL1500_.jpg', -- Image URL
                                            --'https://elderreign.com', -- News Item Link
                                            'Star Wars', -- Company Name
                                            'Baby Yoda night light', -- Optional Name
                                            'Baby Yoda, Grogu', -- Optional Description
                                            19.99; -- Optional Price

-- Get all Products
EXECUTE GetProducts;                                            

--ToDo: Authorize who can use the stored procedure.
-------------------------------------
-- Assigning permissions
-------------------------------------
-- DROP PROCEDURE GetDatabaseUser;
-- GO
-- CREATE PROCEDURE GetDatabaseUser
-- AS
--     DECLARE @ReturnCode INT
--     SET @ReturnCode = 1

--     SELECT CURRENT_USER AS CurrentUser, -- Returns the name of the current user.
--                 SYSTEM_USER AS SystemUser, -- Returns the name of the currently excuting context.
--                 SESSION_USER AS SessionUser; -- Returns the name of the current context in the current database.

--     IF @@ERROR = 0
--         SET @ReturnCode = 0;
--     ELSE
--         RAISERROR('GetDatabaseUser - Database User error.', 16, 1);

-- RETURN @ReturnCode
-- GO

-- EXECUTE GetDatabaseUser;
-- GO

-- sp_helpuser

-- -- Making a user.
-- DROP USER aspnetcore
-- GO
-- CREATE USER aspnetcore FOR LOGIN [BUILTIN\IIS_IUSRS]

-- -- Granting execute permissions.

--GRANT EXECUTE ON AddUserAccountAndAssignRole
--	TO aspnetcore;
--GRANT EXECUTE ON UpdateUserAccount
--	TO aspnetcore;
--GRANT EXECUTE ON UpdateUserAccountPassword
--	TO aspnetcore;
--GRANT EXECUTE ON AddUserAccount
--    TO aspnetcore;
--GRANT EXECUTE ON GetUserAccount
--    TO aspnetcore;
--GRANT EXECUTE ON AssignRole
--    TO aspnetcore;
--GRANT EXECUTE ON CreateRole
--    TO aspnetcore;
--GRANT EXECUTE ON UnAssignRole
--	TO aspnetcore;
--GRANT EXECUTE ON GetRoles
--    TO aspnetcore;
--GRANT EXECUTE ON GetUserAccounts
--    TO aspnetcore;
--GRANT EXECUTE ON Login
--    TO aspnetcore;
--GRANT EXECUTE ON GetUserSalt
--    TO aspnetcore;
--GRANT EXECUTE ON GetUserAccountByUserName
--    TO aspnetcore;

--GRANT EXECUTE ON GetNewsItems
--    TO aspnetcore;
--GRANT EXECUTE ON GetNewsItem
--    TO aspnetcore;
--GRANT EXECUTE ON AddNewsItem
--    TO aspnetcore;
--GRANT EXECUTE ON UpdateNewsItem
--    TO aspnetcore;
--GRANT EXECUTE ON DeleteNewsItem
--    TO aspnetcore;
    
--GRANT EXECUTE ON GetProducts
--    TO aspnetcore;
--GRANT EXECUTE ON GetProduct
--    TO aspnetcore;
--GRANT EXECUTE ON AddProduct
--    TO aspnetcore;
--GRANT EXECUTE ON UpdateProduct
--    TO aspnetcore;
--GRANT EXECUTE ON DeleteProduct
--    TO aspnetcore;


--GRANT EXECUTE ON GetUserAccountNumber
--    TO aspnetcore;
--GRANT EXECUTE ON AddCardToCart
--    TO aspnetcore;
--GRANT EXECUTE ON AddProductToCart
--    TO aspnetcore;
--GRANT EXECUTE ON DeleteFromCart
--    TO aspnetcore;
--GRANT EXECUTE ON GetCart
--    TO aspnetcore;
--GRANT EXECUTE ON DeleteCart
--    TO aspnetcore;
--GRANT EXECUTE ON GetMTGCards
--    TO aspnetcore;
--GRANT EXECUTE ON GetMTGCardsByQuery
--    TO aspnetcore;
--GRANT EXECUTE ON AddCardToMTGCards
--    TO aspnetcore;
--GRANT EXECUTE ON UpdateMTGCard
--    TO aspnetcore;
--GRANT EXECUTE ON GetCard
--    TO aspnetcore;
--GRANT EXECUTE ON GetCards
--    TO aspnetcore;
--GRANT EXECUTE ON GetAllCardsByQuery
--    TO aspnetcore;
--GRANT EXECUTE ON GetExistingSets
--    TO aspnetcore;
--GRANT EXECUTE ON GetBuylist
--    TO aspnetcore;
--GRANT EXECUTE ON AddToBuylist
--    TO aspnetcore;
--GRANT EXECUTE ON DeleteFromBuylist
--    TO aspnetcore;
--GRANT EXECUTE ON GetBuylistByQuery
--    TO aspnetcore;
--GRANT EXECUTE ON AddOrder
--    TO aspnetcore;
--GRANT EXECUTE ON UpdateOrder
--    TO aspnetcore;
--GRANT EXECUTE ON GetOrders
--    TO aspnetcore;
--GRANT EXECUTE ON GetOrdersByStatus
--    TO aspnetcore;
--GRANT EXECUTE ON GetIncompletedOrders
--    TO aspnetcore;
--GRANT EXECUTE ON GetOrdersByOrderID
--    TO aspnetcore;
--GRANT EXECUTE ON GetOrdersByCustomer
--    TO aspnetcore;
--GRANT EXECUTE ON AddOrderItems
--    TO aspnetcore;
--GRANT EXECUTE ON GetOrderItems
--    TO aspnetcore;
--GRANT EXECUTE ON UpdateOrderItem
--    TO aspnetcore;
--GRANT EXECUTE ON AddCardToOrderItem
--    TO aspnetcore;
--GRANT EXECUTE ON AddProductToOrderItem
--    TO aspnetcore;
--GRANT EXECUTE ON UpdateCart
--    TO aspnetcore;
--GRANT EXECUTE ON CompleteOrder
--    TO aspnetcore;
--GRANT EXECUTE ON GetUserByAccountNumber
--    TO aspnetcore;
--GRANT EXECUTE ON UpdateOrderAfterSale
--		TO aspnetcore;