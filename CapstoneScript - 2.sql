 USE CasptoneCustomerRelationsSystem;
DROP TABLE Buylist;
DROP TABLE OrderItems;
DROP TABLE Orders;
DROP TABLE Cart;
DROP TABLE MTGCards;


CREATE TABLE MTGCards (
	CardID VARCHAR(200) NOT NULL,
	CardName VARCHAR(150) NOT NULL,
	URI VARCHAR(300) NOT NULL,
	[Set] VARCHAR(20) NOT NULL,
	SetName VARCHAR(200) NOT NULL,
	CollectorNumber VARCHAR(20) NOT NULL,
	Rarity VARCHAR(20) NOT NULL,
	ImageSmall VARCHAR(300) NOT NULL,
	ImageNormal VARCHAR(300) NOT NULL,
	ImageLarge VARCHAR(300) NOT NULL,
	Price MONEY NULL,
	LastUpdated DATE NULL,
);

ALTER TABLE MTGCards
	ADD CONSTRAINT PK_CardID PRIMARY KEY(CardID);

CREATE TABLE Buylist(
	BuylistID INT IDENTITY(1,1) NOT NULL,
	CardID VARCHAR(200) NOT NULL
);

ALTER TABLE Buylist
	ADD CONSTRAINT PK_BuylistID PRIMARY KEY(BuylistID);
ALTER TABLE Buylist
	ADD CONSTRAINT FK_CardID FOREIGN KEY(CardID) REFERENCES MTGCards(CardID);


CREATE TABLE Orders (
	OrderID INT IDENTITY(1000000,1) NOT NULL,
	CustomerUserAccountNumber INT NOT NULL,
	EmployeeUserAccountNumber INT NULL,
	PlacedDate DATE NOT NULL,
	CompletedDate DATE NULL,
	OrderStatus VARCHAR(20) NOT NULL,
	GST MONEY NOT NULL,
	SubTotal MONEY NOT NULL,
	Total MONEY NOT NULL,
	RequestedStore VARCHAR(10) NOT NULL
);
ALTER TABLE Orders
	ADD CONSTRAINT PK_Orders PRIMARY KEY(OrderID);
ALTER TABLE Orders
	ADD CONSTRAINT FK_CustomerUserAccountNumber FOREIGN KEY(CustomerUserAccountNumber) REFERENCES UserAccount(UserAccountNumber);
ALTER TABLE Orders
	ADD CONSTRAINT FK_EmployeeUserAccoutnNumber FOREIGN KEY(EmployeeUserAccountNumber) REFERENCES UserAccount(UserAccountNumber);

CREATE TABLE OrderItems (
	OrderItemID INT IDENTITY(1,1) NOT NULL,
	OrderID INT NOT NULL,
	ProductID INT NULL,
	CardID VARCHAR(200) NULL,
	QuantityRequested INT NOT NULL,
	QuantityOnHand INT NULL,
	LineItemPrice MONEY NOT NULL
);
ALTER TABLE OrderItems
	ADD CONSTRAINT PK_OrderItems PRIMARY KEY(OrderItemID);
ALTER TABLE OrderItems
	ADD CONSTRAINT FK_OrderID FOREIGN KEY(OrderID) REFERENCES Orders(OrderID);
ALTER TABLE OrderItems
	ADD CONSTRAINT FK_ProductID FOREIGN KEY(ProductID) REFERENCES Product(ProductNumber);
ALTER TABLE OrderItems
	ADD CONSTRAINT FK_CardIDOrderItem FOREIGN KEY(CardID) REFERENCES MTGCards(CardID);

CREATE TABLE Cart (
	CartID INT IDENTITY(1,1) NOT NULL,
	UserAccountNumber INT NOT NULL,
	ProductNumber INT NULL,
	CardID VARCHAR(200) NULL,
	Quantity int NOT NULL
);
ALTER TABLE Cart
	ADD CONSTRAINT PK_CartID PRIMARY KEY(CartID);
ALTER TABLE Cart
	ADD CONSTRAINT FK_UserAccountNumber FOREIGN KEY(UserAccountNumber) REFERENCES UserAccount(UserAccountNumber);
ALTER TABLE Cart
	ADD CONSTRAINT FK_CartProductNumber FOREIGN KEY(ProductNumber) REFERENCES Product(ProductNumber);
ALTER TABLE Cart
	ADD CONSTRAINT FK_CartMTGCardID FOREIGN KEY(CardID) REFERENCES MTGCards(CardID);

