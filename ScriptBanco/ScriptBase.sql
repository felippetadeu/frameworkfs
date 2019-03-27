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


-- Criação do banco de dados para rswebapi
CREATE DATABASE IF NOT EXISTS `rswebapi`;
USE `rswebapi`;

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
  `Ativo` int(11) NOT NULL DEFAULT '0',
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

-- Estrutura para tabela webapi.postcategoria
CREATE TABLE IF NOT EXISTS `postcategoria` (
  `PostCategoriaId` int(11) NOT NULL AUTO_INCREMENT,
  `EmpresaId` int(11) NOT NULL,
  `Nome` varchar(90) COLLATE utf8_bin NOT NULL,
  `Imagem` varchar(120) COLLATE utf8_bin NULL,
  `Ativo` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`PostCategoriaId`),
  KEY `EmpresaId` (`EmpresaId`),
  CONSTRAINT `FK_postcategoria_empresa` FOREIGN KEY (`EmpresaId`) REFERENCES `empresa` (`EmpresaId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- Estrutura para tabela webapi.post
CREATE TABLE IF NOT EXISTS `post` (
  `PostId` int(11) NOT NULL AUTO_INCREMENT,
  `EmpresaId` int(11) NOT NULL,
  `UsuarioId` int(11) NOT NULL,
  `PostCategoriaId` int(11) NOT NULL,
  `Titulo` varchar(120) COLLATE utf8_bin NOT NULL,
  `Descricao` text COLLATE utf8_bin NOT NULL,
  `Imagem` varchar(120) COLLATE utf8_bin NULL,
  `DataHora` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `PostUrlAmigavel` varchar(120) COLLATE utf8_bin NULL,
  `Ativo` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`PostId`),
  KEY `EmpresaId` (`EmpresaId`),
  CONSTRAINT `FK_post_empresa` FOREIGN KEY (`EmpresaId`) REFERENCES `empresa` (`EmpresaId`),
  KEY `PostCategoriaId` (`PostCategoriaId`),
  CONSTRAINT `FK_post_postcategoria` FOREIGN KEY (`PostCategoriaId`) REFERENCES `postcategoria` (`postCategoriaid`),
  KEY `UsuarioId` (`UsuarioId`),
  CONSTRAINT `FK_post_usuario` FOREIGN KEY (`UsuarioId`) REFERENCES `usuario` (`usuarioid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- Estrutura para tabela webapi.postgaleria
CREATE TABLE IF NOT EXISTS `postgaleria` (
  `PostGaleriaId` int(11) NOT NULL AUTO_INCREMENT,
  `EmpresaId` int(11) NOT NULL,
  `PostId` int(11) NOT NULL,
  `Imagem` varchar(120) COLLATE utf8_bin NULL,
  `DataHora` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Ativo` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`PostGaleriaId`),
  KEY `EmpresaId` (`EmpresaId`),
  CONSTRAINT `FK_postgaleria_empresa` FOREIGN KEY (`EmpresaId`) REFERENCES `empresa` (`EmpresaId`),
  KEY `PostId` (`PostId`),
  CONSTRAINT `FK_postgaleria_post` FOREIGN KEY (`PostId`) REFERENCES `post` (`postid`)
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

-- Estrutura para tabela webapi.newsletter
CREATE TABLE IF NOT EXISTS `newsletter` (
  `NewsletterId` int(11) NOT NULL AUTO_INCREMENT,
  `EmpresaId` int(11) NOT NULL,
  `Email` varchar(90) COLLATE utf8_bin NOT NULL,
  `Ativo` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`NewsletterId`),
  KEY `NewsletterId` (`NewsletterId`),
  CONSTRAINT `FK_newsletter_empresa` FOREIGN KEY (`EmpresaId`) REFERENCES `empresa` (`EmpresaId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- Estrutura para tabela webapi.anuncio
CREATE TABLE IF NOT EXISTS `anuncio` (
  `AnuncioId` int(11) NOT NULL AUTO_INCREMENT,
  `EmpresaId` int(11) NOT NULL,
  `UsuarioId` int(11) NOT NULL,
  `Titulo` varchar(90) COLLATE utf8_bin NOT NULL,
  `Descricao` text COLLATE utf8_bin NOT NULL,
  `Preco` decimal(2,10) NOT NULL,
  `PrecoPromocao` decimal(2,10) NULL,
  `Imagem` varchar(120) COLLATE utf8_bin NULL,
  `DataHora` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `DataHoraInicio` datetime NOT NULL,
  `DataHoraFim` datetime NOT NULL,
  `Ativo` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`AnuncioId`),
  KEY `EmpresaId` (`EmpresaId`),
  CONSTRAINT `FK_anuncio_empresa` FOREIGN KEY (`EmpresaId`) REFERENCES `empresa` (`EmpresaId`),
  KEY `UsuarioId` (`UsuarioId`),
  CONSTRAINT `FK_anuncio_usuario` FOREIGN KEY (`UsuarioId`) REFERENCES `usuario` (`usuarioid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;


INSERT INTO `empresa` (`EmpresaId`, `Nome`, `Email`, `Descricao`, `Endereco`, `Numero`, `Bairro`, `Cidade`, `Cep`, `Telefone`, `Celular`, `Logo`, `Imagem`, `Ativo`) VALUES
(1, 'Winmed', 'desenvolvimento@winmed.com.br', 'Empresa Winmed', 'Rua Independência', '3742', 'Vila Santa Cruz', 'São José do Rio Preto', '15014400', NULL, NULL, NULL, NULL, 1);

INSERT INTO `usuario` (`UsuarioId`, `EmpresaId`, `Nome`, `Email`, `Login`, `Senha`, `Imagem`, `Token`, `Administrativo`, `Ativo`) VALUES
(1, 1, 'Rodrigo Santos', 'rodrigo@winmed.com.br', 'rodrigo', '4cd51e51a4225287c8bb2fa0eb4343e1', NULL, 'bu3-IQPWAuWE2X1UGBcxygFWQLSg_KB17uulkSNixwGA4MngKeamBhl_LUEtXMzHNGisSqx3xozFyprns_WErYiF6Ogegol1zukqGesZ03mjLDXWmG0wuOH319YEUrirxA37FlUh4UOkNY5zgB1mqfWM_4RRwFyxHl_N8Ukk0xvpaEQI84G57708I4Xg5LQHqoG5oVXxVAWBULxuUIPkDBT-dwOSiyErsE-_84G7VkM', 1, 1),
(2, 1, 'Felippe Tadeu', 'felippe@winmed.com.br', 'felippe', '14e1b600b1fd579f47433b88e8d85291', NULL, NULL, 1, 1),
(3, 1, 'Alessandra Igrissis', 'alessandra@winmed.com.br', 'alessandra', '14e1b600b1fd579f47433b88e8d85291', NULL, NULL, 1, 1);

INSERT INTO `controlador` (`ControladorId`, `NomeTela`, `Nome`) VALUES
(1, 'Crud Usuário', 'Usuario'),
(2, 'Crud Post', 'Post'),
(3, 'Crud Categoria', 'Categoria'),
(4, 'Crud Newsletter', 'Newsletter'),
(5, 'Crud Anuncio', 'Anuncio'),
(6, 'Crud Galeria', 'Galeria');

INSERT INTO `acao` (`ControladorId`, `Nome`, `NomeTela`) VALUES
(1, 'IdentityInsert', 'Inserir Usuário'),
(1, 'Update', 'Atualizar Usuário'),
(1, 'Delete', 'Deletar Usuário'),
(1, 'Activate', 'Ativar Post'),
(1, 'Deactivate', 'Desativar Post'),
(2, 'IdentityInsert', 'Inserir Post'),
(2, 'IdentityInsertImage', 'Inserir Post Imagem'),
(2, 'Update', 'Atualizar Post'),
(2, 'UpdateImage', 'Alterar Post Imagem'),
(2, 'Delete', 'Deletar Post'),
(2, 'Activate', 'Ativar Post'),
(2, 'Deactivate', 'Desativar Post'),
(3, 'IdentityInsert', 'Inserir Categoria'),
(3, 'IdentityInsertImage', 'Inserir Categoria Imagem'),
(3, 'Update', 'Atualizar Categoria'),
(3, 'UpdateImage', 'Alterar Categoria Imagem'),
(3, 'Delete', 'Deletar Categoria'),
(3, 'Activate', 'Ativar Categria'),
(3, 'Deactivate', 'Desativar Categoria'),
(4, 'IdentityInsert', 'Inserir Newsletter'),
(4, 'Update', 'Atualizar Newsletter'),
(4, 'Delete', 'Deletar Newsletter'),
(4, 'Activate', 'Ativar Newsletter'),
(4, 'Deactivate', 'Desativar Newsletter'),
(5, 'IdentityInsert', 'Inserir Anuncio'),
(5, 'Update', 'Atualizar Anuncio'),
(5, 'Delete', 'Deletar Anuncio'),
(5, 'Activate', 'Ativar Anuncio'),
(5, 'Deactivate', 'Desativar Anuncio'),
(6, 'IdentityInsert', 'Inserir Galeria'),
(6, 'Update', 'Atualizar Galeria'),
(6, 'Delete', 'Deletar Galeria'),
(6, 'Activate', 'Ativar Galeria'),
(6, 'Deactivate', 'Desativar Galeria'),
(6, 'IdentityInsertImage', 'Inserir Galeria Imagem');

INSERT INTO `permissaousuario` (`UsuarioId`, `AcaoId`) VALUES
(1, 1),
(1, 2),
(1, 3),
(1, 4),
(1, 5),
(1, 6),
(1, 7),
(1, 8),
(1, 9),
(1, 10),
(1, 11),
(1, 12),
(1, 13),
(1, 14),
(1, 15),
(1, 16),
(1, 17),
(1, 18),
(1, 19),
(1, 20),
(1, 21),
(1, 22),
(1, 23),
(1, 24),
(1, 25),
(1, 26),
(1, 27),
(1, 28),
(1, 29),
(1, 30),
(1, 31),
(1, 32),
(1, 33),
(1, 34),
(1, 35);

-- Exportação de dados foi desmarcado.
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
