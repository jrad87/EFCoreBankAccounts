-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema efcore_bank_accounts_db
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema efcore_bank_accounts_db
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `efcore_bank_accounts_db` DEFAULT CHARACTER SET utf8 ;
USE `efcore_bank_accounts_db` ;

-- -----------------------------------------------------
-- Table `efcore_bank_accounts_db`.`Accounts`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `efcore_bank_accounts_db`.`Accounts` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `CurrentBalance` INT NULL,
  `CreatedAt` DATETIME NULL,
  `UpdatedAt` DATETIME NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `efcore_bank_accounts_db`.`Users`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `efcore_bank_accounts_db`.`Users` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `FirstName` VARCHAR(255) NULL,
  `LastName` VARCHAR(255) NULL,
  `Email` VARCHAR(255) NULL,
  `Password` VARCHAR(255) NULL,
  `CreatedAt` DATETIME NULL,
  `UpdatedAt` DATETIME NULL,
  `AccountId` INT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_Users_Accounts1_idx` (`AccountId` ASC) VISIBLE,
  CONSTRAINT `fk_Users_Accounts1`
    FOREIGN KEY (`AccountId`)
    REFERENCES `efcore_bank_accounts_db`.`Accounts` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `efcore_bank_accounts_db`.`Transactions`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `efcore_bank_accounts_db`.`Transactions` (
  `Id` INT NOT NULL,
  `Amount` INT NULL,
  `BalanceBefore` INT NULL,
  `BalanceAfter` INT NULL,
  `CreatedAt` DATETIME NULL,
  `UpdatedAt` DATETIME NULL,
  `AccountId` INT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_Transactions_Accounts1_idx` (`AccountId` ASC) VISIBLE,
  CONSTRAINT `fk_Transactions_Accounts1`
    FOREIGN KEY (`AccountId`)
    REFERENCES `efcore_bank_accounts_db`.`Accounts` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
