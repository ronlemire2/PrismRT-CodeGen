truncate table CodeGenProjectFolderItem
truncate table CodeGenProjectFolder
truncate table CodeGenProject
truncate table CodeGenSolution
truncate table CodeGenRules
truncate table CodeGenStrings

--DBCC CHECKIDENT('CodeGenSolution', RESEED, 1)
--DBCC CHECKIDENT('CodeGenProject', RESEED, 1)
--DBCC CHECKIDENT('CodeGenProjectFolder', RESEED, 1)
--DBCC CHECKIDENT('CodeGenProjectFolderItem', RESEED, 1)
--DBCC CHECKIDENT('CodeGenRules', RESEED, 1)
--DBCC CHECKIDENT('CodeGenStrings', RESEED, 1)
