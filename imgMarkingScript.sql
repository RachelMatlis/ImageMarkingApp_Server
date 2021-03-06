USE [PictureAppDB]
GO
/****** Object:  Table [dbo].[DocumentMarkers]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentMarkers](
	[docID] [nvarchar](50) NOT NULL,
	[markerID] [nvarchar](50) NOT NULL,
	[markerType] [nvarchar](50) NOT NULL,
	[markerLocation] [nvarchar](50) NOT NULL,
	[backColor] [nvarchar](50) NOT NULL,
	[userID] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_DocumentMarkers] PRIMARY KEY CLUSTERED 
(
	[markerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Documents]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Documents](
	[owner] [nchar](50) NOT NULL,
	[imageURL] [nvarchar](2083) NULL,
	[documentName] [nvarchar](50) NOT NULL,
	[docID] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Documents] PRIMARY KEY CLUSTERED 
(
	[docID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SharedDocuments]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SharedDocuments](
	[docID] [nvarchar](50) NOT NULL,
	[userID] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[userID] [nvarchar](50) NOT NULL,
	[nameID] [nvarchar](50) NOT NULL,
	[status] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[userID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[CreateDocument]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateDocument] @owner nvarchar(50), @imageURL nvarchar(2083),@documentName nvarchar(50),@docID nvarchar(50)
AS
INSERT INTO Documents VALUES(@owner, @imageURL,@documentName,@docID)
GO
/****** Object:  StoredProcedure [dbo].[CreateMarker]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateMarker] @docID nvarchar(50),@markerID nvarchar(50),@markerType nvarchar(50),
                              @markerLocation nvarchar(50),@backColor nvarchar(50),@userID nvarchar(50)
AS
INSERT INTO DocumentMarkers VALUES(@docID,@markerID,@markerType,@markerLocation,@backColor,@userID)
GO
/****** Object:  StoredProcedure [dbo].[CreateShare]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateShare] @docID nvarchar(50), @userID nvarchar(50)
AS
INSERT INTO SharedDocuments values(@docID,@userID)
GO
/****** Object:  StoredProcedure [dbo].[CreateUser]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateUser] @userID nvarchar(50), @userName nvarchar(50)
AS
INSERT INTO Users values(@userID,@userName,1)
GO
/****** Object:  StoredProcedure [dbo].[GetDocFromDocumentMarkers]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetDocFromDocumentMarkers] 
	-- Add the parameters for the stored procedure here
	@docID nvarchar(50)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * from DocumentMarkers where @docID =docID
END
GO
/****** Object:  StoredProcedure [dbo].[GetDocument]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetDocument] @docID nvarchar(30)
AS
SELECT * FROM Documents WHERE docID = @docID
GO
/****** Object:  StoredProcedure [dbo].[GetDocuments]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetDocuments]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * from Documents
END
GO
/****** Object:  StoredProcedure [dbo].[GetMarker]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetMarker]
	-- Add the parameters for the stored procedure here
	@markerID nvarchar(50)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM DocumentMarkers WHERE markerID=@markerID
END
GO
/****** Object:  StoredProcedure [dbo].[GetMarkers]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetMarkers] @docID nvarchar(50)
AS
SELECT * FROM DocumentMarkers WHERE docID=@docID
GO
/****** Object:  StoredProcedure [dbo].[GetSharedDoc]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetSharedDoc] 
	-- Add the parameters for the stored procedure here
	@docID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * from SharedDocuments where @docID= docID
END
GO
/****** Object:  StoredProcedure [dbo].[GetUser]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetUser] 
	-- Add the parameters for the stored procedure here
	@userID nvarchar(50)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select * from Users where userID = @userID
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserAndSharedDocument]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetUserAndSharedDocument] 
	-- Add the parameters for the stored procedure here
	@userID nvarchar(50),
	@docID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * from SharedDocuments where @userID = userID and @docID = docID
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserDocs]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetUserDocs]
	-- Add the parameters for the stored procedure here
	@userID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * from Documents where @userID= owner
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserDocument]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetUserDocument] 
	-- Add the parameters for the stored procedure here
	@userID nvarchar(50),
	@docID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * from Documents where @userID=owner and @docID=docID 
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserSharedDocuments]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetUserSharedDocuments] 
	-- Add the parameters for the stored procedure here
	@userID nvarchar(50)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * from SharedDocuments where @userID =userID
END
GO
/****** Object:  StoredProcedure [dbo].[RemoveDocFromDocumentMarkers]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RemoveDocFromDocumentMarkers]
	-- Add the parameters for the stored procedure here
	@docID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM DocumentMarkers WHERE @docID=docID
END
GO
/****** Object:  StoredProcedure [dbo].[RemoveDocument]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RemoveDocument] @docID nvarchar(50)
AS
DELETE FROM Documents WHERE docID=@docID
GO
/****** Object:  StoredProcedure [dbo].[RemoveMarker]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RemoveMarker] @markerID nvarchar(50)
AS
DELETE FROM DocumentMarkers WHERE markerID =@markerID
GO
/****** Object:  StoredProcedure [dbo].[RemoveShare]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RemoveShare] @docID nvarchar(50)
AS
DELETE FROM SharedDocuments WHERE docID = @docID
GO
/****** Object:  StoredProcedure [dbo].[RemoveUser]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RemoveUser] @userID nvarchar(50)
AS
DELETE FROM Users WHERE userID = @userID
GO
/****** Object:  StoredProcedure [dbo].[UnsubscribeUser]    Script Date: 7/12/2021 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UnsubscribeUser] @userID nvarchar(50)
AS
UPDATE Users 
SET status = 0
WHERE userID=@userID
GO
