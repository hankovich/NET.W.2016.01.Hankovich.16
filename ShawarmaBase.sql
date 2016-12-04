CREATE TABLE `IngradientCategory` (
	`CategoryId` INT NOT NULL,
	`CategoryName` varchar NOT NULL,
	PRIMARY KEY (`CategoryId`)
);

CREATE TABLE `Ingradient` (
	`IngradientId` INT NOT NULL,
	`IngradientName` varchar NOT NULL,
	`TotalWeight` INT NOT NULL,
	`CategoryId` INT NOT NULL,
	PRIMARY KEY (`IngradientId`)
);

CREATE TABLE `ShawarmaRecipe` (
	`ShawarmaRecipeId` INT NOT NULL,
	`ShawarmaId` INT NOT NULL,
	`IngradientId` INT NOT NULL,
	`Weight` INT NOT NULL,
	PRIMARY KEY (`ShawarmaRecipeId`)
);

CREATE TABLE `Shawarma` (
	`ShawarmaId` INT NOT NULL,
	`ShawarmaName` varchar AUTO_INCREMENT,
	`CookingTime` INT NOT NULL,
	PRIMARY KEY (`ShawarmaId`)
);

CREATE TABLE `OrderHeader` (
	`OrderHeaderId` INT NOT NULL,
	`OrderDate` DATE NOT NULL,
	`SellerId` INT NOT NULL,
	PRIMARY KEY (`OrderHeaderId`)
);

CREATE TABLE `OrderDetails` (
	`OrderHeaderId` INT NOT NULL,
	`ShawarmaId` INT NOT NULL,
	`Quantity` INT NOT NULL
);

CREATE TABLE `PriceController` (
	`PriceControllerId` INT NOT NULL,
	`ShawarmaId` INT NOT NULL,
	`Price` DECIMAL NOT NULL,
	`SellingPointId` INT NOT NULL,
	`Comment` TEXT NOT NULL,
	PRIMARY KEY (`PriceControllerId`)
);

CREATE TABLE `Seller` (
	`SellerId` INT NOT NULL,
	`SellerName` varchar NOT NULL,
	`SellingPointId` VARCHAR(255) NOT NULL,
	PRIMARY KEY (`SellerId`)
);

CREATE TABLE `SellingPoint` (
	`SellingPointId` INT NOT NULL,
	`Address` varchar NOT NULL,
	`SellingPointCategoryId` INT NOT NULL,
	`ShawarmaTitle` varchar NOT NULL,
	PRIMARY KEY (`SellingPointId`)
);

CREATE TABLE `TimeController` (
	`TimeControllerId` INT NOT NULL,
	`SellerId` INT NOT NULL AUTO_INCREMENT,
	`WorkStart` DATETIME NOT NULL,
	`WorkEnd` DATETIME NOT NULL,
	PRIMARY KEY (`TimeControllerId`)
);

CREATE TABLE `SellingPointCategory` (
	`SellingPointCategoryId` INT NOT NULL,
	`SellingPointCategoryName` VARCHAR(255) NOT NULL,
	PRIMARY KEY (`SellingPointCategoryId`)
);

