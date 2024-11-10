-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 10-11-2024 a las 07:20:58
-- Versión del servidor: 10.4.28-MariaDB
-- Versión de PHP: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `apiinmobiliaria`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos`
--

CREATE TABLE `contratos` (
  `IdContrato` int(11) NOT NULL,
  `Precio` double NOT NULL,
  `FechaInicio` datetime NOT NULL,
  `FechaFin` datetime NOT NULL,
  `IdInquilino` int(11) NOT NULL,
  `IdInmueble` int(11) NOT NULL,
  `FechaTerminacion` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contratos`
--

INSERT INTO `contratos` (`IdContrato`, `Precio`, `FechaInicio`, `FechaFin`, `IdInquilino`, `IdInmueble`, `FechaTerminacion`) VALUES
(1, 250000, '2024-10-27 06:54:15', '2027-10-27 06:54:15', 1, 2, NULL),
(2, 240000, '2024-10-30 21:55:22', '2026-10-30 21:55:22', 2, 1, NULL),
(3, 175000, '2024-10-30 21:56:17', '2025-03-12 21:56:17', 3, 10, NULL),
(4, 980000, '2024-10-30 21:58:05', '2025-10-30 21:58:05', 4, 11, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `IdInmueble` int(11) NOT NULL,
  `Direccion` varchar(40) NOT NULL,
  `Ambientes` int(4) NOT NULL,
  `Tipo` varchar(15) NOT NULL,
  `Uso` varchar(15) NOT NULL,
  `Precio` double NOT NULL,
  `Disponible` tinyint(1) NOT NULL DEFAULT 0,
  `Avatar` varchar(255) DEFAULT NULL,
  `IdPropietario` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`IdInmueble`, `Direccion`, `Ambientes`, `Tipo`, `Uso`, `Precio`, `Disponible`, `Avatar`, `IdPropietario`) VALUES
(1, 'Calle 21 entre 73 y 74', 2, 'casa', 'residencial', 230000, 1, NULL, 19),
(2, 'Calle 9 entre 65 y 66 ', 3, 'casa', 'residencial', 200000, 0, '/imgInmuebles/casa.jpg', 19),
(10, 'Justo Daract 1547', 3, 'departamento', 'residencial', 150000, 1, '/imgInmuebles/10_0a3f1888-97c5-4dde-b45f-907da9af78bd.jpg', 19),
(11, 'San Martin 456', 5, 'casa', 'residencial', 980000, 0, '/imgInmuebles/11_fcccf57f-40cc-4331-ab2b-918dfef515f6.jpg', 19),
(12, 'calle 3', 3, 'casa', 'residencial', 200234, 1, '/imgInmuebles/12_cc435394-85bf-4682-bb98-c4fd43501712.jpg', 19),
(13, 'calle 234', 2, 'depto', 'residencial', 123456, 1, '/imgInmuebles/13_94e979ef-a960-4545-a125-8d8d967a8a41.jpg', 19),
(14, 'calle 23', 4, 'casa', 'residencial', 123456, 1, '/imgInmuebles/14_a74fe0aa-1702-4b36-86cf-a050aef74967.jpg', 19);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `IdInquilino` int(11) NOT NULL,
  `Dni` int(11) NOT NULL,
  `Apellido` varchar(40) NOT NULL,
  `Nombre` varchar(40) NOT NULL,
  `Telefono` varchar(30) DEFAULT NULL,
  `NombreGarante` varchar(40) NOT NULL,
  `ApellidoGarante` varchar(40) NOT NULL,
  `TelefonoGarante` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`IdInquilino`, `Dni`, `Apellido`, `Nombre`, `Telefono`, `NombreGarante`, `ApellidoGarante`, `TelefonoGarante`) VALUES
