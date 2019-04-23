-- --------------------------------------------------------
-- Servidor:                     127.0.0.1
-- Versão do servidor:           8.0.13 - MySQL Community Server - GPL
-- OS do Servidor:               Win64
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;  

-- Estrutura para tabela webapi.cliente`
CREATE TABLE IF NOT EXISTS `empresa` (
  `EmpresaId` int(11) NOT NULL AUTO_INCREMENT,
  `Nome` varchar(50) COLLATE utf8_bin NOT NULL,
  `Email` varchar(90) COLLATE utf8_bin NULL,
  `Descricao` varchar(50) COLLATE utf8_bin NULL,
  `Endereco` varchar(120) COLLATE utf8_bin NULL,
  `Numero` varchar(15) COLLATE utf8_bin NULL,
  `Bairro` varchar(120) COLLATE utf8_bin NULL,
  `Cidade` varchar(120) COLLATE utf8_bin NULL,
  `Cep` varchar(45) COLLATE utf8_bin NULL,
  `Telefone` varchar(45) COLLATE utf8_bin NULL,
  `Celular` varchar(45) COLLATE utf8_bin NULL,
  `Logo` varchar(120) COLLATE utf8_bin NULL,
  `Imagem` varchar(120) COLLATE utf8_bin NULL,
  `Ativo` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`EmpresaId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- Estrutura para tabela webapi.usuario
CREATE TABLE IF NOT EXISTS `usuario` (
  `UsuarioId` int(11) NOT NULL AUTO_INCREMENT,
  `EmpresaId` int(11) NOT NULL,
  `Nome` varchar(50) COLLATE utf8_bin NOT NULL,
  `Email` varchar(90) COLLATE utf8_bin NULL,
  `Login` varchar(50) COLLATE utf8_bin NOT NULL,
  `Senha` varchar(50) COLLATE utf8_bin NOT NULL,
  `Imagem` varchar(120) COLLATE utf8_bin NULL,
  `Token` varchar(260) CHARACTER SET utf8 COLLATE utf8_bin DEFAULT NULL,
  `Administrativo` int(11) NOT NULL,
  `Ativo` int(11) NOT NULL DEFAULT 1,
  PRIMARY KEY (`UsuarioId`),
  KEY `EmpresaId` (`EmpresaId`),
  CONSTRAINT `FK_usuario_empresa` FOREIGN KEY (`EmpresaId`) REFERENCES `empresa` (`EmpresaId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- Estrutura para tabela webapi.logsistemaerro
CREATE TABLE IF NOT EXISTS `logsistemaerro` (
  `LogSistemaErroId` int(11) NOT NULL AUTO_INCREMENT,
  `EmpresaId` int(11) NOT NULL,
  `UsuarioId` int(11) NOT NULL,
  `Mensagem` varchar(250) COLLATE utf8_bin NOT NULL,
  `Erro` text COLLATE utf8_bin NOT NULL,
  `DataHora` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Resolvido` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`LogSistemaErroId`),
  KEY `EmpresaId` (`EmpresaId`),
  CONSTRAINT `FK_logsistemaerro_empresa` FOREIGN KEY (`EmpresaId`) REFERENCES `empresa` (`EmpresaId`),
  KEY `UsuarioId` (`UsuarioId`),
  CONSTRAINT `FK_logsistemaerro_usuario` FOREIGN KEY (`UsuarioId`) REFERENCES `usuario` (`usuarioid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- Estrutura para tabela webapi.acao
CREATE TABLE IF NOT EXISTS `acao` (
  `AcaoId` int(11) NOT NULL AUTO_INCREMENT,
  `ControladorId` int(11) NOT NULL,
  `Nome` varchar(50) COLLATE utf8_bin NOT NULL,
  `NomeTela` varchar(100) COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`AcaoId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- Estrutura para tabela webapi.controlador
CREATE TABLE IF NOT EXISTS `controlador` (
  `ControladorId` int(11) NOT NULL AUTO_INCREMENT,
  `NomeTela` varchar(100) COLLATE utf8_bin NOT NULL DEFAULT '0',
  `Nome` varchar(50) COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`ControladorId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- Estrutura para tabela webapi.logsistema
CREATE TABLE IF NOT EXISTS `logsistema` (
  `LogSistemaId` int(11) NOT NULL AUTO_INCREMENT,
  `EmpresaId` int(11) NOT NULL,
  `UsuarioId` int(11) NOT NULL,
  `Acao` int(11) NOT NULL,
  `DataHora` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Dados` text COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`LogSistemaId`),
  KEY `FK_logsistema_usuario` (`UsuarioId`),
  KEY `FK_logsistema_empresa` (`EmpresaId`),
  CONSTRAINT `FK_logsistema_empresa` FOREIGN KEY (`EmpresaId`) REFERENCES `empresa` (`EmpresaId`),
  CONSTRAINT `FK_logsistema_usuario` FOREIGN KEY (`UsuarioId`) REFERENCES `usuario` (`UsuarioId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- Estrutura para tabela webapi.logsistemaerro
CREATE TABLE IF NOT EXISTS `logsistemaerro` (
  `LogSistemaErro` int(11) NOT NULL AUTO_INCREMENT,
  `UsuarioId` int(11) NOT NULL,
  `Mensagem` varchar(250) COLLATE utf8_bin NOT NULL,
  `Erro` text COLLATE utf8_bin NOT NULL,
  `DataHora` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Resolvido` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`LogSistemaErro`),
  KEY `UsuarioId` (`UsuarioId`),
  CONSTRAINT `FK_logsistemaerro_usuario` FOREIGN KEY (`UsuarioId`) REFERENCES `usuario` (`UsuarioId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- Estrutura para tabela webapi.permissaousuario
CREATE TABLE IF NOT EXISTS `permissaousuario` (
  `PermissaoUsuarioId` int(11) NOT NULL AUTO_INCREMENT,
  `UsuarioId` int(11) NOT NULL,
  `AcaoId` int(11) NOT NULL,
  PRIMARY KEY (`PermissaoUsuarioId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- Exportação de dados foi desmarcado.
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
