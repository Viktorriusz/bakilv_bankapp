-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2026. Okt 22. 10:12
-- Kiszolgáló verziója: 10.4.32-MariaDB
-- PHP verzió: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `crackbank`
--

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `felhasznalok`
--

CREATE TABLE `felhasznalok` (
  `id` int(11) NOT NULL,
  `name` varchar(30) NOT NULL,
  `egyenleg` double DEFAULT NULL,
  `password` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `felhasznalok`
--

INSERT INTO `felhasznalok` (`id`, `name`, `egyenleg`, `password`) VALUES
(1, 'admin', 100000, 'gdyb21LQTcIANtvYMT7QVQ=='),
(2, 'admin2', 140000, 'gdyb21LQTcIANtvYMT7QVQ==');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `tranzakciok`
--

CREATE TABLE `tranzakciok` (
  `id` int(11) NOT NULL,
  `felhasznaloid` int(11) NOT NULL,
  `valtozas` double NOT NULL,
  `date` datetime NOT NULL,
  `egyenleg` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `tranzakciok`
--

INSERT INTO `tranzakciok` (`id`, `felhasznaloid`, `valtozas`, `date`, `egyenleg`) VALUES
(38, 1, 50000, '2026-10-22 00:00:00', 50000),
(39, 1, -10000, '2026-10-22 00:00:00', 40000),
(40, 1, -25000, '2026-10-22 00:00:00', 15000),
(41, 2, 25000, '2026-10-22 00:00:00', 25000),
(42, 1, 50000, '2026-10-22 00:00:00', 65000),
(43, 1, 50000, '2026-10-22 00:00:00', 115000),
(44, 1, 50000, '2026-10-22 00:00:00', 165000),
(45, 1, -50000, '2026-10-22 00:00:00', 115000),
(46, 2, 50000, '2026-10-22 00:00:00', 75000),
(47, 1, 50000, '2026-10-22 00:00:00', 165000),
(48, 1, -15000, '2026-10-22 00:00:00', 150000),
(49, 2, 15000, '2026-10-22 00:00:00', 90000),
(50, 1, -50000, '2026-10-22 10:05:18', 100000),
(51, 2, 50000, '2026-10-22 10:05:18', 140000);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `utalasok`
--

CREATE TABLE `utalasok` (
  `id` int(11) NOT NULL,
  `felhasznaloid1` int(11) NOT NULL,
  `felhasznaloid2` int(11) NOT NULL,
  `osszeg` double NOT NULL,
  `datum` date NOT NULL,
  `felh1egyenleg` double NOT NULL,
  `felh2egyenleg` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `utalasok`
--

INSERT INTO `utalasok` (`id`, `felhasznaloid1`, `felhasznaloid2`, `osszeg`, `datum`, `felh1egyenleg`, `felh2egyenleg`) VALUES
(4, 1, 2, 25000, '2026-10-22', 15000, 25000),
(5, 1, 2, 50000, '2026-10-22', 115000, 75000),
(6, 1, 2, 15000, '2026-10-22', 150000, 90000),
(7, 1, 2, 50000, '2026-10-22', 100000, 140000);

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `felhasznalok`
--
ALTER TABLE `felhasznalok`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `tranzakciok`
--
ALTER TABLE `tranzakciok`
  ADD PRIMARY KEY (`id`),
  ADD KEY `felhasznaloid` (`felhasznaloid`);

--
-- A tábla indexei `utalasok`
--
ALTER TABLE `utalasok`
  ADD PRIMARY KEY (`id`),
  ADD KEY `felhasznaloid1` (`felhasznaloid1`,`felhasznaloid2`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `felhasznalok`
--
ALTER TABLE `felhasznalok`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT a táblához `tranzakciok`
--
ALTER TABLE `tranzakciok`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=52;

--
-- AUTO_INCREMENT a táblához `utalasok`
--
ALTER TABLE `utalasok`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `tranzakciok`
--
ALTER TABLE `tranzakciok`
  ADD CONSTRAINT `fk_tranzakciok_felhasznalok` FOREIGN KEY (`felhasznaloid`) REFERENCES `felhasznalok` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