DROP PROCEDURE GetUserAccountNumber;
DROP PROCEDURE AddCardToCart;
DROP PROCEDURE AddProductToCart;
DROP PROCEDURE DeleteFromCart;
DROP PROCEDURE GetCart;
DROP PROCEDURE DeleteCart;
DROP PROCEDURE GetMTGCards;
DROP PROCEDURE GetMTGCardsByQuery;
DROP PROCEDURE AddCardToMTGCards;
DROP PROCEDURE UpdateMTGCard;
DROP PROCEDURE GetCard;
DROP PROCEDURE GetCards;
DROP PROCEDURE GetAllCardsByQuery;
DROP PROCEDURE GetExistingSets;
DROP PROCEDURE GetBuylist;
DROP PROCEDURE AddToBuylist;
DROP PROCEDURE DeleteFromBuylist;
DROP PROCEDURE GetBuylistByQuery;
DROP PROCEDURE AddOrder;
DROP PROCEDURE UpdateOrder;
DROP PROCEDURE GetOrders;
DROP PROCEDURE GetOrdersByStatus;
DROP PROCEDURE GetIncompletedOrders;
DROP PROCEDURE GetOrdersByOrderID;
DROP PROCEDURE GetOrdersByCustomer;
DROP PROCEDURE AddOrderItems;
DROP PROCEDURE GetOrderItems;
DROP PROCEDURE UpdateOrderItem;
DROP PROCEDURE AddCardToOrderItem;
DROP PROCEDURE AddProductToOrderItem;
DROP PROCEDURE UpdateCart;
DROP PROCEDURE CompleteOrder;
DROP PROCEDURE GetUserByAccountNumber;
DROP PROCEDURE UpdateOrderAfterSale;

GO
CREATE PROCEDURE GetMTGCards
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	SELECT
		CardID,
		CardName,
		URI,
		[Set],
		SetName,
		CollectorNumber,
		Rarity,
		ImageSmall,
		ImageNormal,
		ImageLarge,
		Price,
		LastUpdated
	FROM MTGCards

	IF @@ERROR = 1
		BEGIN
			RAISERROR('GetMTGCards: Failed to select from MTGCards', 16, 1)
			SET @ReturnCode = 1
		END
END
RETURN @ReturnCode

