CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `BerletTipusok` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Nev` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Kategoria` int NOT NULL,
    `Ar` decimal(10,2) NOT NULL,
    `AlapertelmezettAlkalmak` int NULL,
    `Aktiv` tinyint(1) NOT NULL,
    CONSTRAINT `PK_BerletTipusok` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Munkakorok` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Nev` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Munkakorok` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `SzolgaltatasTipusok` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Nev` longtext CHARACTER SET utf8mb4 NOT NULL,
    `AlapertelmezettAr` decimal(10,2) NOT NULL,
    `Aktiv` tinyint(1) NOT NULL,
    CONSTRAINT `PK_SzolgaltatasTipusok` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Tagok` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Vezeteknev` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Keresztnev` longtext CHARACTER SET utf8mb4 NOT NULL,
    `SzuletesiDatum` date NOT NULL,
    `Cim` longtext CHARACTER SET utf8mb4 NULL,
    `Telefon` longtext CHARACTER SET utf8mb4 NULL,
    `Email` longtext CHARACTER SET utf8mb4 NULL,
    `RegisztracioDatuma` date NOT NULL,
    `VhKapcsolattartoNev` longtext CHARACTER SET utf8mb4 NULL,
    `VhKapcsolattartoTelefon` longtext CHARACTER SET utf8mb4 NULL,
    `Nem` int NOT NULL,
    `Megjegyzes` longtext CHARACTER SET utf8mb4 NULL,
    `Aktiv` tinyint(1) NOT NULL,
    CONSTRAINT `PK_Tagok` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Dolgozok` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Vezeteknev` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Keresztnev` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Becenev` longtext CHARACTER SET utf8mb4 NULL,
    `MunkakorId` int NOT NULL,
    `SzemelyiSzam` longtext CHARACTER SET utf8mb4 NULL,
    `TajSzam` longtext CHARACTER SET utf8mb4 NULL,
    `Adoszam` longtext CHARACTER SET utf8mb4 NULL,
    `Telefon` longtext CHARACTER SET utf8mb4 NULL,
    `Email` longtext CHARACTER SET utf8mb4 NULL,
    `SzuletesiDatum` date NOT NULL,
    `Nem` int NOT NULL,
    `FenykepUrl` longtext CHARACTER SET utf8mb4 NULL,
    `PortfolioUrl` longtext CHARACTER SET utf8mb4 NULL,
    `Oradij` decimal(10,2) NULL,
    `Megjegyzes` longtext CHARACTER SET utf8mb4 NULL,
    `Aktiv` tinyint(1) NOT NULL,
    `Letrehozva` datetime(6) NOT NULL,
    `Modositva` datetime(6) NULL,
    CONSTRAINT `PK_Dolgozok` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Dolgozok_Munkakorok_MunkakorId` FOREIGN KEY (`MunkakorId`) REFERENCES `Munkakorok` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Edzok` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `DolgozoId` int NOT NULL,
    `Szakterulet` longtext CHARACTER SET utf8mb4 NULL,
    `Vegzettseg` longtext CHARACTER SET utf8mb4 NULL,
    `Oradij` decimal(10,2) NULL,
    CONSTRAINT `PK_Edzok` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Edzok_Dolgozok_DolgozoId` FOREIGN KEY (`DolgozoId`) REFERENCES `Dolgozok` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Felhasznalok` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Felhasznalonev` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `JelszoHash` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Szerepkor` int NOT NULL,
    `DolgozoId` int NULL,
    `TagId` int NULL,
    `Aktiv` tinyint(1) NOT NULL,
    `JelszoIdeiglenes` tinyint(1) NOT NULL,
    `UtolsoBejelentkezes` datetime(6) NULL,
    CONSTRAINT `PK_Felhasznalok` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Felhasznalok_Dolgozok_DolgozoId` FOREIGN KEY (`DolgozoId`) REFERENCES `Dolgozok` (`Id`),
    CONSTRAINT `FK_Felhasznalok_Tagok_TagId` FOREIGN KEY (`TagId`) REFERENCES `Tagok` (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Munkarendek` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `DolgozoId` int NOT NULL,
    `Datum` date NOT NULL,
    `TolIdo` time(6) NOT NULL,
    `IgIdo` time(6) NOT NULL,
    `Megjegyzes` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Munkarendek` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Munkarendek_Dolgozok_DolgozoId` FOREIGN KEY (`DolgozoId`) REFERENCES `Dolgozok` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `EdzoElerhetosegek` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `EdzoId` int NOT NULL,
    `HetNapja` int NOT NULL,
    `TolIdo` time(6) NOT NULL,
    `IgIdo` time(6) NOT NULL,
    CONSTRAINT `PK_EdzoElerhetosegek` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_EdzoElerhetosegek_Edzok_EdzoId` FOREIGN KEY (`EdzoId`) REFERENCES `Edzok` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Berletek` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `TagId` int NOT NULL,
    `BerletTipusId` int NOT NULL,
    `KezdoDatum` date NOT NULL,
    `LejaratiDatum` date NOT NULL,
    `ArFizetve` decimal(10,2) NOT NULL,
    `HatralevoAlkalmak` int NULL,
    `Statusz` int NOT NULL,
    `LetrehozvaAltalId` int NOT NULL,
    CONSTRAINT `PK_Berletek` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Berletek_BerletTipusok_BerletTipusId` FOREIGN KEY (`BerletTipusId`) REFERENCES `BerletTipusok` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Berletek_Felhasznalok_LetrehozvaAltalId` FOREIGN KEY (`LetrehozvaAltalId`) REFERENCES `Felhasznalok` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Berletek_Tagok_TagId` FOREIGN KEY (`TagId`) REFERENCES `Tagok` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `CsoportosFoglalkozasok` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Megnevezes` longtext CHARACTER SET utf8mb4 NOT NULL,
    `EdzoId` int NOT NULL,
    `Idopont` datetime(6) NOT NULL,
    `Helyszin` longtext CHARACTER SET utf8mb4 NOT NULL,
    `MaxLetszam` int NOT NULL,
    `Statusz` int NOT NULL,
    `MeghirdetteId` int NOT NULL,
    `JovahagytaId` int NULL,
    `JovahagyasDatuma` datetime(6) NULL,
    `LezarasDatuma` datetime(6) NULL,
    `ElutasitasIndoka` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_CsoportosFoglalkozasok` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_CsoportosFoglalkozasok_Edzok_EdzoId` FOREIGN KEY (`EdzoId`) REFERENCES `Edzok` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_CsoportosFoglalkozasok_Felhasznalok_JovahagytaId` FOREIGN KEY (`JovahagytaId`) REFERENCES `Felhasznalok` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_CsoportosFoglalkozasok_Felhasznalok_MeghirdetteId` FOREIGN KEY (`MeghirdetteId`) REFERENCES `Felhasznalok` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `EgyeniFoglalkozasok` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `SzolgaltatasTipusId` int NOT NULL,
    `DolgozoId` int NOT NULL,
    `TagId` int NOT NULL,
    `Idopont` datetime(6) NOT NULL,
    `Ar` decimal(10,2) NOT NULL,
    `Allapot` int NOT NULL,
    `RogzitveAltalId` int NOT NULL,
    `Megjegyzes` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_EgyeniFoglalkozasok` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_EgyeniFoglalkozasok_Dolgozok_DolgozoId` FOREIGN KEY (`DolgozoId`) REFERENCES `Dolgozok` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_EgyeniFoglalkozasok_Felhasznalok_RogzitveAltalId` FOREIGN KEY (`RogzitveAltalId`) REFERENCES `Felhasznalok` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_EgyeniFoglalkozasok_SzolgaltatasTipusok_SzolgaltatasTipusId` FOREIGN KEY (`SzolgaltatasTipusId`) REFERENCES `SzolgaltatasTipusok` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_EgyeniFoglalkozasok_Tagok_TagId` FOREIGN KEY (`TagId`) REFERENCES `Tagok` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `CsoportosJelentkezesek` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `CsoportosFoglalkozasId` int NOT NULL,
    `TagId` int NOT NULL,
    `JelentkezesDatuma` datetime(6) NOT NULL,
    `RogzitveAltalId` int NOT NULL,
    `JelenVolt` tinyint(1) NULL,
    CONSTRAINT `PK_CsoportosJelentkezesek` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_CsoportosJelentkezesek_CsoportosFoglalkozasok_CsoportosFogla~` FOREIGN KEY (`CsoportosFoglalkozasId`) REFERENCES `CsoportosFoglalkozasok` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_CsoportosJelentkezesek_Felhasznalok_RogzitveAltalId` FOREIGN KEY (`RogzitveAltalId`) REFERENCES `Felhasznalok` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_CsoportosJelentkezesek_Tagok_TagId` FOREIGN KEY (`TagId`) REFERENCES `Tagok` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_Berletek_BerletTipusId` ON `Berletek` (`BerletTipusId`);

CREATE INDEX `IX_Berletek_LetrehozvaAltalId` ON `Berletek` (`LetrehozvaAltalId`);

CREATE INDEX `IX_Berletek_TagId` ON `Berletek` (`TagId`);

CREATE INDEX `IX_CsoportosFoglalkozasok_EdzoId` ON `CsoportosFoglalkozasok` (`EdzoId`);

CREATE INDEX `IX_CsoportosFoglalkozasok_JovahagytaId` ON `CsoportosFoglalkozasok` (`JovahagytaId`);

CREATE INDEX `IX_CsoportosFoglalkozasok_MeghirdetteId` ON `CsoportosFoglalkozasok` (`MeghirdetteId`);

CREATE UNIQUE INDEX `IX_CsoportosJelentkezesek_CsoportosFoglalkozasId_TagId` ON `CsoportosJelentkezesek` (`CsoportosFoglalkozasId`, `TagId`);

CREATE INDEX `IX_CsoportosJelentkezesek_RogzitveAltalId` ON `CsoportosJelentkezesek` (`RogzitveAltalId`);

CREATE INDEX `IX_CsoportosJelentkezesek_TagId` ON `CsoportosJelentkezesek` (`TagId`);

CREATE INDEX `IX_Dolgozok_MunkakorId` ON `Dolgozok` (`MunkakorId`);

CREATE INDEX `IX_EdzoElerhetosegek_EdzoId` ON `EdzoElerhetosegek` (`EdzoId`);

CREATE UNIQUE INDEX `IX_Edzok_DolgozoId` ON `Edzok` (`DolgozoId`);

CREATE INDEX `IX_EgyeniFoglalkozasok_DolgozoId` ON `EgyeniFoglalkozasok` (`DolgozoId`);

CREATE INDEX `IX_EgyeniFoglalkozasok_RogzitveAltalId` ON `EgyeniFoglalkozasok` (`RogzitveAltalId`);

CREATE INDEX `IX_EgyeniFoglalkozasok_SzolgaltatasTipusId` ON `EgyeniFoglalkozasok` (`SzolgaltatasTipusId`);

CREATE INDEX `IX_EgyeniFoglalkozasok_TagId` ON `EgyeniFoglalkozasok` (`TagId`);

CREATE UNIQUE INDEX `IX_Felhasznalok_DolgozoId` ON `Felhasznalok` (`DolgozoId`);

CREATE UNIQUE INDEX `IX_Felhasznalok_Felhasznalonev` ON `Felhasznalok` (`Felhasznalonev`);

CREATE UNIQUE INDEX `IX_Felhasznalok_TagId` ON `Felhasznalok` (`TagId`);

CREATE INDEX `IX_Munkarendek_DolgozoId` ON `Munkarendek` (`DolgozoId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20260920121508_InitialCreate', '8.0.2');

COMMIT;

