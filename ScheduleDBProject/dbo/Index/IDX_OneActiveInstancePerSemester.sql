CREATE UNIQUE INDEX [IDX_OneActiveInstancePerSemester]
ON [dbo].[SemesterInstance]([SemesterId])
WHERE [SemesterStatus] = 'active';
