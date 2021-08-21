Select
	*
From LPR_PlateHits
Where
	best_uuid in
	(
		Select
			T1.best_uuid
		From
		(
			Select
				best_uuid,
				Count(*) as [Cnt]
			From LPR_PlateHits
			Group By best_uuid
		) as T1
		Where
			T1.Cnt > 1
	)
Order By
	best_uuid


Delete
From LPR_LocalInfo
Where
	UUID in
	(
		Select
			T1.UUID
		From
		(
			Select
				UUID,
				Count(*) as [Cnt]
			From LPR_LocalInfo
			Group By UUID
		) as T1
		Where
			T1.Cnt > 1
	)