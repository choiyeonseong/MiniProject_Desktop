USE [master]
GO
/****** Object:  Database [OpenApiLab]    Script Date: 2021-04-05 오후 4:44:57 ******/
CREATE DATABASE [OpenApiLab]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'OpenApiLab', FILENAME = N'D:\Data\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\OpenApiLab.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'OpenApiLab_log', FILENAME = N'D:\Data\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\OpenApiLab_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [OpenApiLab] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [OpenApiLab].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [OpenApiLab] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [OpenApiLab] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [OpenApiLab] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [OpenApiLab] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [OpenApiLab] SET ARITHABORT OFF 
GO
ALTER DATABASE [OpenApiLab] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [OpenApiLab] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [OpenApiLab] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [OpenApiLab] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [OpenApiLab] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [OpenApiLab] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [OpenApiLab] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [OpenApiLab] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [OpenApiLab] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [OpenApiLab] SET  DISABLE_BROKER 
GO
ALTER DATABASE [OpenApiLab] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [OpenApiLab] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [OpenApiLab] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [OpenApiLab] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [OpenApiLab] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [OpenApiLab] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [OpenApiLab] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [OpenApiLab] SET RECOVERY FULL 
GO
ALTER DATABASE [OpenApiLab] SET  MULTI_USER 
GO
ALTER DATABASE [OpenApiLab] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [OpenApiLab] SET DB_CHAINING OFF 
GO
ALTER DATABASE [OpenApiLab] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [OpenApiLab] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [OpenApiLab] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [OpenApiLab] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'OpenApiLab', N'ON'
GO
ALTER DATABASE [OpenApiLab] SET QUERY_STORE = OFF
GO
USE [OpenApiLab]
GO
/****** Object:  Table [dbo].[NaverFavoriteMovies]    Script Date: 2021-04-05 오후 4:44:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NaverFavoriteMovies](
	[Idx] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](1000) NOT NULL,
	[Link] [varchar](500) NULL,
	[Image] [varchar](500) NULL,
	[Subtitle] [varchar](1000) NULL,
	[PubDate] [varchar](20) NULL,
	[Director] [nvarchar](1000) NULL,
	[Actor] [nvarchar](1000) NULL,
	[UserRating] [varchar](10) NULL,
	[RegDate] [datetime] NULL,
 CONSTRAINT [PK_NaverFavoriteMovies] PRIMARY KEY CLUSTERED 
(
	[Idx] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [OpenApiLab] SET  READ_WRITE 
GO