(1, 12345, 'Lopez', 'Juan', '0303456', 'Luis', 'Sosa', '4560303'),
(2, 123123, 'Gonzales', 'Matias', '266429292', 'Juan', 'Sosa', '266432323'),
(3, 5344634, 'Torrez', 'Gabriel', '266439393', 'German', 'Torrez', '26643443'),
(4, 46533, 'Torrez', 'German', '26643432', 'Gabriel', 'Torrez', '26648383');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `IdPago` int(11) NOT NULL,
  `NroPago` int(11) NOT NULL,
  `IdContrato` int(11) NOT NULL,
  `Fecha` datetime NOT NULL,
  `Importe` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pagos`
--

INSERT INTO `pagos` (`IdPago`, `NroPago`, `IdContrato`, `Fecha`, `Importe`) VALUES
(1, 1, 1, '2024-04-28 23:40:33', 120000),
(2, 2, 1, '2024-05-28 23:41:25', 120000),
(3, 3, 1, '2024-06-28 23:41:46', 120000),
(4, 4, 1, '2024-07-30 22:00:02', 120000),
(5, 5, 1, '2024-08-30 22:00:02', 120000),
(6, 6, 1, '2024-09-30 22:00:40', 120000),
(7, 7, 1, '2024-10-30 22:00:40', 120000),
(8, 1, 2, '2024-06-30 22:01:01', 240000),
(9, 2, 2, '2024-07-30 22:01:01', 240000),
(10, 3, 2, '2024-08-30 22:01:46', 240000),
(11, 4, 2, '2024-09-30 22:01:46', 240000),
(12, 5, 2, '2024-10-30 22:02:08', 240000),
(13, 6, 3, '0000-00-00 00:00:00', 240000),
(14, 1, 3, '2024-03-30 22:02:26', 175000),
(15, 2, 3, '2024-04-30 22:02:26', 175000),
(16, 3, 3, '2024-05-30 22:02:53', 175000),
(17, 4, 3, '2024-06-30 22:02:53', 175000),
(18, 5, 3, '2024-07-30 22:03:11', 175000),
(19, 6, 3, '2024-08-30 22:03:11', 175000),
(20, 7, 3, '2024-09-30 22:03:26', 175000),
(21, 8, 3, '2024-10-30 22:03:26', 175000),
(22, 1, 4, '2024-07-30 22:03:47', 980000),
(23, 2, 4, '2024-08-30 22:03:47', 980000),
(24, 3, 4, '2024-09-30 22:04:08', 980000),
(25, 4, 4, '2024-10-30 22:04:09', 980000),
(26, 5, 4, '2024-10-30 22:04:27', 980000),
(27, 6, 4, '2024-10-30 22:04:27', 980000),
(28, 7, 4, '2024-10-30 22:04:46', 980000),
(29, 8, 0, '2024-10-30 22:04:46', 980000);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `IdPropietario` int(11) NOT NULL,
  `Dni` varchar(20) NOT NULL,
  `Apellido` varchar(50) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Telefono` varchar(15) NOT NULL,
  `Estado` tinyint(1) NOT NULL DEFAULT 1,
  `Email` varchar(100) NOT NULL,
  `Clave` varchar(255) NOT NULL,
  `Avatar` varchar(255) DEFAULT '/avatars/sinfoto.PNG'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`IdPropietario`, `Dni`, `Apellido`, `Nombre`, `Telefono`, `Estado`, `Email`, `Clave`, `Avatar`) VALUES
(10, '1234534343', 'Young', 'Angus', '', 1, 'admin2@gmail.com', 'ivPQC5+MPSB7tWE+QTdrOTf3B6I9c41qlX7bfL0f/9g=', NULL),
(19, '36018434', 'Torrez', 'Gabriel', '26642323', 0, 'gabrielt9303@gmail.com', 'DBO8Ef5IVGM6Kxtvz/gNhOrhLC9jxBS18cR/BxZ3pnc=', '/avatars/19_Gabriellll_Torrezzz_23e48d2d-f475-4df4-b054-fef5c45e945e.jpg');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`IdContrato`),
  ADD KEY `IdInquilino` (`IdInquilino`),
  ADD KEY `IdInmueble` (`IdInmueble`);

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`IdInmueble`),
  ADD KEY `FK_Propietario` (`IdPropietario`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`IdInquilino`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`IdPago`),
  ADD KEY `IdAlquiler` (`IdContrato`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`IdPropietario`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contratos`
--
ALTER TABLE `contratos`
  MODIFY `IdContrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `IdInmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `IdInquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `IdPago` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=30;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `IdPropietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD CONSTRAINT `contratos_ibfk_1` FOREIGN KEY (`IdInquilino`) REFERENCES `inquilinos` (`IdInquilino`),
  ADD CONSTRAINT `contratos_ibfk_2` FOREIGN KEY (`IdInmueble`) REFERENCES `inmuebles` (`IdInmueble`);

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `FK_Propietario` FOREIGN KEY (`IdPropietario`) REFERENCES `propietarios` (`IdPropietario`);

--
-- Filtros para la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD CONSTRAINT `pagos_ibfk_1` FOREIGN KEY (`IdContrato`) REFERENCES `contratos` (`IdContrato`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
