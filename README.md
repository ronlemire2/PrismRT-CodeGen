'PrismRT CodeGen' is a code generation system for setting up Line of Business 
Windows Store applications that use the 'Prism for Windows Runtime' guidance 
from the Microsoft Patterns and Practices group. 
(see http://msdn.microsoft.com/en-us/library/windows/apps/xx130643.aspx)  
It helps to reduce the repetitious procedures for starting such Windows Store 
applications. 

Documentation is included in the download at Docs->PrismRT CodeGen.pdf.

The code generation system is composed of:
1) PrismAddin (VS Addin Project)
2) PrismTemplates (VS Exported Project Templates zip files)
3) T4 Code Templates (Project Items of PrismTemplates)
4) SQL Server Database (Domain table plus a Rules table and a Strings table)

The Addin uses Visual Studio Automation (EnvDTE) to create a target application
from the PrismTemplates. Automation renames the generic .tt files to domain 
specific ones then runs a TransformAll on them to produce generated code
(.gc files). 
The Addin then removes the generic .tt files and renames the .gc files to .cs
files. After Addin completes a few manual steps need to be done (documented 
in pdf 'What you have to do' page. The Addin takes about 1-2 minutes to 
complete.

The generated application is a simple starter application based on a single
Domain table (e.g. Customers) in the SQL Server Database. The generated 
application UI consists of a MainPage that displays all the rows in the 
Domain table in a ListView and a DetailsPage that supports CRUD on the row 
selected in the ListView. All the Xaml, ViewModel, WebAPI and Data Layer code
is generated based on the Domain table using SMO(SQL Server Management Objects)
in the T4 code templates.

This is a work in progress. Future features will include:
1) One-to-many Domain tables
2) Code generation to add parts to the starter app
3) Better styling including 3rd party controls
4) Inclusion of more 'Prism for Windows Runtime' components