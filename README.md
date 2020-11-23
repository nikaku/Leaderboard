
# Leaderboard
[![LinkedIn][linkedin-shield]][linkedin-url]

<a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
      </ul>
      
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/nikakurdadze/

### Prerequisites

1. Install [AccessDatabaseEngine](https://download.microsoft.com/download/2/4/3/24375141-E08D-4803-AB0E-10F2E3A07AAA/AccessDatabaseEngine_X64.exe)
2. Create Database
   ```sql
   IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'LeaderBoard')
   BEGIN
    CREATE DATABASE LeaderBoard
   END
   USE [LeaderBoard]
   GO
   SET ANSI_NULLS ON
   GO
   SET QUOTED_IDENTIFIER ON
   GO
   CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](100) NULL,
      CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED 
      (
	[Id] ASC
      )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]) ON [PRIMARY]
      GO
      SET ANSI_NULLS ON
      GO
      SET QUOTED_IDENTIFIER ON
      GO
      CREATE TABLE [dbo].[UserScores](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[CreateDate] [datetime2](7) NULL,
	[UpdateDate] [datetime2](7) NULL,
	[ScoreDate] [datetime2](7) NULL,
	[Score] [int] NULL,
      CONSTRAINT [PK_UserScores] PRIMARY KEY CLUSTERED (
	[Id] ASC)
      WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
      ) ON [PRIMARY]
      GO
      ALTER TABLE [dbo].[UserScores]  WITH CHECK ADD  CONSTRAINT [FK_UserScores_Users] FOREIGN KEY([UserId])
      REFERENCES [dbo].[Users] ([Id])
      GO
      ALTER TABLE [dbo].[UserScores] CHECK CONSTRAINT [FK_UserScores_Users]
      GO

   ```