GO
CREATE PROCEDURE GetMTGCardsByQuery
(
	@Query VARCHAR(100) NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	SELECT
		CardID,
		CardName,
		URI,
		[Set],
		SetName,
		CollectorNumber,
		Rarity,
		ImageSmall,
		ImageNormal,
		ImageLarge,
		Price,
		LastUpdated
	FROM MTGCards
	WHERE CardName LIKE '%' + @Query + '%'

	IF @@ERROR = 1
		BEGIN
			RAISERROR('GetMTGCardsByQuery: Failed to select from MTGCards', 16, 1)
			SET @ReturnCode = 1
		END
END
RETURN @ReturnCode

GO
CREATE PROCEDURE AddCardToMTGCards
(
	@CardID VARCHAR(200) NULL,
	@CardName VARCHAR(150) NULL,
	@URI VARCHAR(300) NULL,
	@Set VARCHAR(20) NULL,
	@SetName VARCHAR(200) NULL,
	@CollectorNumber VARCHAR(20) NULL,
	@Rarity VARCHAR(20) NULL,
	@ImageSmall VARCHAR(300) NULL,
	@ImageNormal VARCHAR(300) NULL,
	@ImageLarge VARCHAR(300) NULL,
	@Price MONEY NULL,
	@LastUpdated DATETIME NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	IF @CardID IS NULL
		RAISERROR('AddCardToMTGCards: CardID cannot be null', 16, 1)
		IF @CardName IS NULL
			RAISERROR('AddCardToMTGCards: CardName cannot be null', 16, 1)
			IF @URI IS NULL	
				RAISERROR('AddCardToMTGCards: URI cannot be null' ,16, 1)
				IF @Set IS NULL
					RAISERROR('AddCardToMTGCards: Set cannot be null', 16, 1)
					IF @SetName IS NULL
						RAISERROR('AddCardToMTGCards: SetName cannot be null', 16, 1)
						IF @CollectorNumber IS NULL
							RAISERROR('AddCardToMTGCards: CollectorNumber cannot be null', 16, 1)
							IF @Rarity IS NULL	
								RAISERROR('AddCardToMTGCards: Rarity cannot be null' ,16, 1)
								IF @ImageSmall IS NULL
									RAISERROR('AddCardToMTGCards: ImageSmall cannot be null' ,16, 1)
									IF @ImageNormal IS NULL
										RAISERROR('AddCardToMTGCards: ImageLarge cannot be null', 16, 1)
										IF @ImageLarge IS NULL
											RAISERROR('AddCardToMTGCards: ImageLarge cannot be null', 16, 1)
		BEGIN
			INSERT
			INTO MTGCards
			(
				CardID,
				CardName,
				URI,
				[Set],
				SetName,
				CollectorNumber,
				Rarity,
				ImageSmall,
				ImageNormal,
				ImageLarge,
				Price,
				LastUpdated
			)
			VALUES
			(
				@CardID,
				@CardName,
				@URI,
				@Set,
				@SetName,
				@CollectorNumber,
				@Rarity,
				@ImageSmall,
				@ImageNormal,
				@ImageLarge,
				@Price,
				@LastUpdated
			)

			IF @@ERROR = 1
				BEGIN
					RAISERROR('AddCardToMTGCards: Failed to insert into MTGCards', 16, 1)
					SET @ReturnCode = 1
				END
		END
END
RETURN @ReturnCode

GO
CREATE PROCEDURE UpdateMTGCard
(
	@CardID VARCHAR(200) NULL,
	@Price MONEY NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	IF @CardID IS NULL
		RAISERROR('UpdateMTGCard: CardID cannot be null' ,16, 1)
		IF @Price IS NULL
			RAISERROR('UpdateMTGCard: Price cannot be null', 16, 1)
	BEGIN
		UPDATE
			MTGCards
		SET
			Price = @Price
		WHERE
			CardID = @CardID
		IF @@ERROR = 1
			BEGIN
				RAISERROR('UpdateMTGCard: Failed to update the MTG card', 16, 1)
				SET @ReturnCode = 1
			END
	END
END
RETURN @ReturnCode


GO
CREATE PROCEDURE GetCard
(
	@CardID VARCHAR(200) NULL
)
AS
BEGIN
	DECLARE @ReturnCode INT
	SET @ReturnCode = 0

	SELECT
		CardID,
		CardName,
		URI,
		[Set],
		SetName,
		CollectorNumber,
		Rarity,
		ImageSmall,
		ImageNormal,
		ImageLarge,
		Price
	FROM MTGCards
	WHERE @CardID = CardID

	IF @@ERROR = 1
		BEGIN
			RAISERROR('GetCards: Failed to select from MTGCards', 16, 1)
			SET @ReturnCode = 1
		END
END
RETURN @ReturnCode
GO
CREATE PROCEDURE GetCards
AS
BEGIN
	DECLARE @ReturnCode INT
	SET @ReturnCode = 0

	SELECT
		CardID,
		CardName,
		URI,
		[Set],
		SetName,
		CollectorNumber,
		Rarity,
		ImageSmall,
		ImageNormal,
		ImageLarge,
		Price
	FROM MTGCards

	IF @@ERROR = 1
		BEGIN
			RAISERROR('GetCards: Failed to select from MTGCards', 16, 1)
			SET @ReturnCode = 1
		END
END
RETURN @ReturnCode

GO
CREATE PROCEDURE GetAllCardsByQuery
(
	@Query VARCHAR(200)
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	SELECT
		CardID,
		CardName,
		URI,
		[Set],
		SetName,
		CollectorNumber,
		Rarity,
		ImageSmall,
		ImageNormal,
		ImageLarge,
		Price
	FROM MTGCards
	WHERE CardName LIKE '%' + @Query + '%'

	IF @@ERROR = 1
		BEGIN
			RAISERROR('GetAllCardsByQuery: Failed to select from MTGCards', 16, 1)
			SET @ReturnCode = 1
		END
END
RETURN @ReturnCode

GO
CREATE PROCEDURE GetExistingSets
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	SELECT 
		[Set] 
	FROM MTGCards 
	GROUP BY [Set]

	IF @@ERROR = 1
		BEGIN
			RAISERROR('GetExistingSets: Failed to select from MTGCards', 16, 1)
			SET @ReturnCode = 1
		END
END
RETURN @ReturnCode

GO
CREATE PROCEDURE GetBuylist
AS
BEGIN
DECLARE @ReturnCode INT
SET @ReturnCode = 0
	BEGIN
		SELECT
			BuylistID,
			Buylist.CardID,
			CardName,
			URI,
			[Set],
			SetName,
			CollectorNumber,
			Rarity,
			ImageSmall,
			ImageNormal,
			ImageLarge,
			Price
		FROM Buylist
		INNER JOIN MTGCards
		ON MTGCards.CardID = Buylist.CardID
	END
END
RETURN @ReturnCode

GO
CREATE PROCEDURE AddToBuylist
(
	@CardID VARCHAR(200) NULL
	--AddedBy INT NULL
)
AS
BEGIN
DECLARE @ReturnCode INT
SET @ReturnCode = 1
	BEGIN
		IF @CardID IS NULL
			RAISERROR('AddToBuylist: CardID cannot be null', 16, 1)
		INSERT
		INTO
		Buylist
		(
			CardID
		)
		VALUES
		(
			@CardID		
		)

		IF @@ERROR = 1
			BEGIN
				RAISERROR('AddToBuylist: Failed to insert into Buylist', 16, 1)
				SET @ReturnCode = 1
			END
	END
END
RETURN @ReturnCode

GO
CREATE PROCEDURE DeleteFromBuylist
(
	@BuylistID INT NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	IF @BuylistID IS NULL
		RAISERROR('DeleteFromBuylist: BuylistID cannot be null', 16, 1)
	DELETE
	FROM Buylist
	WHERE BuylistID = @BuylistID

	IF @@ERROR = 1
		BEGIN
			RAISERROR('DeleteFromBuylist: Failed to delete from Buylist', 16, 1)
			SET @ReturnCode = 1
		END
END
RETURN @ReturnCode

GO
CREATE PROCEDURE GetBuylistByQuery
(
	@Query VARCHAR(200) NULL
)
As
BEGIN
DECLARE @ReturnCode INT
SET @ReturnCode = 0
	BEGIN
		SELECT
			BuylistID,
			Buylist.CardID,
			CardName,
			URI,
			[Set],
			SetName,
			CollectorNumber,
			Rarity,
			ImageSmall,
			ImageNormal,
			ImageLarge,
			Price
		FROM Buylist
		INNER JOIN MTGCards
		ON MTGCards.CardID = Buylist.CardID
	END
END
RETURN @ReturnCode

GO
CREATE PROCEDURE AddOrder
(
	@CustomerUserAccountNumber INT NULL,
	@PlacedDate DATE NULL,
	@OrderStatus VARCHAR(20) NULL,
	@GST MONEY NULL,
	@SubTotal MONEY NULL,
	@Total MONEY NULL,
	@CasptoneLocation VARCHAR(10) NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	IF @CustomerUserAccountNumber IS NULL
		RAISERROR('AddOrder: CustomerAccountNumber cannot be null', 16, 1)
		IF @PlacedDate IS NULL
			RAISERROR('AddOrder: PlacedDate cannot be null', 16, 1)
			IF @OrderStatus IS NULL
				RAISERROR('AddOrder: OrderStatus cannot be null', 16, 1)
				IF @GST IS NULL
					RAISERROR('AddOrder: GST cannot be null', 16, 1)
					IF @SubTotal IS NULL
						RAISERROR('AddOrder: SubTotal cannot be null', 16, 1)
						IF @Total IS NULL
							RAISERROR('AddOrder: Total cannot be null', 16, 1)
							IF @CasptoneLocation IS NULL
								RAISERROR('AddOrder: CasptoneLocation cannot be null', 16, 1)
	INSERT
	INTO Orders
	(
		CustomerUserAccountNumber, 
		PlacedDate, 
		OrderStatus, 
		GST, 
		SubTotal, 
		Total,
		RequestedStore
	)
	VALUES
	(
		@CustomerUserAccountNumber,
		@PlacedDate,
		@OrderStatus,
		@GST,
		@SubTotal,
		@Total,
		@CasptoneLocation
	)

	SELECT SCOPE_IDENTITY()
END
RETURN @ReturnCode

GO
CREATE PROCEDURE UpdateOrder
(
	@OrderID INT NULL,
	@EmployeeUserAccountNumber INT NULL,
	@OrderStatus VARCHAR(20) NULL,
	@Tax MONEY NULL,
	@SubTotal MONEY NULL,
	@Total MONEY NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	IF @OrderID IS NULL
		RAISERROR('UpdateOrder: OrderID cannot be null', 16, 1)
		IF @EmployeeUserAccountNumber IS NULL
			RAISERROR('UpdateOrder: EmployeeUserAccountNumber cannot be null', 16, 1)
			IF @OrderStatus IS NULL
				RAISERROR('UpdateOrder: OrderStatus cannot be null', 16, 1)
				IF @Tax IS NULL
					RAISERROR('UpdateOrder: Tax cannot be null', 16, 1)
					IF @SubTotal IS NULL
						RAISERROR('UpdateOrder: SubTotal cannot be null', 16, 1)
						IF @Total IS NULL
							RAISERROR('UpdateOrder: Total cannot be null', 16, 1)
	UPDATE Orders
	SET
		EmployeeUserAccountNumber = @EmployeeUserAccountNumber,
		GST = @Tax,
		SubTotal = @SubTotal,
		Total = @Total,
		OrderStatus = @OrderStatus
	WHERE
		OrderID = @OrderID

	IF @@ERROR = 1
		BEGIN
			RAISERROR('UpdateOrder: Failed to update the Order', 16, 1)
			SET @ReturnCode = 1
		END
END
RETURN @ReturnCode
GO
CREATE PROCEDURE UpdateOrderAfterSale
(
	@OrderID INT NULL,
	@OrderStatus VARCHAR(20) NULL,
	@CompletedDate DATE NULL,
	@Tax MONEY NULL,
	@SubTotal MONEY NULL,
	@Total MONEY NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	IF @OrderID IS NULL
		RAISERROR('UpdateOrderAfterSale: OrderID cannot be null', 16, 1)
		IF @CompletedDate IS NULL
			RAISERROR('UpdateOrderAfterSale: CompletedDate cannot be null', 16, 1)
			IF @OrderStatus IS NULL
				RAISERROR('UpdateOrderAfterSale: OrderStatus cannot be null', 16, 1)
				IF @Tax IS NULL
					RAISERROR('UpdateOrder: Tax cannot be null', 16, 1)
					IF @SubTotal IS NULL
						RAISERROR('UpdateOrder: SubTotal cannot be null', 16, 1)
						IF @Total IS NULL
							RAISERROR('UpdateOrder: Total cannot be null', 16, 1)
	UPDATE Orders
	SET
		OrderStatus = @OrderStatus,
		GST = @Tax,
		SubTotal = @SubTotal,
		Total = @Total,
		CompletedDate = @CompletedDate
	WHERE
		OrderID = @OrderID

	IF @@ERROR = 1
		BEGIN
			RAISERROR('UpdateOrderAfterSale: Failed to update the Order', 16, 1)
			SET @ReturnCode = 1
		END
END
RETURN @ReturnCode
GO
CREATE PROCEDURE GetOrders
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	SELECT
		OrderID,
		CustomerUserAccountNumber,
		EmployeeUserAccountNumber,
		PlacedDate,
		CompletedDate,
		OrderStatus,
		GST,
		SubTotal,
		Total,
		RequestedStore
	FROM Orders
	ORDER BY PlacedDate DESC

	IF @@ERROR = 1
		BEGIN
			RAISERROR('GetOrders: Failed to select from Orders', 16, 1)
			SET @ReturnCode = 1
		END
END
RETURN @ReturnCode

GO
CREATE PROCEDURE GetOrdersByStatus
(
	@OrderStatus VARCHAR(20) NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	SELECT
		OrderID,
		CustomerUserAccountNumber,
		EmployeeUserAccountNumber,
		PlacedDate,
		CompletedDate,
		OrderStatus,
		GST,
		SubTotal,
		Total, 
		RequestedStore
	FROM Orders
	WHERE OrderStatus = @OrderStatus
	ORDER BY PlacedDate DESC

	IF @@ERROR = 1
		BEGIN
			RAISERROR('GetOrders: Failed to select from Orders', 16, 1)
			SET @ReturnCode = 1
		END
END
RETURN @ReturnCode
GO
CREATE PROCEDURE GetOrdersByCustomer
(
	@UserAccountNumber VARCHAR(20) NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	SELECT
		OrderID,
		PlacedDate,
		CompletedDate,
		OrderStatus,
		GST,
		SubTotal,
		Total,
		RequestedStore
	FROM Orders
	WHERE CustomerUserAccountNumber = @UserAccountNumber
	ORDER BY PlacedDate DESC

	IF @@ERROR = 1
		BEGIN
			RAISERROR('GetOrders: Failed to select from Orders', 16, 1)
			SET @ReturnCode = 1
		END
END
RETURN @ReturnCode
GO
CREATE PROCEDURE GetIncompletedOrders
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	SELECT
		OrderID,
		CustomerUserAccountNumber,
		CustomerName = (SELECT
							FirstName + ' ' + LastName
						FROM UserAccount
						WHERE UserAccountNumber = CustomerUserAccountNumber),
		EmployeeUserAccountNumber,
		EmployeeName = (SELECT
							FirstName + ' ' + LastName
						FROM UserAccount
						WHERE UserAccountNumber = EmployeeUserAccountNumber),
		PlacedDate,
		CompletedDate,
		OrderStatus,
		GST,
		SubTotal,
		Total,
		RequestedStore
	FROM Orders
	WHERE OrderStatus NOT LIKE 'Completed'
	AND OrderStatus NOT LIKE 'Rejected'
	ORDER BY PlacedDate DESC

	IF @@ERROR = 1
		BEGIN
			RAISERROR('GetOrders: Failed to select from Orders', 16, 1)
			SET @ReturnCode = 1
		END
END
RETURN @ReturnCode

GO
CREATE PROCEDURE GetOrdersByOrderID
(
	@OrderID INT NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	SELECT
		OrderID,
		CustomerUserAccountNumber,
		CustomerName = (SELECT
							FirstName + ' ' + LastName
						FROM UserAccount
						WHERE UserAccountNumber = CustomerUserAccountNumber),
		Email = (SELECT
					Email
				FROM UserAccount
				WHERE UserAccountNumber = CustomerUserAccountNumber),
		PhoneNumber = (SELECT
							PhoneNumber
						FROM UserAccount
						WHERE UserAccountNumber = CustomerUserAccountNumber),
		EmployeeUserAccountNumber,
		EmployeeName = (SELECT
							FirstName + ' ' + LastName
						FROM UserAccount
						WHERE UserAccountNumber = EmployeeUserAccountNumber),
		PlacedDate,
		CompletedDate,
		OrderStatus,
		GST,
		SubTotal,
		Total,
		RequestedStore
	FROM Orders
	WHERE OrderID = @OrderID

	IF @@ERROR = 1
		BEGIN
			RAISERROR('GetOrders: Failed to select from Orders', 16, 1)
			SET @ReturnCode = 1
		END
END
RETURN @ReturnCode

GO
CREATE PROCEDURE AddOrderItems
(
	@OrderID INT NULL,
	@ProductID INT NULL,
	@CardID VARCHAR(200) NULL,
	@QuantityRequested INT NULL,
	@LineItemPrice MONEY NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	IF @OrderID IS NULL
		RAISERROR('AddOrderItems: OrderID cannot be null', 16, 1)
		IF @QuantityRequested IS NULL
			RAISERROR('AddOrderItems: QuantityRequested cannot be null', 16, 1)
			IF @LineItemPrice IS NULL
				RAISERROR('AddOrderItems: LineItemPrice cannot be null', 16, 1)
	INSERT
	INTO OrderItems
	(
		OrderID,
		ProductID,
		CardID,
		QuantityRequested,
		LineItemPrice
	)
	VALUES
	(
		@OrderID,
		@ProductID,
		@CardID,
		@QuantityRequested,
		@LineItemPrice
	)

	IF @@ERROR = 1
		BEGIN
			RAISERROR('AddOrderItems: Failed to insert into OrderItems', 16, 1)
			SET @ReturnCode = 1
		END
END
RETURN @ReturnCode

GO
CREATE PROCEDURE GetOrderItems
(
	@OrderID INT NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	IF @OrderID IS NULL
		RAISERROR('GetOrderItems: OrderID cannot be null', 16, 1)

	SELECT
		OrderItemID,
		ProductID,
		CardID,
		QuantityRequested,
		QuantityOnHand,
		LineItemPrice
	FROM OrderItems
	WHERE OrderID = @OrderID

	IF @@ERROR = 1
		BEGIN
			RAISERROR('GetOrderItems: Failed to select from OrderItems', 16, 1)
			SET @ReturnCode = 1
		END
END
RETURN @ReturnCode

GO
CREATE PROCEDURE UpdateOrderItem
(
	@OrderItemID INT NULL,
	@QuantityOnHand INT NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	IF @OrderItemID IS NULL
		RAISERROR('UpdateOrderItem: OrderItemID cannot be null', 16, 1)
		IF @QuantityOnHand IS NULL
			RAISERROR('UpdateOrderItem: QuantityOnHand cannot be null', 16, 1)
	BEGIN
		UPDATE OrderItems
		SET
			QuantityOnHand = @QuantityOnHand
		WHERE @OrderItemID = OrderItemID

		IF @@ERROR = 1
			BEGIN
				RAISERROR('UpdateOrderItem: Failed to update OrderItems', 16, 1)
				SET @ReturnCode = 1
			END
	END
END
RETURN @ReturnCode
GO
CREATE PROCEDURE GetUserAccountNumber
(
	@UserName VARCHAR(50)
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	IF @UserName IS NULL
		RAISERROR('GetUserAccountNumber: UserName cannot be null', 16, 1)
	SELECT
		UserAccountNumber
	FROM UserAccount
	WHERE UserName = @UserName
END
RETURN @ReturnCode


GO
CREATE PROCEDURE AddCardToCart
(
	@CardID VARCHAR(200) NULL,
	@Quantity INT NULL,
	@UserAccountNumber INT NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	IF @Quantity IS NULL
		RAISERROR('AddToCart: Quantity cannot be null', 16, 1)
		IF @UserAccountNumber IS NULL
			RAISERROR('AddToCart: UserAccountNumber cannot be null', 16, 1)
	INSERT INTO
	Cart
	(
		CardID,
		UserAccountNumber,
		Quantity
	)
	VALUES	
	(
		@CardID,
		@UserAccountNumber,
		@Quantity
	)

	IF @@ERROR = 1
		BEGIN
			RAISERROR('AddToCart: Failed to insert into Cart', 16, 1)
			SET @ReturnCode = 1
		END
END
RETURN @ReturnCode
GO
CREATE PROCEDURE AddProductToCart
(
	@ProductID INT NULL,
	@Quantity INT NULL,
	@UserAccountNumber INT NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	IF @Quantity IS NULL
		RAISERROR('AddToCart: Quantity cannot be null', 16, 1)
		IF @UserAccountNumber IS NULL
			RAISERROR('AddToCart: UserAccountNumber cannot be null', 16, 1)
	INSERT INTO
	Cart
	(
		ProductNumber,
		UserAccountNumber,
		Quantity
	)
	VALUES	
	(
		@ProductID,
		@UserAccountNumber,
		@Quantity
	)

	IF @@ERROR = 1
		BEGIN
			RAISERROR('AddToCart: Failed to insert into Cart', 16, 1)
			SET @ReturnCode = 1
		END
END
RETURN @ReturnCode
GO
CREATE PROCEDURE DeleteFromCart
(
	@CartID INT NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	IF @CartID IS NULL
		RAISERROR('DeletFromCart: CartID cannot be null', 16, 1)
	DELETE
	FROM Cart
	WHERE CartID = @CartID

	IF @@ERROR = 1
		BEGIN
			RAISERROR('DeleteFromCart: Failed to delete from Cart', 16, 1)
			SET @ReturnCode = 1
		END
END
RETURN @ReturnCode
GO
CREATE PROCEDURE GetCart
(
	@UserAccountNumber INT NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	IF @UserAccountNumber IS NULL
		RAISERROR('GetCart: UserAccountNumber cannot be null', 16, 1)
	SELECT
		UserAccountNumber,
		CartID,
		CardID,
		ProductNumber,
		Quantity,
		CASE
			WHEN CardID IS NULL THEN (SELECT
											ProductPrice
										FROM Product
										WHERE ProductNumber = CT.ProductNumber)
			WHEN ProductNumber IS NULL THEN (SELECT
													Price
												FROM MTGCards
												WHERE CardID = CT.CardID)
		END AS Price
	FROM Cart AS CT
	WHERE UserAccountNumber = @UserAccountNumber
END
RETURN @ReturnCode
GO
CREATE PROCEDURE DeleteCart
(
	@UserAccountNumber INT NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	IF @UserAccountNumber IS NULL
		RAISERROR('DeleteCart: UserAccountNumber cannot be null', 16, 1)
	DELETE 
	FROM Cart
	WHERE UserAccountNumber = @UserAccountNumber

	IF @@ERROR = 1
		BEGIN
			RAISERROR('DeleteCart: Failed to delete from Cart', 16, 1)
			SET @ReturnCode = 1
		END
END
RETURN @ReturnCode
GO
CREATE PROCEDURE AddCardToOrderItem
(
	@OrderID INT NULL,
	@CardID VARCHAR(200) NULL,
	@QuantityRequested INT NULL,
	@LineItemPrice MONEY NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	IF @OrderID IS NULL
		RAISERROR('AddCardToOrderItem: OrderID cannot be null', 16, 1)
		IF @CardID IS NULL
			RAISERROR('AddCardToOrderItem: CardID cannot be null', 16, 1)
			IF @QuantityRequested IS NULL
				RAISERROR('AddCardToOrderItem: QuantityRequested cannot be null', 16, 1)
				IF @LineItemPrice IS NULL
					RAISERROR('AddCardToOrderItem: LineItemPrice cannot be null', 16, 1)
	INSERT
	INTO OrderItems
	(OrderID, CardID, QuantityRequested, LineItemPrice)
	VALUES
	(@OrderID, @CardID, @QuantityRequested, @LineItemPrice)

	IF @@ERROR = 1
		BEGIN
			RAISERROR('AddCardToOrderItem: Failed to insert into OrderItems', 16, 1)
			SET @ReturnCode = 1
		END
END
GO
CREATE PROCEDURE AddProductToOrderItem
(
	@OrderID INT NULL,
	@ProductID VARCHAR(200) NULL,
	@QuantityRequested INT NULL,
	@LineItemPrice MONEY NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	IF @OrderID IS NULL
		RAISERROR('AddProductToOrderItem: OrderID cannot be null', 16, 1)
		IF @ProductID IS NULL
			RAISERROR('AddProductToOrderItem: ProductID cannot be null', 16, 1)
			IF @QuantityRequested IS NULL
				RAISERROR('AddProductToOrderItem: QuantityRequested cannot be null', 16, 1)
				IF @LineItemPrice IS NULL
					RAISERROR('AddProductToOrderItem: LineItemPrice cannot be null', 16, 1)
	INSERT
	INTO OrderItems
	(OrderID, ProductID, QuantityRequested, LineItemPrice)
	VALUES
	(@OrderID, @ProductID, @QuantityRequested, @LineItemPrice)

	IF @@ERROR = 1
		BEGIN
			RAISERROR('AddProductToOrderItem: Failed to insert into OrderItems', 16, 1)
			SET @ReturnCode = 1
		END
END
GO
CREATE PROCEDURE UpdateCart
(
	@CartID INT NULL,
	@Quantity INT NULL
	--,@GST MONEY NULL,
	--@SubTotal MONEY NULL,
	--@Total MONEY NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	IF @CartID IS NULL
		RAISERROR('UpdateCart: CartID cannot be null', 16, 1)
		IF @Quantity IS NULL
			RAISERROR('UpdateCart: Quantity cannot be null', 16, 1)
			--IF @GST IS NULL
			--	RAISERROR('UpdateCart: GST cannot be null', 16, 1)
			--	IF @SubTotal IS NULL
			--		RAISERROR('UpdateCart: SubTotal cannot be null', 16, 1)
			--		IF @Total IS NULL
			--			RAISERROR('UpdateCart: Total cannot be null', 16, 1)

	UPDATE Cart
	SET
		Quantity = @Quantity
		
	WHERE @CartID = CartID

	IF @@ERROR = 1
		BEGIN
			RAISERROR('UpdateCart: Failed to update Cart', 16, 1)
			SET @ReturnCode = 1
		END
END
RETURN @ReturnCode
GO
CREATE PROCEDURE CompleteOrder
(
	@OrderID INT NULL,
	@OrderStatus VARCHAR(20) NULL,
	@CompletedDate DATE NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	IF @OrderID IS NULL
		RAISERROR('CompleteOrder: OrderID cannot be null', 16, 1)
		IF @OrderStatus IS NULL
			RAISERROR('CompleteOrder: OrderStatus cannot be null', 16, 1)
			IF @CompletedDate IS NULL
				RAISERROR('CompleteOrder: CompletedDate cannot be null', 16, 1)

	UPDATE Orders
	SET
		OrderStatus = @OrderStatus,
		CompletedDate = @CompletedDate
	WHERE 
		OrderID = @OrderID

	IF @@ERROR = 1
		BEGIN
			RAISERROR('CompleteOrder: Failed to Update Orders', 16, 1)
			SET @ReturnCode = 1
		END
END
RETURN @ReturnCode
GO
CREATE PROCEDURE GetUserByAccountNumber
(
	@UserAccountNumber INT NULL
)
AS
DECLARE @ReturnCode INT
SET @ReturnCode = 0
BEGIN
	IF @UserAccountNumber IS NULL
		RAISERROR('GetUserByAccountNumber: UserAccountNumber cannot be null', 16, 1)
	BEGIN
		SELECT
			FirstName,
			LastName,
			Email,
			UserName
		FROM UserAccount
		WHERE UserAccountNumber = @UserAccountNumber

	IF @@ERROR = 1
		BEGIN
			RAISERROR('GetUserByAccountNumber: Failed to select from GetUserByAccountNumber', 16, 1)
			SET @ReturnCode = 1
		END
END
END
RETURN @ReturnCode